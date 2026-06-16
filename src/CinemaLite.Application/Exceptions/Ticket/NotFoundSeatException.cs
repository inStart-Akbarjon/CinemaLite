using CinemaLite.Application.Exceptions.Abstract;
using Microsoft.AspNetCore.Http;

namespace CinemaLite.Application.Exceptions.Ticket;

public class NotFoundSeatException : AppException
{
    public NotFoundSeatException(int seatRow, int seatNumber) : base(
        message: $"The seat in {seatNumber} row {seatRow} not Found.",
        statusCode: StatusCodes.Status404NotFound,
        errorCode: "Seat Not Found!")
    {
    }
    public NotFoundSeatException(Guid seatId) : base(
        message: $"The seat with seat ID: {seatId} not Found.",
        statusCode: StatusCodes.Status404NotFound,
        errorCode: "Seat Not Found!")
    {
    }
}