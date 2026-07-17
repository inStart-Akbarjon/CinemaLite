using CinemaLite.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CinemaLite.Infrastructure.BackgroundServices;

public class ExpireTicketWorker(
    IServiceScopeFactory scopeFactory, 
    ILogger<ExpireSessionWorker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        do
        {
            using var scope = scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var nextRun = DateTime.Today.AddHours(1).AddMinutes(00);
            if (nextRun < DateTime.Now)
            {
                nextRun = nextRun.AddDays(1);
            }

            var waitTime = nextRun - DateTime.Now;

            await Task.Delay(waitTime, stoppingToken);
            
            logger.LogInformation("Expire Ticket worker started at {time}", DateTimeOffset.Now);
            
            var expiredTickets = await dbContext.Tickets
                .Where(t => t.StartTime <= DateTime.UtcNow)
                .ToListAsync(stoppingToken);

            foreach (var ticket in expiredTickets)
            {
                ticket.SoftDelete();
            }
            
            await dbContext.SaveChangesAsync(stoppingToken);

            logger.LogInformation("Expire Ticket worker ended job at {time}", DateTimeOffset.Now);
            
        } while (!stoppingToken.IsCancellationRequested);
    }
}