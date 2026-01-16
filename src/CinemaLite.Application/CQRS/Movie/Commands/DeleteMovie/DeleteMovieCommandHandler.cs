using CinemaLite.Application.Exceptions.Movie;
using CinemaLite.Application.Interfaces.DbContext;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaLite.Application.CQRS.Movie.Commands.DeleteMovie;

public class DeleteMovieCommandHandler(IAppDbContext dbContext) : IRequestHandler<DeleteMovieCommand, IActionResult>
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

        return new OkResult();
    }
}