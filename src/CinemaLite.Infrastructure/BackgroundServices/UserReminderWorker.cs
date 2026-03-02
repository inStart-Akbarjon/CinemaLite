using CinemaLite.Application.Models.Notification.UserReminderMessages;
using CinemaLite.Application.Services.Implementations.RedisDistributedLock;
using CinemaLite.Contracts.Events;
using CinemaLite.Domain.Models;
using CinemaLite.Infrastructure.Database;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CinemaLite.Infrastructure.BackgroundServices;

public class UserReminderWorker(
    IServiceScopeFactory scopeFactory,
    IDistributedLockService distributedLockService) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        do
        {
            using var scope = scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var publishEndpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();

            var nextRun = DateTime.Today.AddHours(12).AddMinutes(52);
            if (nextRun < DateTime.Now)
            {
                nextRun = nextRun.AddDays(1);
            }

            var waitTime = nextRun - DateTime.Now;

            await Task.Delay(waitTime, stoppingToken);

            var lockKey = $"lock:user-reminder-worker";
            
            using var workerLock = await distributedLockService.AcquireAsync(
                lockKey,
                TimeSpan.FromMinutes(10),
                stoppingToken);
            
            if (workerLock != null)
            {
                var tickets = await dbContext.Tickets
                    .Where(m => m.DeletedAt == null)
                    .ToListAsync(cancellationToken: stoppingToken);
            
                async Task SendReminder(Ticket ticket, string message)
                {
                    await publishEndpoint.Publish(new UserEmailReminderEvent
                    {
                        UserEmail = ticket.UserEmail,
                        Message = message,
                        Subject = "CinemaApp Reminder"
                    }, stoppingToken);
                }
            
                foreach (var ticket in tickets)
                {
                    if (ticket.StartTime.Date == DateTime.Today.AddDays(5))
                    {
                        var message = new FiveDaysBeforeMovieReminderMessage(ticket.MovieTitle, ticket.StartTime).Message;
                    
                        await SendReminder(ticket, message);
                    }
                
                    if (ticket.StartTime.Date == DateTime.Today.AddDays(3))
                    {
                        var message = new ThreeDaysBeforeMovieReminderMessage(ticket.MovieTitle, ticket.StartTime).Message;
                    
                        await SendReminder(ticket, message);
                    }
                
                    if (ticket.StartTime.Date == DateTime.Today)
                    {
                        var message = new ZeroDayBeforeMovieReminderMessage(ticket.MovieTitle, ticket.StartTime).Message;
                    
                        await SendReminder(ticket, message);
                    }
                }
            }

        } while (!stoppingToken.IsCancellationRequested);
    }
}