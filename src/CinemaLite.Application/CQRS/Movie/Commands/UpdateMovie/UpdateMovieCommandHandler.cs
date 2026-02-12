using CinemaLite.Application.DTOs.Movie.Response;
using CinemaLite.Application.Exceptions.Movie;
using CinemaLite.Application.Extensions.RedisCache;
using CinemaLite.Application.Interfaces.DbContext;
using CinemaLite.Application.Interfaces.Mappers;
using CinemaLite.Application.Models.Cache;
using CinemaLite.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace CinemaLite.Application.CQRS.Movie.Commands.UpdateMovie;

public class UpdateMovieCommandHandler(
    IAppDbContext dbContext, 
    IMovieMapper movieMapper,
    IConnectionMultiplexer redis) : IRequestHandler<UpdateMovieCommand, UpdateMovieResponse>
{
    public async Task<UpdateMovieResponse> Handle(UpdateMovieCommand request, CancellationToken cancellationToken)
    {
        var duplicateMovie = await dbContext.Movies
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Title == request.Title && m.DeletedAt == null && m.Status == MovieStatus.Published, cancellationToken);
    
        if (duplicateMovie != null)
        {
            throw new DuplicateMovieException(request.Title);
        }
        
        var movie = await dbContext.Movies
            .FirstOrDefaultAsync(m => m.Id == request.Id && m.DeletedAt == null, cancellationToken);
        
        if (movie is null)
        {
            throw new NotFoundMovieException(request.Id);
        }
        
        movie.Title = request.Title;
        movie.DurationMinutes = request.DurationMinutes;
        movie.Genre = request.Genre;
        movie.IsTop = request.IsTop;
        movie.TopSubscriptionPeriod = request.TopSubscriptionPeriod;
        
        dbContext.Movies.Update(movie);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        await redis.InvalidateAsync(MoviesCacheKeys.Registry);
        
        if (movie.IsTop)
        {
            await redis.InvalidateAsync(TopMoviesCacheKeys.Registry);
        }
        
        return movieMapper.ToUpdateMovieResponse(movie);
    }
}