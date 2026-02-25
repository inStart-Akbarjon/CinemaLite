using CinemaLite.Application.DTOs.Session.Respone;
using CinemaLite.Application.Exceptions.Movie;
using CinemaLite.Application.Extensions.RedisCache;
using CinemaLite.Application.Extensions.SessionSeats;
using CinemaLite.Application.Interfaces.DbContext;
using CinemaLite.Application.Interfaces.Mappers;
using CinemaLite.Application.Models.Cache;
using CinemaLite.Domain.Enums;
using CinemaLite.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace CinemaLite.Application.CQRS.Session.Commands.CreateSession;

public class CreateSessionCommandHandler(
    IAppDbContext dbContext, 
    ISessionMapper sessionMapper,
    IConnectionMultiplexer redis) : IRequestHandler<CreateSessionCommand, CreateSessionResponse>
{
    public async Task<CreateSessionResponse> Handle(CreateSessionCommand request, CancellationToken cancellationToken)
    {
        var movie = await dbContext.Movies
            .FirstOrDefaultAsync(m => m.Id == request.MovieId && m.DeletedAt == null, cancellationToken);
        
        if (movie == null)
        {
            throw new NotFoundMovieException(request.MovieId);
        }
        
        var sessions = movie.Sessions?.ToList() ?? [];

        if (movie.MinPrice == 0 || request.Price < movie.MinPrice)
        {
            movie.MinPrice = request.Price;
        }
        
        var seats = new List<Seat>();
        
        seats.GenerateSeats(request.TotalRows, request.SeatsPerRow);
        
        var session = new Domain.Models.Session()
        {
            StartTime = request.StartTime,
            CinemaName = request.CinemaName,
            Price = request.Price,
            AvailableSeats = request.SeatsPerRow * request.TotalRows,
            TotalRows = request.TotalRows,
            SeatsPerRow = request.SeatsPerRow,
            Seats = seats,
        };
        
        sessions.Add(session);
        
        movie.Sessions = sessions;
        
        if (movie.Status != MovieStatus.Published)
        {
            movie.Status = MovieStatus.Published;
        }
        
        await dbContext.SaveChangesAsync(cancellationToken);
        
        await redis.InvalidateAsync(MoviesCacheKeys.Registry);
        await redis.InvalidateAsync(TopMoviesCacheKeys.Registry);
        
        return sessionMapper.ToCreateSessionResponse(session);
    }
}