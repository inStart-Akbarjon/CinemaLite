using CinemaLite.Application.DTOs.Session.Respone;
using CinemaLite.Application.Exceptions.Movie;
using CinemaLite.Application.Interfaces.DbContext;
using CinemaLite.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CinemaLite.Application.CQRS.Session.Queries.GetAvailableSeats;

public class GetAvailableSeatsQueryHandler(IAppDbContext dbContext) : IRequestHandler<GetAvailableSeatsQuery, GetAvailableSeatsResponse>
{
    public async Task<GetAvailableSeatsResponse> Handle(GetAvailableSeatsQuery request, CancellationToken cancellationToken)
    {
        var movie = await dbContext.Movies
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.DeletedAt == null && m.Id == request.MovieId && m.Status == MovieStatus.Published, cancellationToken);

        if (movie == null)
        {
            throw new NotFoundMovieException(request.MovieId);
        }

        var sessionAvailableSeats = new GetAvailableSeatsResponse()
        {
            MovieId = movie.Id,
            Title = movie.Title,
            DurationMinutes = movie.DurationMinutes,
            MinPrice = movie.MinPrice,
            Genre = movie.Genre,
            Status = movie.Status,
            Sessions = movie.Sessions
                .Where(s => s.DeletedAt == null && s.Id == request.Id)
                .Select(s => new GetSessionWithAvailableSeatsResponse()
                {
                    Id = s.Id,
                    CinemaName = s.CinemaName,
                    AvailableSeats = s.AvailableSeats,
                    TotalRows = s.TotalRows,
                    SeatsPerRow = s.SeatsPerRow,
                    Price = s.Price,
                    Seats = s.Seats.Where(s => !s.IsBooked).ToList(),
                    StartTime = s.StartTime
                }).ToList()
        };

        return sessionAvailableSeats;
    }
}