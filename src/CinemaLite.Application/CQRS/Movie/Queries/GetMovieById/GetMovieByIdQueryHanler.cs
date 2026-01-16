using CinemaLite.Application.DTOs.Movie.Response;
using CinemaLite.Application.Exceptions.Movie;
using CinemaLite.Application.Interfaces.DbContext;
using CinemaLite.Application.Interfaces.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CinemaLite.Application.CQRS.Movie.Queries.GetMovieById;

public class GetMovieByIdQueryHanler(IAppDbContext dbContext, IMovieMapper movieMapper) : IRequestHandler<GetMovieByIdQuery, GetMovieByIdResponse>
{
    public async Task<GetMovieByIdResponse> Handle(GetMovieByIdQuery request, CancellationToken cancellationToken)
    {
        var movie = await dbContext.Movies
            .AsNoTracking()
            .Where(m => m.DeletedAt == null)
            .FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken);

        if (movie is null)
        {
            throw new NotFoundMovieException(request.Id);
        }

        return movieMapper.ToGetMovieByIdResponse(movie);
    }
}