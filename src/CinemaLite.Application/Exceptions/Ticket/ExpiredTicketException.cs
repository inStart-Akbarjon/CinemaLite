using CinemaLite.Application.Exceptions.Abstract;
using Microsoft.AspNetCore.Http;

namespace CinemaLite.Application.Exceptions.Ticket;

public class ExpiredTicketException: AppException
{
    public ExpiredTicketException(Guid ticketId) : base(
        message: $"The ticket with this Id {ticketId} is expired.",
        statusCode: StatusCodes.Status400BadRequest,
        errorCode: "Ticket is expired!")
    {
    }
}