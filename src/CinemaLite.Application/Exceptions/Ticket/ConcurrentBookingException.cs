using CinemaLite.Application.Exceptions.Abstract;
using Microsoft.AspNetCore.Http;

namespace CinemaLite.Application.Exceptions.Ticket;

public class ConcurrentBookingException : AppException
{
    public ConcurrentBookingException(int seatRow, int seatNumber) : base(
        message: $"Possibility of concurrent booking same seat: SeatNumber {seatNumber}, SeatRow {seatRow}. Try again later.",
        statusCode: StatusCodes.Status409Conflict,
        errorCode: "Concurrent Booking!")
    {
    }
}