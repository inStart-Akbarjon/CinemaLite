using CinemaLite.Application.DTOs.Cart.Response;
using CinemaLite.Application.Interfaces.Mappers;
using CinemaLite.Domain.Models;

namespace CinemaLite.Application.Mappers;

public class CartMapper : ICartMapper
{
    public GetItemsFromCartResponse ToGetItemsFromCartResponse(List<SeatReservation> seatReservations)
    {
        return new GetItemsFromCartResponse()
        {
            Items = seatReservations.Select(t => new GetItemsFromCart()
            {
                SeatReservationId = t.Id,
                CartId = t.CartId,
                MovieTitle = t.MovieTitle,
                CinemaName = t.CinemaName,
                StartTime = t.StartTime,
                PricePaid = t.PricePaid,
                SeatRow = t.SeatRow,
                SeatNumber = t.SeatNumber
            }).ToList()
        };
    }

    public GetUserCartResponse ToGetUserCartResponse(Cart cart)
    {
        return new GetUserCartResponse()
        {
            Id = cart.Id,
            CustomerId = cart.CustomerId, 
            TotalPrice = cart.TotalPrice,
        };
    }
}