namespace CinemaLite.Domain.Models;

public class Ticket : BaseEntity
{
    public required int UserId { get; set; }
    public required Guid MovieId { get; set; }
    public required Guid SessionId { get; set; }
    public required string MovieTitle { get; set; }
    public required string CinemaName { get; set; }
    public required int SeatRow { get; set; }
    public required int SeatNumber { get; set; }
    public required DateTime StartTime { get; set; }
    public required decimal PricePaid { get; set; }
}