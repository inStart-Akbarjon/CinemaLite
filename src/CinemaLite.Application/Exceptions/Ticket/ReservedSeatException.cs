using CinemaLite.Application.Exceptions.Abstract;
using Microsoft.AspNetCore.Http;

namespace CinemaLite.Application.Exceptions.Ticket;

public class ReservedSeatException : AppException
{
    public ReservedSeatException(int seatRow, int seatNumber) : base(
        message: $"The seat with SeatNumber {seatNumber} and SeatRow {seatRow} already reserved by other person.",
        statusCode: StatusCodes.Status400BadRequest,
        errorCode: "Reserved Seat!")
    {
    }
}