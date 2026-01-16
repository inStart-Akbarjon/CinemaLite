using CinemaLite.Application.DTOs.Ticket.Response;
using MediatR;

namespace CinemaLite.Application.CQRS.Ticket.Command.CreateTicket;

public record CreateTicketCommand(
    Guid MovieId,
    Guid SessionId
) : IRequest<CreateTicketResponse>
{
}