using CinemaLite.Application.CQRS.Movie.Events;
using CinemaLite.Application.Models.Cache;
using MassTransit;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace CinemaLite.Infrastructure.Consumers.CacheInvalidation;

public class MovieCacheInvalidationConsumer(
    IConnectionMultiplexer redis,
    ILogger<MovieCacheInvalidationConsumer> logger) : IConsumer<MovieCacheInvalidationEvent>
{
    public async Task Consume(ConsumeContext<MovieCacheInvalidationEvent> context)
    {
        logger.LogInformation("MovieCreatedConsumer: {message}", context.Message);
        
        var db = redis.GetDatabase();
        
        var members = await db.SetMembersAsync(MoviesCacheKeys.Registry);

        var keys = members
            .Select(x => new RedisKey(x))
            .ToArray();

        if (keys.Length > 0)
        {
            await db.KeyDeleteAsync(keys);
        }
        
        await db.KeyDeleteAsync(MoviesCacheKeys.Registry);
    }
}