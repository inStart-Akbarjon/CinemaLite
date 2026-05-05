using CinemaLite.Application.Exceptions.Abstract;
using Microsoft.AspNetCore.Http;

namespace CinemaLite.Application.Exceptions.Ticket;

public class BookedSeatException : AppException
{
    public BookedSeatException(int seatRow, int seatNumber) : base(
        message: $"The seat with SeatNumber {seatNumber} and SeatRow {seatRow} already booked by other person.",
        statusCode: StatusCodes.Status400BadRequest,
        errorCode: "Booked Seat!")
    {
    }
}