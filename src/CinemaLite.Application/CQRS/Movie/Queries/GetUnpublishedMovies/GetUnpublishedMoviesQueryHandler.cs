using CinemaLite.Application.DTOs.Movie.Response;
using CinemaLite.Application.DTOs.Pagination;
using CinemaLite.Application.Extensions.Pagination;
using CinemaLite.Application.Interfaces.DbContext;
using CinemaLite.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CinemaLite.Application.CQRS.Movie.Queries.GetUnpublishedMovies;

public class GetUnpublishedMoviesQueryHandler(IAppDbContext dbContext) : IRequestHandler<GetUnpublishedMoviesQuery, PaginatedMovieList<GetAllMoviesResponse>>
{
    public async Task<PaginatedMovieList<GetAllMoviesResponse>> Handle(GetUnpublishedMoviesQuery request, CancellationToken cancellationToken)
    {
        var movies = await dbContext.Movies
            .AsNoTracking()
            .Where(m => m.DeletedAt == null && m.Status == MovieStatus.UnPublished)
            .ToGetAllMoviesResponse()
            .PaginateAsync(request.PageNumber, request.PageSize, cancellationToken);

        return movies;
    }
}