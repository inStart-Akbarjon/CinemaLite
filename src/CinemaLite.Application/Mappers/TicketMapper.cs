using CinemaLite.Application.CQRS.Cart.Commands.AddToCart;
using CinemaLite.Application.DTOs.Ticket.Response;
using CinemaLite.Application.Interfaces.Mappers;
using CinemaLite.Domain.Models;

namespace CinemaLite.Application.Mappers;

public class TicketMapper : ITicketMapper
{
    public Ticket ToTicketEntity(
        AddToCartCommand request, 
        Movie movie,
        Session session,
        Seat seat,
        int userId,
        Guid CartId,
        string userPhone,
        string userEmail)
    {
        return new Ticket()
        {
            UserId = userId,
            MovieId = request.MovieId,
            UserPhone = userPhone,
            UserEmail = userEmail,
            SessionId = request.SessionId,
            MovieTitle = movie.Title,
            CinemaName = session.CinemaName,
            SeatRow = seat.SeatRow,
            SeatNumber = seat.SeatNumber,
            StartTime = session.StartTime,
            PricePaid = session.Price,
        };
    }

    public GetUserTicketsResponse ToGetUserTicketsResponse(List<Ticket> tickets)
    {
        return new GetUserTicketsResponse()
        {
            Tickets = tickets.Select(t => new GetUserTickets()
            {
                TicketId = t.Id,
                MovieTitle = t.MovieTitle,
                CinemaName = t.CinemaName,
                SeatRow = t.SeatRow,
                SeatNumber = t.SeatNumber,
                PricePaid = t.PricePaid,
                StartTime = t.StartTime
            }).ToList()
        };
    }
}