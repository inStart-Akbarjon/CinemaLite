using CinemaLite.Application.DTOs.Session.Respone;
using CinemaLite.Application.Exceptions.Movie;
using CinemaLite.Application.Exceptions.Session;
using CinemaLite.Application.Interfaces.DbContext;
using CinemaLite.Application.Interfaces.Mappers;
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
            .Where(m => m.DeletedAt == null)
            .FirstOrDefaultAsync(m => m.Id == request.MovieId, cancellationToken);

        if (movie is null)
        {
            throw new NotFoundMovieException(request.MovieId);
        }
        
        var session = movie.Sessions
            .Where(m => m.DeletedAt == null)
            .FirstOrDefault(s => s.Id == request.Id);

        if (session is null)
        {
            throw new NotFoundSessionException(request.Id);
        }

        session.CinemaName = request.CinemaName;
        session.Price = request.Price;
        session.AvailableSeats = request.AvailableSeats;
        session.StartTime = request.StartTime;
        
        dbContext.Movies.Update(movie);
        await  dbContext.SaveChangesAsync(cancellationToken);
        return sessionMapper.ToUpdateSessionResponse(session);
    }
}