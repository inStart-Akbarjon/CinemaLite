namespace CinemaLite.Domain.Models;

public class SeatReservation : BaseEntity
{
    public Guid? CartId { get; set; }
    public Guid? OrderId { get; set; }
    public required Guid MovieId { get; set; }
    public required Guid SessionId { get; set; }
    public required string MovieTitle { get; set; }
    public required string CinemaName { get; set; }
    public required DateTime StartTime { get; set; }
    public required decimal PricePaid { get; set; }
    public required int SeatRow { get; set; }
    public required int SeatNumber { get; set; }
} 