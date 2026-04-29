using CinemaLite.Application.Exceptions.Abstract;
using Microsoft.AspNetCore.Http;

namespace CinemaLite.Application.Exceptions.SeatReservation;

public class NotFoundSeatReservationException : AppException
{
    public NotFoundSeatReservationException(Guid? CartId) : base(
        message: $"The SeatReservation with this cart Id '{CartId}' not found.",
        statusCode: StatusCodes.Status404NotFound,
        errorCode: "Ticket Not Found!")
    {
    }
}