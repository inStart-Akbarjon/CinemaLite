namespace CinemaLite.Application.DTOs.Cart.Response;

public class GetItemsFromCartResponse
{
    public List<GetItemsFromCart> Items { get; set; }
}

public class GetItemsFromCart
{
    public required Guid SeatReservationId { get; set; }
    public Guid? CartId { get; set; }
    public required string MovieTitle { get; set; }
    public required string CinemaName { get; set; }
    public required DateTime StartTime { get; set; }
    public required decimal PricePaid { get; set; }
    public int SeatRow { get; set; }
    public int SeatNumber { get; set; }
}