using CinemaLite.Application.CQRS.Cart.Commands.AddToCart;
using CinemaLite.Application.DTOs.Cart.Response;
using CinemaLite.Application.Interfaces.Mappers;
using CinemaLite.Domain.Models;

namespace CinemaLite.Application.Mappers;

public class SeatReservationMapper : ISeatReservationMapper
{
    public SeatReservation ToSeatReservationEntity(
        AddToCartCommand request, 
        Movie movie, 
        Session session, 
        Seat seat, 
        Guid CartId)
    {
        return new SeatReservation()
        {
            CartId = CartId,
            MovieId = request.MovieId,
            SessionId = request.SessionId,
            MovieTitle = movie.Title,
            CinemaName = session.CinemaName,
            StartTime = session.StartTime,
            PricePaid = session.Price,
            SeatRow = seat.SeatRow,
            SeatNumber = seat.SeatNumber
        };
    }

    public AddSeatReservationToCartResponse ToCreateSeatReservationResponse(SeatReservation request)
    {
        return new AddSeatReservationToCartResponse()
        {
            SeatReservationId = request.Id,
            MovieTitle = request.MovieTitle,
            SeatRow = request.SeatRow,
            SeatNumber = request.SeatNumber
        };
    }
}