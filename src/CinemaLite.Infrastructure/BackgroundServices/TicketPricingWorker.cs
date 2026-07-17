using CinemaLite.Domain.Enums;
using CinemaLite.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CinemaLite.Infrastructure.BackgroundServices;

public class TicketPricingWorker(
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
            
            logger.LogInformation("TicketPricing worker started at {time}", DateTimeOffset.Now);
            
            var movies = await dbContext.Movies
                .Where(m => m.DeletedAt == null && m.Status == MovieStatus.Published)
                .ToListAsync(stoppingToken);
            
            var today = DateTime.Now.Date;
            
            foreach (var movie in movies)
            {
                var sessions = movie.Sessions.ToList();    
                
                foreach (var session in sessions)
                {
                    var daysUntilMovie = (session.StartTime.Date - today).TotalDays;
                    
                    if (daysUntilMovie <= 2)
                    {
                        session.Price *= 1.5m;
                    }
                }
                
                dbContext.Movies.Update(movie);
            }
            
            await dbContext.SaveChangesAsync(stoppingToken);

            logger.LogInformation("TicketPricing worker ended job at {time}", DateTimeOffset.Now);
            
        } while (!stoppingToken.IsCancellationRequested);
    }
}
