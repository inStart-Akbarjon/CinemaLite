using CinemaLite.Application.CQRS.Movie.Events;
using CinemaLite.Application.DTOs.Movie.Response;
using CinemaLite.Application.Exceptions.Movie;
using CinemaLite.Application.Interfaces.DbContext;
using CinemaLite.Application.Interfaces.Mappers;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CinemaLite.Application.CQRS.Movie.Commands.CreateMovie;

public class CreateMovieCommandHandler(
    IAppDbContext dbContext, 
    IMovieMapper movieMapper, 
    IPublishEndpoint publishEndpoint) : IRequestHandler<CreateMovieCommand, CreateMovieResponse>
{
    public async Task<CreateMovieResponse> Handle(CreateMovieCommand request, CancellationToken cancellationToken)
    {
        var duplicateMovie = await dbContext.Movies
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Title == request.Title && m.DeletedAt == null, cancellationToken);

        if (duplicateMovie != null)
        {
            throw new DuplicateMovieException(request.Title);
        }
        
        var movie = movieMapper.ToMovieEntityFromCreateMovieCommand(request);
        
        await dbContext.Movies.AddAsync(movie, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        await publishEndpoint.Publish(new MovieCacheInvalidationEvent()
        {
            Id = movie.Id,
        }, cancellationToken);
        
        return movieMapper.ToCreateMovieResponse(movie);
    }
}