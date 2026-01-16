using CinemaLite.Application.Exceptions.Abstract;
using Microsoft.AspNetCore.Http;

namespace CinemaLite.Application.Exceptions.Ticket;

public class NoAvailableSeatsException : AppException
{
    public NoAvailableSeatsException(Guid id) : base(
        message: $"The movie session with id {id} has no available seats.",
        statusCode: StatusCodes.Status400BadRequest,
        errorCode: "No Available Seats!")
    {
    }
}