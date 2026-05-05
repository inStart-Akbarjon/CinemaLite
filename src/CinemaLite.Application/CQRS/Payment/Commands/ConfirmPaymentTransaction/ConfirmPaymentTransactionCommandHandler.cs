using CinemaLite.Application.Interfaces.DbContext;
using CinemaLite.Application.Models.Notification;
using CinemaLite.Contracts.Events;
using CinemaLite.Domain.Enums;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaLite.Application.CQRS.Payment.Commands.ConfirmPaymentTransaction;

public class ConfirmPaymentTransactionCommandHandler(
    IAppDbContext dbContext,
    IPublishEndpoint publishEndpoint) : IRequestHandler<ConfirmPaymentTransactionCommand, IActionResult>
{
    public async Task<IActionResult> Handle(ConfirmPaymentTransactionCommand request,
        CancellationToken cancellationToken)
    {
        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var paymentTransaction =
                await dbContext.PaymentTransactions
                    .FirstOrDefaultAsync(p => p.PaymentIntentId == request.PaymentIntentId, cancellationToken);

            if (paymentTransaction == null)
            {
                return new NotFoundResult();
            }

            var paymentMessages = new PaymentMessages();

            if (request.Status == PaymentStatus.Succeed)
            {
                var order = await dbContext.Orders
                    .FirstOrDefaultAsync(o => o.Id == paymentTransaction.OrderId && o.Status == OrderStatus.Pending, cancellationToken);

                if (order != null)
                {
                    order.Status = OrderStatus.Paid;
                    dbContext.Orders.Update(order);

                    var seatReservations = await dbContext.SeatReservations
                        .Where(r => r.OrderId == order.Id)
                        .ToListAsync(cancellationToken);

                    foreach (var seatReservation in seatReservations)
                    {
                        var ticket = new Domain.Models.Ticket()
                        {
                            UserId = paymentTransaction.UserId,
                            UserPhone = paymentTransaction.UserPhone,
                            UserEmail = paymentTransaction.UserEmail,
                            MovieId = seatReservation.MovieId,
                            SessionId = seatReservation.SessionId,
                            MovieTitle = seatReservation.MovieTitle,
                            CinemaName = seatReservation.CinemaName,
                            SeatRow = seatReservation.SeatRow,
                            SeatNumber = seatReservation.SeatNumber,
                            StartTime = seatReservation.StartTime,
                            PricePaid = seatReservation.PricePaid,
                        };

                        await dbContext.Tickets.AddAsync(ticket, cancellationToken);
                    }

                    paymentTransaction.Status = request.Status;
                    
                    await publishEndpoint.Publish(new EmailNotificationEvent()
                    {
                        Message = paymentMessages.PaymentSucceed,
                        UserEmail = paymentTransaction.UserEmail,
                        Subject = "Payment Succeed",
                    }, cancellationToken);
                }
            }
            else if (request.Status == PaymentStatus.Failed)
            {
                var order = await dbContext.Orders.FirstOrDefaultAsync(o => o.Id == paymentTransaction.OrderId && o.Status == OrderStatus.Pending,
                    cancellationToken);

                if (order != null)
                {
                    order.Status = OrderStatus.Failed;
                    dbContext.Orders.Update(order);

                    paymentTransaction.Status = request.Status;
                    
                    await publishEndpoint.Publish(new EmailNotificationEvent()
                    {
                        Message = paymentMessages.PaymentFailed,
                        UserEmail = paymentTransaction.UserEmail,
                        Subject = "Payment Failed",
                    }, cancellationToken);
                }
            }

            dbContext.PaymentTransactions.Update(paymentTransaction);
            await dbContext.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            return new OkResult();
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}