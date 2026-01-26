using CinemaLite.Domain.Models;

namespace CinemaLite.Application.Extensions.SessionSeats;

public static class GenerateSessionSeats
{
    public static IList<Seat> GenerateSeats(this IList<Seat> seats, int totalRows, int seatsPerRow)
    {
        for (int row = 1; row <= totalRows; row++)
        {
            for (int seatNum = 1; seatNum <= seatsPerRow; seatNum++)
            {
                seats.Add(new Seat
                {
                    SeatRow = row,
                    SeatNumber = seatNum,
                    IsBooked = false
                });
            }
        }
        
        return seats;
    }
}