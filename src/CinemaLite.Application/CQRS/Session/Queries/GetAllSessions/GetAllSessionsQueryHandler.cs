using CinemaLite.Application.DTOs.Session.Respone;
using CinemaLite.Application.Interfaces.DbContext;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CinemaLite.Application.CQRS.Session.Queries.GetAllSessions;

public class GetAllSessionsQueryHandler(IAppDbContext dbContext) : IRequestHandler<GetAllSessionsQuery, List<GetAllSessionsFromMovieResponse>>
{
    public async Task<List<GetAllSessionsFromMovieResponse>> Handle(
        GetAllSessionsQuery request, 
        CancellationToken cancellationToken
    ) {
        var movie = await dbContext.Movies
            .AsNoTracking()
            .Where(m => m.DeletedAt == null && m.Id == request.MovieId)
            .ToListAsync(cancellationToken);
        
        var movieSessions = movie
            .Select(m => new GetAllSessionsFromMovieResponse()
            {
                MovieId = m.Id,
                Title = m.Title,
                DurationMinutes = m.DurationMinutes,
                MinPrice = m.MinPrice,
                Genre = m.Genre,
                Status = m.Status,
                Sessions = m.Sessions
                    .Where(s => s.DeletedAt == null)
                    .Select(s => new GetAllSessionsResponse()
                {
                    SessionId = s.Id,
                    CinemaName = s.CinemaName,
                    AvailableSeats = s.AvailableSeats,
                    Price = s.Price,
                    StartTime = s.StartTime
                }).ToList()
            }).ToList();

        return movieSessions;
    }
}