using CinemaLite.Application.DTOs.Session.Respone;
using CinemaLite.Application.Exceptions.Movie;
using CinemaLite.Application.Exceptions.Session;
using CinemaLite.Application.Extensions.SessionSeats;
using CinemaLite.Application.Interfaces.DbContext;
using CinemaLite.Application.Interfaces.Mappers;
using CinemaLite.Domain.Enums;
using CinemaLite.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CinemaLite.Application.CQRS.Session.Commands.UpdateSession;

public class UpdateSessionCommandHandler(
    IAppDbContext dbContext, 
    ISessionMapper sessionMapper
) : IRequestHandler<UpdateSessionCommand, UpdateSessionResponse>
{
    public async Task<UpdateSessionResponse> Handle(
        UpdateSessionCommand request, 
        CancellationToken cancellationToken
    ) {
        var movie = await dbContext.Movies
            .FirstOrDefaultAsync(m => m.Id == request.MovieId && m.DeletedAt == null && m.Status == MovieStatus.Published, cancellationToken);

        if (movie is null)
        {
            throw new NotFoundMovieException(request.MovieId);
        }
        
        var session = movie.Sessions
            .FirstOrDefault(s => s.Id == request.Id && s.DeletedAt == null);

        if (session is null)
        {
            throw new NotFoundSessionException(request.Id);
        }
        
        var seats = new List<Seat>();
        
        seats.GenerateSeats(request.TotalRows, request.SeatsPerRow);
        
        session.StartTime = request.StartTime;
        session.CinemaName = request.CinemaName;
        session.Price = request.Price;
        session.AvailableSeats = request.SeatsPerRow * request.TotalRows;
        session.TotalRows = request.TotalRows;
        session.SeatsPerRow = request.SeatsPerRow;
        session.Seats = seats;
        
        dbContext.Movies.Update(movie);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return sessionMapper.ToUpdateSessionResponse(session);
    }
}