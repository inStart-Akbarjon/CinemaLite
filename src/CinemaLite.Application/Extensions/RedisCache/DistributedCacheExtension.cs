using StackExchange.Redis;

namespace CinemaLite.Application.Extensions.RedisCache;

public static class DistributedCacheExtension
{
    public static async Task InvalidateAsync(this IConnectionMultiplexer redis, string registry)
    {
        var db = redis.GetDatabase();
        
        var members = await db.SetMembersAsync(registry);

        var keys = members
            .Select(x => new RedisKey(x))
            .ToArray();

        if (keys.Length > 0)
        {
            await db.KeyDeleteAsync(keys);
        }
        
        await db.KeyDeleteAsync(registry);
    }
}