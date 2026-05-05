using CinemaLite.Application.DTOs.Order.Response;
using CinemaLite.Application.Interfaces.Mappers;
using CinemaLite.Domain.Models;

namespace CinemaLite.Application.Mappers;

public class OrderMapper : IOrderMapper
{
    public GetItemsFromOrderResponse ToGetItemsFromOrderResponse(List<SeatReservation> seatReservations)
    {
        return new GetItemsFromOrderResponse()
        {
            Items = seatReservations.Select(t => new GetItemsFromOrder()
            {
                SeatReservationId = t.Id,
                Order = t.OrderId,
                MovieTitle = t.MovieTitle,
                CinemaName = t.CinemaName,
                StartTime = t.StartTime,
                PricePaid = t.PricePaid,
                SeatRow = t.SeatRow,
                SeatNumber = t.SeatNumber
            }).ToList()
        };
    }
}