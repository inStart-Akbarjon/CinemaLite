using System.Text;
using CinemaLite.Application.Models.Cache;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace CinemaLite.Application.Extensions.RedisCache;

public static class DistributedCacheExtension
{
    public static async Task<T?> GetOrSetAsync<T>(
        this IDistributedCache cache, 
        string cacheKey, 
        Func<Task<T>> task)
    {
        var options = new DistributedCacheEntryOptions()
        {
            SlidingExpiration = TimeSpan.FromMinutes(5),
        };

        var cached = await cache.GetStringAsync(cacheKey);

        if (cached != null)
        {
            return JsonConvert.DeserializeObject<T>(cached);
        }

        var movies = await task();

        var bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(movies));
        await cache.SetAsync(cacheKey, bytes, options);

        return movies;
    }

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