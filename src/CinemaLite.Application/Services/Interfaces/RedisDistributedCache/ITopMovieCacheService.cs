using CinemaLite.Application.DTOs.Movie.Response;
using CinemaLite.Application.DTOs.Pagination;

namespace CinemaLite.Application.Services.Interfaces.RedisDistributedCache;

public interface ITopMovieCacheService
{
    Task<PaginatedMovieList<GetAllMoviesResponse>?> GetTopMoviesFromCacheAsync(string cacheKey);
    Task AddTopMoviesToCacheAsync(string cacheKey, PaginatedMovieList<GetAllMoviesResponse> movies);
}