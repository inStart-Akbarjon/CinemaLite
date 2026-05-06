using CinemaLite.Application.Interfaces.DbContext;
using CinemaLite.Application.Services.Interfaces.Common;
using CinemaLite.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace CinemaLite.Application.Services.Implementations.Common;

public class OpenSeatsStatus(IAppDbContext dbContext) : IOpenSeatsStatus
{
    public async Task OpenSeatsStatusFromCart(Guid cartId, CancellationToken cancellationToken)
    {
        var seatReservations = await dbContext.SeatReservations
            .Where(s => s.CartId == cartId)
            .ToListAsync(cancellationToken);
        
        var movieIds = seatReservations.Select(x => x.MovieId).Distinct().ToList();
            
        var movies = await dbContext.Movies
            .Where(m => movieIds.Contains(m.Id) && m.DeletedAt == null)
            .ToListAsync(cancellationToken);
            
        foreach (var seatReservation in seatReservations)
        {
            
            var movie = movies.FirstOrDefault(m => m.Id == seatReservation.MovieId && m.DeletedAt == null);
            
            var session = movie?.Sessions
                .FirstOrDefault(s => s.Id == seatReservation.SessionId && s.DeletedAt == null);
            
            var seat = session?.Seats.FirstOrDefault(s =>
                s.Id == seatReservation.SeatId);

            if (seat != null)
            {
                seat.Status = SeatStatus.Open;
            }

            if (movie != null)
            {
                dbContext.Movies.Update(movie);
            }
        }
        
        dbContext.SeatReservations.RemoveRange(seatReservations);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task OpenSeatsStatusFromOrder(Guid orderId, CancellationToken cancellationToken)
    {
        var seatReservations = await dbContext.SeatReservations
            .Where(s => s.OrderId == orderId)
            .ToListAsync(cancellationToken);
            
        var movieIds = seatReservations.Select(x => x.MovieId).Distinct().ToList();
        
        var movies = await dbContext.Movies
            .Where(m => movieIds.Contains(m.Id) && m.DeletedAt == null)
            .ToListAsync(cancellationToken);
        
        foreach (var seatReservation in seatReservations)
        {
            var movie = movies.FirstOrDefault(m => m.Id == seatReservation.MovieId && m.DeletedAt == null);
            
            var session = movie?.Sessions
                .FirstOrDefault(s => s.Id == seatReservation.SessionId && s.DeletedAt == null);
            
            var seat = session?.Seats.FirstOrDefault(s =>
                s.Id == seatReservation.SeatId);
            
            if (seat != null)
            {
                seat.Status = SeatStatus.Open;
            }

            if (movie != null)
            {
                dbContext.Movies.Update(movie);
            }
        }
        
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}