using CinemaLite.Application.CQRS.Ticket.Command.CreateTicket;
using CinemaLite.Application.CQRS.Ticket.Queries.GetUserTickets;
using CinemaLite.Application.DTOs.Ticket.Response;
using CinemaLite.Domain.Models;

namespace CinemaLite.Application.Interfaces.Mappers;

public interface ITicketMapper
{
    public Ticket ToTicketEntity(CreateTicketCommand request, Movie movie, Session session, int userId);
    
    public CreateTicketResponse ToCreateTicketResponse(Ticket request);
    
    public GetUserTicketsResponse ToGetUserTicketsResponse(List<Ticket> tickets);
}