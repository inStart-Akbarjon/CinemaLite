namespace CinemaLite.Application.DTOs.Cart.Request;

public class AddTicketToCartRequest
{
    public int SeatRow { get; set; }
    public int SeatNumber { get; set; }
}