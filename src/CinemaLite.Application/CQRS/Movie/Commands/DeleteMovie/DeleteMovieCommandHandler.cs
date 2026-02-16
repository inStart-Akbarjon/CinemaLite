using CinemaLite.Application.Exceptions.Movie;
using CinemaLite.Application.Extensions.RedisCache;
using CinemaLite.Application.Interfaces.DbContext;
using CinemaLite.Application.Models.Cache;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace CinemaLite.Application.CQRS.Movie.Commands.DeleteMovie;

public class DeleteMovieCommandHandler(
    IAppDbContext dbContext,
    IConnectionMultiplexer redis
    ) : IRequestHandler<DeleteMovieCommand, IActionResult>
{
    public async Task<IActionResult> Handle(DeleteMovieCommand request, CancellationToken cancellationToken)
    {
        var movie = await dbContext.Movies
            .FirstOrDefaultAsync(m => m.Id == request.Id && m.DeletedAt == null, cancellationToken);

        if (movie is null)
        {
            throw new NotFoundMovieException(request.Id);
        }
        
        movie.SoftDelete();
        
        await  dbContext.SaveChangesAsync(cancellationToken);
        
        await redis.InvalidateAsync(MoviesCacheKeys.Registry);
        
        if (movie.IsTop)
        {
            await redis.InvalidateAsync(TopMoviesCacheKeys.Registry);
        }
        
        return new OkResult();
    }
}