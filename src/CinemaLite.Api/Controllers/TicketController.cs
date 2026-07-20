using CinemaLite.Application.CQRS.Ticket.Commands.RefundTicket;
using CinemaLite.Application.CQRS.Ticket.Queries.GetUserTickets;
using CinemaLite.Application.DTOs.Ticket.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace CinemaLite.Api.Controllers;

[ApiController]
[Route("api/tickets")]
public class TicketController(IMediator mediator) : ControllerBase
{
    [Authorize]
    [HttpGet("my")]
    public async Task<GetUserTicketsResponse> GetUserTickets(CancellationToken cancellationToken) 
    {
        var query = new GetUserTicketsQuery();
        return await mediator.Send(query, cancellationToken);
    }
    
    [Authorize]
    [HttpPost("{ticketId}/refunds")]
    public async Task<RefundTicketResponse> RefundUserTicket([FromRoute] Guid ticketId, CancellationToken cancellationToken) 
    {
        var command = new RefundTicketCommand(ticketId);
        return await mediator.Send(command, cancellationToken);
    }
}