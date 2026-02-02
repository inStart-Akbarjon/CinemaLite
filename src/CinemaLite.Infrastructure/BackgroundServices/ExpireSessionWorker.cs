using CinemaLite.Domain.Enums;
using CinemaLite.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CinemaLite.Infrastructure.BackgroundServices;

public class ExpireSessionWorker(IServiceScopeFactory scopeFactory, ILogger<ExpireSessionWorker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        do
        {
            using var scope = scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var nextRun = DateTime.Today.AddHours(1);
            if (nextRun < DateTime.Now)
            {
                nextRun = nextRun.AddDays(1);
            }

            var waitTime = nextRun - DateTime.Now;

            await Task.Delay(waitTime, stoppingToken);

            logger.LogInformation("Worker started at {time}", DateTimeOffset.Now);
            
            var movies = await dbContext.Movies.Where(m => m.DeletedAt == null && m.Status == MovieStatus.Published)
                .ToListAsync(stoppingToken);
            
            foreach (var movie in movies)
            {
                foreach (var session in movie.Sessions.Where(session =>
                             session.DeletedAt == null && session.StartTime <= DateTime.Now))
                {
                    session.DeletedAt = DateTime.Now;
                }

                var nonExpiredSessions = movie.Sessions.Where(s => s.DeletedAt == null).ToList();

                if (nonExpiredSessions.Count == 0)
                {
                    movie.Status = MovieStatus.UnPublished;
                }

                dbContext.Movies.Update(movie);
            }
            
            await dbContext.SaveChangesAsync(stoppingToken);

            logger.LogInformation("Worker ended job at {time}", DateTimeOffset.Now);
            
        } while (!stoppingToken.IsCancellationRequested);
    }
}