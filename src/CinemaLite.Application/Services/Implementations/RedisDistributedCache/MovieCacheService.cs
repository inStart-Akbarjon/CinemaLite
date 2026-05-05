using System.Text;
using CinemaLite.Application.DTOs.Movie.Response;
using CinemaLite.Application.DTOs.Pagination;
using CinemaLite.Application.Models.Cache;
using CinemaLite.Application.Services.Interfaces.RedisDistributedCache;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace CinemaLite.Application.Services.Implementations.RedisDistributedCache;

public class MovieCacheService(IDistributedCache cache, IConnectionMultiplexer redis) : IMovieCacheService
{
    public async Task<PaginatedQueryList<GetAllMoviesResponse>?> GetAllMoviesFromCacheAsync(string cacheKey)
    {
        var cached = await cache.GetStringAsync(cacheKey);
        
        if (cached != null)
        {
            return JsonConvert.DeserializeObject<PaginatedQueryList<GetAllMoviesResponse>?>(cached);
        }
        
        return null;
    }

    public async Task AddMoviesToCacheAsync(string cacheKey, PaginatedQueryList<GetAllMoviesResponse> movies)
    {
        var options = new DistributedCacheEntryOptions()
        {
            SlidingExpiration = TimeSpan.FromMinutes(5),
        };
        
        var bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(movies));
        await cache.SetAsync(cacheKey, bytes, options);
        
        var db = redis.GetDatabase();
        await db.SetAddAsync(
            MoviesCacheKeys.Registry,
            cacheKey);
    }
}