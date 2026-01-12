namespace CinemaLite.Domain.Models;

public class Ticket : BaseEntity
{
    public required string MovieId { get; set; }
    public required string MovieTitle { get; set; }
    public required string CinemaName { get; set; }
    public required DateTime StartTime { get; set; }
    public required decimal PricePaid { get; set; }
    public int UserId { get; set; }
}