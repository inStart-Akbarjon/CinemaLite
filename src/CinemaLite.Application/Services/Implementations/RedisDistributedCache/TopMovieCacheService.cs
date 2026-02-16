using System.Text;
using CinemaLite.Application.DTOs.Movie.Response;
using CinemaLite.Application.DTOs.Pagination;
using CinemaLite.Application.Models.Cache;
using CinemaLite.Application.Services.Interfaces.RedisDistributedCache;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace CinemaLite.Application.Services.Implementations.RedisDistributedCache;

public class TopMovieCacheService(IDistributedCache cache, IConnectionMultiplexer redis) : ITopMovieCacheService
{
    public async Task<PaginatedMovieList<GetTopMoviesResponse>?> GetTopMoviesFromCacheAsync(string cacheKey)
    {
        var cached = await cache.GetStringAsync(cacheKey);

        if (cached != null)
        {
            return JsonConvert.DeserializeObject<PaginatedMovieList<GetTopMoviesResponse>?>(cached);
        }
        
        return null;
    }

    public async Task AddTopMoviesToCacheAsync(string cacheKey, PaginatedMovieList<GetTopMoviesResponse> movies)
    {
        var options = new DistributedCacheEntryOptions()
        {
            SlidingExpiration = TimeSpan.FromMinutes(5),
        };
        
        var bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(movies));
        await cache.SetAsync(cacheKey, bytes, options);
        
        var db = redis.GetDatabase();
        await db.SetAddAsync(
            TopMoviesCacheKeys.Registry,
            cacheKey);
    }
}