using System.Security.AccessControl;

namespace CinemaLite.Application.DTOs.Cart.Request;

public class AddTicketToCartRequest
{
    public Guid SeatId { get; set; }
}