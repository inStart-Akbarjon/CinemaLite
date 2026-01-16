using CinemaLite.Application.DTOs.Movie.Response;
using CinemaLite.Application.Exceptions.Movie;
using CinemaLite.Application.Interfaces.DbContext;
using CinemaLite.Application.Interfaces.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CinemaLite.Application.CQRS.Movie.Commands.UpdateMovie;

public class UpdateMovieCommandHandler(IAppDbContext dbContext, IMovieMapper movieMapper) : IRequestHandler<UpdateMovieCommand, UpdateMovieResponse>
{
    public async Task<UpdateMovieResponse> Handle(UpdateMovieCommand request, CancellationToken cancellationToken)
    {
        var movie = await dbContext.Movies
            .FirstOrDefaultAsync(m => m.Id == request.Id && m.DeletedAt == null, cancellationToken);

        if (movie is null)
        {
            throw new NotFoundMovieException(request.Id);
        }
        
        movie.Title = request.Title;
        movie.DurationMinutes = request.DurationMinutes;
        movie.Genre = request.Genre;
        
        dbContext.Movies.Update(movie);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return movieMapper.ToUpdateMovieResponse(movie);
    }
}