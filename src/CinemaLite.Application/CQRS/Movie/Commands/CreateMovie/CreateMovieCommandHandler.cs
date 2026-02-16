using CinemaLite.Application.DTOs.Movie.Response;
using CinemaLite.Application.Exceptions.Movie;
using CinemaLite.Application.Extensions.RedisCache;
using CinemaLite.Application.Interfaces.DbContext;
using CinemaLite.Application.Interfaces.Mappers;
using CinemaLite.Application.Models.Cache;
using Microsoft.EntityFrameworkCore;
using MediatR;
using StackExchange.Redis;

namespace CinemaLite.Application.CQRS.Movie.Commands.CreateMovie;

public class CreateMovieCommandHandler(
    IAppDbContext dbContext, 
    IMovieMapper movieMapper,
    IConnectionMultiplexer redis) : IRequestHandler<CreateMovieCommand, CreateMovieResponse>
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
        
        if (movie.IsTop)
        {
            movie.TopSubscriptionStartDate = DateTime.UtcNow;
        }
        
        await dbContext.Movies.AddAsync(movie, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        await redis.InvalidateAsync(MoviesCacheKeys.Registry);

        if (movie.IsTop)
        {
            await redis.InvalidateAsync(TopMoviesCacheKeys.Registry);
        }
        
        return movieMapper.ToCreateMovieResponse(movie);
    }
}