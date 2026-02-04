using System.Text;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

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
}