using CinemaLite.Application.DTOs.Movie.Response;
using CinemaLite.Application.DTOs.Pagination;
using CinemaLite.Application.Extensions.Pagination;
using CinemaLite.Application.Extensions.RedisCache;
using CinemaLite.Application.Interfaces.DbContext;
using CinemaLite.Application.Models.Cache;
using CinemaLite.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;

namespace CinemaLite.Application.CQRS.Movie.Queries.GetAllMovies;

public class GetAllMoviesQueryHandler(IAppDbContext dbContext, IDistributedCache cache, IConnectionMultiplexer redis) : IRequestHandler<GetAllMoviesQuery, PaginatedMovieList<GetAllMoviesResponse>>
{
    public async Task<PaginatedMovieList<GetAllMoviesResponse>> Handle(GetAllMoviesQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = MoviesCacheKeys.Page(request.PageNumber, request.PageSize);

        var result = await cache.GetOrSetAsync(
            cacheKey,
            async () =>
            {
                var movies = await dbContext.Movies
                    .AsNoTracking()
                    .Where(m => m.DeletedAt == null && m.Status == MovieStatus.Published)
                    .ToGetAllMoviesResponse()
                    .PaginateAsync(request.PageNumber, request.PageSize, cancellationToken);
                
                var db = redis.GetDatabase();

                await db.SetAddAsync(
                    MoviesCacheKeys.Registry,
                    cacheKey);
                
                return movies;
            });

        return result;
    }
}