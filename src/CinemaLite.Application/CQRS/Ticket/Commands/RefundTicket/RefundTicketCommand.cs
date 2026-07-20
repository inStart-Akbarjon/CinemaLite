using CinemaLite.Application.DTOs.Ticket.Response;
using MediatR;

namespace CinemaLite.Application.CQRS.Ticket.Commands.RefundTicket;

public record RefundTicketCommand(Guid TicketId) : IRequest<RefundTicketResponse>
{
}