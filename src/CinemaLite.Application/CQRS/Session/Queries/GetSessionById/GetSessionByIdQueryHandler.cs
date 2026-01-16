using CinemaLite.Application.DTOs.Session.Respone;
using CinemaLite.Application.Exceptions.Movie;
using CinemaLite.Application.Exceptions.Session;
using CinemaLite.Application.Interfaces.DbContext;
using CinemaLite.Application.Interfaces.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CinemaLite.Application.CQRS.Session.Queries.GetSessionById;

public class GetSessionByIdQueryHandler(IAppDbContext dbContext, ISessionMapper sessionMapper) : IRequestHandler<GetSessionByIdQuery, GetSessionByIdResponse>
{
    public async Task<GetSessionByIdResponse> Handle(
        GetSessionByIdQuery request, 
        CancellationToken cancellationToken
    ) {
        var movie = await dbContext.Movies
            .AsNoTracking()
            .Where(m => m.DeletedAt == null)
            .FirstOrDefaultAsync(m => m.Id == request.MovieId, cancellationToken);

        if (movie == null)
        {
            throw new NotFoundMovieException(request.MovieId);
        }
        
        var session = movie.Sessions
            .Where(s => s.DeletedAt == null)
            .FirstOrDefault(s => s.Id == request.Id);

        if (session == null)
        {
            throw new NotFoundSessionException(request.Id);
        }
        
        return sessionMapper.ToGetSessionByIdResponse(session);
    }
}