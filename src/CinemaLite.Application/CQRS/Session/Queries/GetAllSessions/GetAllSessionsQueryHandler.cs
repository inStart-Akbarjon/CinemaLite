using CinemaLite.Application.DTOs.Session.Respone;
using CinemaLite.Application.Exceptions.Movie;
using CinemaLite.Application.Interfaces.DbContext;
using CinemaLite.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CinemaLite.Application.CQRS.Session.Queries.GetAllSessions;

public class GetAllSessionsQueryHandler(IAppDbContext dbContext) : IRequestHandler<GetAllSessionsQuery, GetAllSessionsFromMovieResponse>
{
    public async Task<GetAllSessionsFromMovieResponse> Handle(
        GetAllSessionsQuery request, 
        CancellationToken cancellationToken
    ) {
        var movie = await dbContext.Movies
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.DeletedAt == null && m.Id == request.MovieId && m.Status == MovieStatus.Published, cancellationToken);

        if (movie == null)
        {
            throw new NotFoundMovieException(request.MovieId);
        }

        var movieSessions = new GetAllSessionsFromMovieResponse()
        {
            MovieId = movie.Id,
            Title = movie.Title,
            DurationMinutes = movie.DurationMinutes,
            MinPrice = movie.MinPrice,
            Genre = movie.Genre,
            Status = movie.Status,
            Sessions = movie.Sessions
                .Where(s => s.DeletedAt == null)
                .Select(s => new GetAllSessionsResponse()
                {
                    Id = s.Id,
                    CinemaName = s.CinemaName,
                    AvailableSeats = s.AvailableSeats,
                    Price = s.Price,
                    StartTime = s.StartTime
                }).ToList()
        };

        return movieSessions;
    }
}