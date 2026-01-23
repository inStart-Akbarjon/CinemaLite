using CinemaLite.Application.DTOs.Movie.Response;
using CinemaLite.Application.DTOs.Pagination;
using CinemaLite.Application.Extensions.Pagination;
using CinemaLite.Application.Interfaces.DbContext;
using CinemaLite.Application.Interfaces.Mappers;
using CinemaLite.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CinemaLite.Application.CQRS.Movie.Queries.SearchMovies;

public class SearchMoviesQueryHandler(IAppDbContext dbContext) : IRequestHandler<SearchMoviesQuery, PaginatedMovieList<GetAllMoviesResponse>>
{
    public async Task<PaginatedMovieList<GetAllMoviesResponse>> Handle(SearchMoviesQuery request, CancellationToken cancellationToken)
    {
        var lowerCaseTerm = request.SearchTerm.ToLower().Trim();
        
        var movies = await dbContext.Movies
            .AsNoTracking()
            .Where(m => m.DeletedAt == null && m.Status == MovieStatus.Published && m.Title.ToLower().Contains(lowerCaseTerm))
            .ToGetAllMoviesResponse()
            .PaginateAsync(request.PageNumber, request.PageSize, cancellationToken);

        return movies;
    }
}