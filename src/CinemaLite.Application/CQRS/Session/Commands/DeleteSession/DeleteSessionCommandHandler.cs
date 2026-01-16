using CinemaLite.Application.Exceptions.Movie;
using CinemaLite.Application.Exceptions.Session;
using CinemaLite.Application.Interfaces.DbContext;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaLite.Application.CQRS.Session.Commands.DeleteSession;

public class DeleteSessionCommandHandler(IAppDbContext dbContext) : IRequestHandler<DeleteSessionCommand, IActionResult>
{
    public async Task<IActionResult> Handle(
        DeleteSessionCommand request, 
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
        
        session.SoftDelete();

        dbContext.Movies.Update(movie);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return new OkResult();
    }
}