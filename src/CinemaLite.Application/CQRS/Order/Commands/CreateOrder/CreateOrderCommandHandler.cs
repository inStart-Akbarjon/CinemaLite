using CinemaLite.Application.DTOs.Order.Response;
using CinemaLite.Application.Exceptions.Cart;
using CinemaLite.Application.Interfaces.DbContext;
using CinemaLite.Application.Services.Interfaces.Auth;
using CinemaLite.Contracts.Events;
using CinemaLite.Domain.Enums;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CinemaLite.Application.CQRS.Order.Commands.CreateOrder;

public class CreateOrderCommandHandler(
    IAppDbContext dbContext, 
    ICurrentUserService currentUserService,
    ISendEndpointProvider sendEndpointProvider) : IRequestHandler<CreateOrderCommand, CreateOrderResponse>
{
    public async Task<CreateOrderResponse> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;
        
        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

        var endpoint = await sendEndpointProvider.GetSendEndpoint(
            new Uri("queue:reservation.expire.delay.queue"));
        
        try
        {
            var cart = await dbContext.Carts.FirstOrDefaultAsync(c => c.Id == request.CartId, cancellationToken);

            if (cart == null)
            {
                throw new CartNotFoundException(request.CartId);
            }

            var seatReservations = await dbContext.SeatReservations
                .Where(s => s.CartId == request.CartId)
                .ToListAsync(cancellationToken);
            
            var orderId = Guid.NewGuid();

            var random = new Random();
            
            var order = new Domain.Models.Order()
            {
                Id = orderId,
                OrderNumber = random.Next(1000, 1000000000),
                CustomerId = userId,
                TotalPrice = cart.TotalPrice,
                Status = OrderStatus.Pending
            };
            
            await dbContext.Orders.AddAsync(order, cancellationToken);

            foreach (var seatReservation in seatReservations)
            {
                seatReservation.OrderId = orderId;
                seatReservation.CartId = null;
                
                dbContext.SeatReservations.Update(seatReservation);
            }
            
            await endpoint.Send(new OrderCreatedEvent()
            {
                OrderId = orderId
            }, cancellationToken);

            dbContext.Carts.Remove(cart);
            await dbContext.SaveChangesAsync(cancellationToken);
            
            await transaction.CommitAsync(cancellationToken);

            return new CreateOrderResponse()
            {
                Id = order.Id,
                Status = order.Status,
                TotalPrice = order.TotalPrice,
            };
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}