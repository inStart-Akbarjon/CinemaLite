using CinemaLite.Application.DTOs.Movie.Response;
using CinemaLite.Application.DTOs.Pagination;
using MediatR;

namespace CinemaLite.Application.CQRS.Movie.Queries.SearchMovies;

public record SearchMoviesQuery(
    string SearchTerm,
    int PageNumber, 
    int PageSize
) : IRequest<PaginatedMovieList<GetAllMoviesResponse>>
{
}