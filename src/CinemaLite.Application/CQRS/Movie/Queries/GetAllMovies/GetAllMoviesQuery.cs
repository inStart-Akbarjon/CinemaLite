using CinemaLite.Application.DTOs.Movie.Response;
using CinemaLite.Application.DTOs.Pagination;
using MediatR;

namespace CinemaLite.Application.CQRS.Movie.Queries.GetAllMovies;

public record GetAllMoviesQuery(int pageNumber, int pageSize) : IRequest<PaginatedMovieList<GetAllMoviesResponse>>
{
}