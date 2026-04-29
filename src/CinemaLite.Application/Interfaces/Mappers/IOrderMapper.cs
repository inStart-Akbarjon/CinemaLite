using CinemaLite.Application.DTOs.Order.Response;
using CinemaLite.Domain.Models;

namespace CinemaLite.Application.Interfaces.Mappers;

public interface IOrderMapper
{
    public GetItemsFromOrderResponse ToGetItemsFromOrderResponse(List<SeatReservation> seatReservations);
}