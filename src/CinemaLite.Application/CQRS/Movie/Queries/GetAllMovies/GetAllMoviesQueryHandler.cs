using CinemaLite.Application.DTOs.Movie.Response;
using CinemaLite.Application.DTOs.Pagination;
using CinemaLite.Application.Extensions.Pagination;
using CinemaLite.Application.Interfaces.DbContext;
using CinemaLite.Application.Models.Cache;
using CinemaLite.Application.Services.Interfaces.RedisDistributedCache;
using CinemaLite.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CinemaLite.Application.CQRS.Movie.Queries.GetAllMovies;

public class GetAllMoviesQueryHandler(
    IAppDbContext dbContext, 
    IMovieCacheService movieCacheService) : IRequestHandler<GetAllMoviesQuery, PaginatedMovieList<GetAllMoviesResponse>>
{
    public async Task<PaginatedMovieList<GetAllMoviesResponse>> Handle(GetAllMoviesQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = MoviesCacheKeys.Page(request.PageNumber, request.PageSize);
        
        var cachedMovies = await movieCacheService.GetAllMoviesFromCacheAsync(cacheKey);

        if (cachedMovies != null)
        {
            return cachedMovies;
        }
        
        var movies = await dbContext.Movies
            .AsNoTracking()
            .Where(m => m.DeletedAt == null && m.Status == MovieStatus.Published)
            .ToGetAllMoviesResponse()
            .PaginateAsync(request.PageNumber, request.PageSize, cancellationToken);
            
        await movieCacheService.AddMoviesToCacheAsync(cacheKey, movies);
        
        return movies;
    }
}