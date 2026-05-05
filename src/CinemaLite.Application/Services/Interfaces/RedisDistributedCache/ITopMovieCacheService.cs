using CinemaLite.Application.DTOs.Movie.Response;
using CinemaLite.Application.DTOs.Pagination;

namespace CinemaLite.Application.Services.Interfaces.RedisDistributedCache;

public interface ITopMovieCacheService
{
    Task<PaginatedQueryList<GetTopMoviesResponse>?> GetTopMoviesFromCacheAsync(string cacheKey);
    Task AddTopMoviesToCacheAsync(string cacheKey, PaginatedQueryList<GetTopMoviesResponse> movies);
}