using CinemaLite.Application.DTOs.Session.Respone;
using CinemaLite.Application.Exceptions.Movie;
using CinemaLite.Application.Interfaces.DbContext;
using CinemaLite.Application.Interfaces.Mappers;
using CinemaLite.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CinemaLite.Application.CQRS.Session.Commands.CreateSession;

public class CreateSessionCommandHandler(IAppDbContext dbContext, ISessionMapper sessionMapper) : IRequestHandler<CreateSessionCommand, CreateSessionResponse>
{
    public async Task<CreateSessionResponse> Handle(CreateSessionCommand request, CancellationToken cancellationToken)
    {
        var movie = await dbContext.Movies
            .Where(m => m.DeletedAt == null)
            .FirstOrDefaultAsync(m => m.Id == request.MovieId, cancellationToken);
        
        if (movie == null)
        {
            throw new NotFoundMovieException(request.MovieId);
        }
        
        var sessions = movie.Sessions?.ToList() ?? [];

        if (movie.MinPrice == 0 || request.Price < movie.MinPrice)
        {
            movie.MinPrice = request.Price;
        }

        var session = new Domain.Models.Session()
        {
            Id = Guid.NewGuid(),
            StartTime = request.StartTime,
            CinemaName = request.CinemaName,
            Price = request.Price,
            AvailableSeats = request.AvailableSeats,
            MovieId = movie.Id
        };

        sessions.Add(session);
        
        movie.Sessions = sessions;
        
        if (movie.Status != MovieStatus.Published)
        {
            movie.Status = MovieStatus.Published;
        }
        
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return sessionMapper.ToCreateSessionResponse(session);
    }
}