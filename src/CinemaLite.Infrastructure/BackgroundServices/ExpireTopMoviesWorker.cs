using CinemaLite.Application.Extensions.RedisCache;
using CinemaLite.Application.Models.Cache;
using CinemaLite.Domain.Enums;
using CinemaLite.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace CinemaLite.Infrastructure.BackgroundServices;

public class ExpireTopMoviesWorker(
    IServiceScopeFactory scopeFactory, 
    ILogger<ExpireSessionWorker> logger, 
    IConnectionMultiplexer redis) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        do
        {
            using var scope = scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var nextRun = DateTime.Today.AddHours(23).AddMinutes(30);
            if (nextRun < DateTime.Now)
            {
                nextRun = nextRun.AddDays(1);
            }

            var waitTime = nextRun - DateTime.Now;

            await Task.Delay(waitTime, stoppingToken);

            logger.LogInformation("Expire TopMovies worker started at {time}", DateTimeOffset.Now);
            
            var movies = await dbContext.Movies.Where(m => m.DeletedAt == null && m.Status == MovieStatus.Published && m.IsTop)
                .ToListAsync(stoppingToken);
            
            foreach (var movie in movies)
            {
                if (movie.TopSubscriptionStartDate?.AddDays(movie.TopSubscriptionPeriod) < DateTime.Now)
                {
                    movie.IsTop = false;
                    movie.TopSubscriptionPeriod = 0;
                    movie.TopSubscriptionStartDate = null;
                }
                
                dbContext.Movies.Update(movie);
            }
            
            await redis.InvalidateAsync(MoviesCacheKeys.Registry);
            await redis.InvalidateAsync(TopMoviesCacheKeys.Registry);
            
            await dbContext.SaveChangesAsync(stoppingToken);

            logger.LogInformation("Expire TopMovies worker ended job at {time}", DateTimeOffset.Now);
            
        } while (!stoppingToken.IsCancellationRequested);
    }
}