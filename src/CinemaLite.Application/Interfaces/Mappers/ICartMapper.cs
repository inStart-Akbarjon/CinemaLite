using CinemaLite.Application.DTOs.Cart.Response;
using CinemaLite.Domain.Models;

namespace CinemaLite.Application.Interfaces.Mappers;

public interface ICartMapper
{
    public GetItemsFromCartResponse ToGetItemsFromCartResponse(List<SeatReservation> seatReservations);
    public GetUserCartResponse ToGetUserCartResponse(Cart cart);
}