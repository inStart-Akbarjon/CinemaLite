using CinemaLite.Application.CQRS.Cart.Commands.AddToCart;
using CinemaLite.Application.DTOs.Cart.Response;
using CinemaLite.Domain.Models;

namespace CinemaLite.Application.Interfaces.Mappers;

public interface ISeatReservationMapper
{
    public SeatReservation ToSeatReservationEntity(
        AddToCartCommand request, 
        Movie movie, 
        Session session, 
        Seat seat,
        Guid CartId);

    public AddSeatReservationToCartResponse ToCreateSeatReservationResponse(SeatReservation request);
}