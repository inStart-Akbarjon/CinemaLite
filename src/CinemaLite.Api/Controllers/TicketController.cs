using CinemaLite.Application.CQRS.Ticket.Command.CreateTicket;
using CinemaLite.Application.CQRS.Ticket.Queries.GetUserTickets;
using CinemaLite.Application.DTOs.Ticket.Response;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CinemaLite.Api.Controllers;

[ApiController]
[Route("api/ticket")]
public class TicketController(IMediator mediator) : ControllerBase
{
    [HttpGet("my")]
    [Authorize]
    public async Task<GetUserTicketsResponse> GetUserTickets(CancellationToken cancellationToken) 
    {
        var query = new GetUserTicketsQuery();
        return await mediator.Send(query, cancellationToken);
    }

    [HttpPost]
    [Authorize]
    public async Task<CreateTicketResponse> CreateTicket([FromBody] CreateTicketCommand request, CancellationToken cancellationToken) 
    {
        var command = new CreateTicketCommand(request.MovieId, request.SessionId);
        return await mediator.Send(command, cancellationToken);
    }
}