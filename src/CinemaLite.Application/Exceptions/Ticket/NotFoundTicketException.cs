using CinemaLite.Application.Exceptions.Abstract;
using Microsoft.AspNetCore.Http;

namespace CinemaLite.Application.Exceptions.Ticket;

public class NotFoundTicketException : AppException
{
    public NotFoundTicketException(int id) : base(
        message: $"The user tickets with this user Id '{id}' not found.",
        statusCode: StatusCodes.Status404NotFound,
        errorCode: "Tickets Not Found!")
    {
    }
}