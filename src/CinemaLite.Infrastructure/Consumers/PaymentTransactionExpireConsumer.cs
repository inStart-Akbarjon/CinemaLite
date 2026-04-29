using CinemaLite.Application.Models.Notification;
using CinemaLite.Contracts.Events;
using CinemaLite.Domain.Enums;
using CinemaLite.Domain.Models;
using CinemaLite.Infrastructure.Database;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CinemaLite.Infrastructure.Consumers;

public class PaymentTransactionExpireConsumer(IServiceScopeFactory scopeFactory) : IConsumer<PaymentTransactionCreatedEvent>
{
    public async Task Consume(ConsumeContext<PaymentTransactionCreatedEvent> context)
    {
        using var scope = scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var publishEndpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        
        await using var transaction = await dbContext.Database.BeginTransactionAsync();

        try
        {
            var paymentTransaction = await dbContext.PaymentTransactions.FirstOrDefaultAsync(p => p.Id == context.Message.PaymentId && p.Status == PaymentStatus.Initiated);

            if (paymentTransaction != null)
            {
                paymentTransaction.Status = PaymentStatus.Expired;

                var user = await userManager.FindByIdAsync(paymentTransaction.UserId.ToString());

                if (user != null)
                {
                    var message = new PaymentMessages();

                    await publishEndpoint.Publish(new EmailNotificationEvent()
                    {
                        UserEmail = user.Email,
                        Message = message.PaymentExpired,
                        Subject = $"Payment expired"
                    });
                }

                dbContext.PaymentTransactions.Update(paymentTransaction);
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