using CinemaLite.Application.DTOs.Movie.Response;
using CinemaLite.Application.DTOs.Pagination;
using MediatR;

namespace CinemaLite.Application.CQRS.Movie.Queries.GetUnpublishedMovies;

public record GetUnpublishedMoviesQuery(int PageNumber = 1, int PageSize = 10) : IRequest<PaginatedMovieList<GetAllMoviesResponse>>
{
}