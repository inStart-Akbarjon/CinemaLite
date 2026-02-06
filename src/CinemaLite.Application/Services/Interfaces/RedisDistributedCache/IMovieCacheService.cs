using CinemaLite.Application.DTOs.Movie.Response;
using CinemaLite.Application.DTOs.Pagination;

namespace CinemaLite.Application.Services.Interfaces.RedisDistributedCache;

public interface IMovieCacheService
{
    Task<PaginatedMovieList<GetAllMoviesResponse>?> GetAllMoviesFromCacheAsync(string cacheKey);
    Task AddMoviesToCacheAsync(string cacheKey, PaginatedMovieList<GetAllMoviesResponse> movies);
}