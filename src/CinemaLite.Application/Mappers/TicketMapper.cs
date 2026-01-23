using CinemaLite.Application.CQRS.Ticket.Command.CreateTicket;
using CinemaLite.Application.DTOs.Ticket.Response;
using CinemaLite.Application.Interfaces.Mappers;
using CinemaLite.Domain.Models;

namespace CinemaLite.Application.Mappers;

public class TicketMapper : ITicketMapper
{
    public Ticket ToTicketEntity(CreateTicketCommand request, Movie movie, Session session, Seat seat, int userId)
    {
        return new Ticket()
        {
            UserId = userId,
            MovieId = request.MovieId,
            SessionId = request.SessionId,
            MovieTitle = movie.Title,
            CinemaName = session.CinemaName,
            SeatRow = seat.SeatRow,
            SeatNumber = seat.SeatNumber,
            StartTime = session.StartTime,
            PricePaid = session.Price,
        };
    }

    public CreateTicketResponse ToCreateTicketResponse(Ticket request)
    {
        return new CreateTicketResponse()
        {
            TicketId = request.Id,
            MovieTitle = request.MovieTitle,
            CinemaName = request.CinemaName,
            SeatRow = request.SeatRow,
            SeatNumber = request.SeatNumber,
            StartTime = request.StartTime,
            PricePaid = request.PricePaid
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