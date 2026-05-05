namespace CinemaLite.Application.DTOs.Order.Response;

public class GetItemsFromOrderResponse
{
    public List<GetItemsFromOrder> Items { get; set; }
}

public class GetItemsFromOrder
{
    public required Guid SeatReservationId { get; set; }
    public Guid? Order { get; set; }
    public required string MovieTitle { get; set; }
    public required string CinemaName { get; set; }
    public required DateTime StartTime { get; set; }
    public required decimal PricePaid { get; set; }
    public int SeatRow { get; set; }
    public int SeatNumber { get; set; }
}