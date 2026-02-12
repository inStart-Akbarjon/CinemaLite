using CinemaLite.Application.DTOs.Movie.Response;
using CinemaLite.Application.DTOs.Pagination;
using CinemaLite.Application.Extensions.Pagination;
using CinemaLite.Application.Interfaces.DbContext;
using CinemaLite.Application.Models.Cache;
using CinemaLite.Application.Services.Interfaces.RedisDistributedCache;
using CinemaLite.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CinemaLite.Application.CQRS.Movie.Queries.GetTopMovies;

public class GetTopMoviesQueryHandler(IAppDbContext dbContext, ITopMovieCacheService topMovieCacheService) : IRequestHandler<GetTopMoviesQuery, PaginatedMovieList<GetAllMoviesResponse>>
{
    public async Task<PaginatedMovieList<GetAllMoviesResponse>> Handle(GetTopMoviesQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = TopMoviesCacheKeys.Page(request.PageNumber, request.PageSize);
        
        var cachedMovies = await topMovieCacheService.GetTopMoviesFromCacheAsync(cacheKey);

        if (cachedMovies != null)
        {
            return cachedMovies;
        }
        
        var movies = await dbContext.Movies
            .AsNoTracking()
            .Where(m => m.DeletedAt == null && m.Status == MovieStatus.Published && m.IsTop == true)
            .ToGetAllMoviesResponse()
            .PaginateAsync(request.PageNumber, request.PageSize, cancellationToken);
            
        await topMovieCacheService.AddTopMoviesToCacheAsync(cacheKey, movies);
        
        return movies;
    }
}