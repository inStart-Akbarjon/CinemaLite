using CinemaLite.Application.CQRS.Cart.Commands.AddToCart;
using CinemaLite.Application.DTOs.Ticket.Response;
using CinemaLite.Domain.Models;

namespace CinemaLite.Application.Interfaces.Mappers;

public interface ITicketMapper
{
    public Ticket ToTicketEntity(
        AddToCartCommand request, 
        Movie movie, 
        Session session, 
        Seat seat, 
        int UserId, 
        Guid CartId, 
        string userPhone, 
        string userEmail);
    
    public GetUserTicketsResponse ToGetUserTicketsResponse(List<Ticket> tickets);
}