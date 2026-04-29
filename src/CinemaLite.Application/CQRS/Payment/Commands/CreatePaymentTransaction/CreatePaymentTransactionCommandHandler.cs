using CinemaLite.Application.Common.Stripe;
using CinemaLite.Application.DTOs.Payment.Response;
using CinemaLite.Application.Exceptions.Order;
using CinemaLite.Application.Interfaces.DbContext;
using CinemaLite.Application.Services.Interfaces.Auth;
using CinemaLite.Contracts.Events;
using CinemaLite.Domain.Enums;
using CinemaLite.Domain.Models;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Stripe;

namespace CinemaLite.Application.CQRS.Payment.Commands.CreatePaymentTransaction;

public class CreatePaymentTransactionCommandHandler(
    IAppDbContext dbContext, 
    IOptions<StripeSettings> stripeSettings,
    ICurrentUserService currentUserService,
    ISendEndpointProvider sendEndpointProvider) : IRequestHandler<CreatePaymentTransactionCommand, CreatePaymentResponse>
{
    public async Task<CreatePaymentResponse> Handle(CreatePaymentTransactionCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;
        var userPhone = currentUserService.UserPhone;
        var userEmail = currentUserService.UserEmail;
        
        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

        var endpoint = await sendEndpointProvider.GetSendEndpoint(
            new Uri("queue:reservation.expire.delay.queue"));
            
        try
        {
            var order = await dbContext.Orders.FirstOrDefaultAsync(o => o.Id == request.OrderId && o.Status == OrderStatus.Pending, cancellationToken);

            var paymentTransaction = await dbContext.PaymentTransactions.FirstOrDefaultAsync(p => p.OrderId == request.OrderId, cancellationToken);
            
            if (order == null)
            {
                throw new OrderNotFoundException(request.OrderId);
            }

            if (paymentTransaction != null)
            {
                throw new OrderPaymentTransactionException(request.OrderId);
            }
            
            var stripeOptions = new PaymentIntentCreateOptions
            {
                Amount = (long) order.TotalPrice * 100,
                Currency = "usd",
                AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
                {
                    Enabled = true,
                },
            };

            var client = new StripeClient(stripeSettings.Value.SecretKey);
            
            var service = client.V1.PaymentIntents;
            var paymentIntent = await service.CreateAsync(stripeOptions, cancellationToken: cancellationToken);
            
            var payment = new PaymentTransaction()
            {
                UserId = userId,
                OrderId = order.Id,
                TotalAmount = order.TotalPrice,
                Status = PaymentStatus.Initiated,
                PaymentIntentId = paymentIntent.Id,
                ClientSecret = paymentIntent.ClientSecret,
                UserEmail = userEmail,
                UserPhone = userPhone,
            };
            
            await dbContext.PaymentTransactions.AddAsync(payment, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            
            await endpoint.Send(new PaymentTransactionCreatedEvent()
            {
                PaymentId = payment.Id
            }, cancellationToken);
            
            await transaction.CommitAsync(cancellationToken);

            return new CreatePaymentResponse()
            {
                Id = payment.Id,
                OrderId = payment.OrderId,
                TotalAmount = payment.TotalAmount,
                Status = payment.Status,
                ClientSecret = paymentIntent.ClientSecret,
                PaymentIntentId = paymentIntent.Id
            };
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}