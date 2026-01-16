using CinemaLite.Application.DTOs.Movie.Response;
using CinemaLite.Application.DTOs.Pagination;
using CinemaLite.Application.Extensions.Pagination;
using CinemaLite.Application.Interfaces.DbContext;
using CinemaLite.Application.Interfaces.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CinemaLite.Application.CQRS.Movie.Queries.GetAllMovies;

public class GetAllMoviesQueryHandler(IAppDbContext dbContext, IMovieMapper movieMapper) : IRequestHandler<GetAllMoviesQuery, PaginatedMovieList<GetAllMoviesResponse>>
{
    public async Task<PaginatedMovieList<GetAllMoviesResponse>> Handle(GetAllMoviesQuery request, CancellationToken cancellationToken)
    {
        var movies = await dbContext.Movies
            .AsNoTracking()
            .Where(m => m.DeletedAt == null)
            .ToGetAllMoviesResponse()
            .PaginateAsync(request.pageNumber, request.pageSize, cancellationToken);

        return movies;
    }
}