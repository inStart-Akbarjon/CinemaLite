using CinemaLite.Application.DTOs.Movie.Response;
using CinemaLite.Application.DTOs.Pagination;
using CinemaLite.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CinemaLite.Application.Extensions.Pagination;

public static class PaginationExtension
{
    public static async Task<PaginatedMovieList<T>> PaginateAsync<T>(
        this IQueryable<T> queryable, 
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var paginatedItems =
            await queryable
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

        return new PaginatedMovieList<T>()
        {
            Movies = paginatedItems,
            PageNumber = pageNumber + 1,
            PageSize = pageSize,
            HasNextPage = paginatedItems.Count == pageSize,
        };
    }

    public static IQueryable<GetAllMoviesResponse> ToGetAllMoviesResponse(
        this IQueryable<Movie> queryable)
    {
        var allMoviesResponses = queryable.Select(x =>
            new GetAllMoviesResponse()
            {
                Id = x.Id,
                Title = x.Title,
                DurationMinutes = x.DurationMinutes,
                Status = x.Status,
                MinPrice = x.MinPrice,
                IsTop = x.IsTop,
                TopSubscriptionPeriod = x.TopSubscriptionPeriod,
                Genre = x.Genre,
            });

        return allMoviesResponses;
    }
}