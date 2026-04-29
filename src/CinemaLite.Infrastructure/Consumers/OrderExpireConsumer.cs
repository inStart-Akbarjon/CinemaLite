using CinemaLite.Application.Models.Notification;
using CinemaLite.Application.Services.Interfaces.Common;
using CinemaLite.Contracts.Events;
using CinemaLite.Domain.Enums;
using CinemaLite.Domain.Models;
using CinemaLite.Infrastructure.Database;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CinemaLite.Infrastructure.Consumers;

public class OrderExpireConsumer(IServiceScopeFactory scopeFactory) : IConsumer<OrderCreatedEvent>
{
    public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
    {
        using var scope = scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var openSeatsStatus = scope.ServiceProvider.GetRequiredService<IOpenSeatsStatus>();
        var publishEndpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        
        await using var transaction = await dbContext.Database.BeginTransactionAsync();

        try
        {
            var order = await dbContext.Orders.FirstOrDefaultAsync(o => o.Id == context.Message.OrderId && o.Status == OrderStatus.Pending);

            if (order != null)
            {
                order.Status = OrderStatus.Expired;

                var user = await userManager.FindByIdAsync(order.CustomerId.ToString());

                if (user != null)
                {
                    var message = new ExpirationMessages();

                    await publishEndpoint.Publish(new EmailNotificationEvent()
                    {
                        UserEmail = user.Email,
                        Message = message.OrderExpirationMessage,
                        Subject = $"Order #{order.OrderNumber} expired"
                    });
                }
            
                await openSeatsStatus.OpenSeatsStatusFromOrder(context.Message.OrderId, context.CancellationToken);

                dbContext.Orders.Update(order);
                await dbContext.SaveChangesAsync();
            }
            
            await transaction.CommitAsync();
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}