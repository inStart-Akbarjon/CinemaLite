namespace CinemaLite.Application.DTOs.Cart.Response;

public class AddSeatReservationToCartResponse
{
    public Guid SeatReservationId { get; set; }
    public string MovieTitle { get; set; }
    public int SeatRow { get; set; }
    public int SeatNumber { get; set; }
}