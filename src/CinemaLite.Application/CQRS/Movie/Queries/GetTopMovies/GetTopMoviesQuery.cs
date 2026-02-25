using CinemaLite.Application.DTOs.Movie.Response;
using CinemaLite.Application.DTOs.Pagination;
using MediatR;

namespace CinemaLite.Application.CQRS.Movie.Queries.GetTopMovies;

public record GetTopMoviesQuery(int PageNumber = 1, int PageSize = 10)
    : IRequest<PaginatedMovieList<GetTopMoviesResponse>>
{
};