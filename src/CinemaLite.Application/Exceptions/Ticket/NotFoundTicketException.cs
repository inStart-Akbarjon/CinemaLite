using CinemaLite.Application.Exceptions.Abstract;
using Microsoft.AspNetCore.Http;

namespace CinemaLite.Application.Exceptions.Ticket;

public class NotFoundTicketException : AppException
{
    public NotFoundTicketException(int userId) : base(
        message: $"The user tickets with this user Id '{userId}' not found.",
        statusCode: StatusCodes.Status404NotFound,
        errorCode: "Tickets Not Found!")
    {
    }
    public NotFoundTicketException(Guid? CartId) : base(
        message: $"The ticket with this cart Id '{CartId}' not found.",
        statusCode: StatusCodes.Status404NotFound,
        errorCode: "Ticket Not Found!")
    {
    }
}