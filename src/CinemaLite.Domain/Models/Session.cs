namespace CinemaLite.Domain.Models;

public class Session : BaseEntity
{
    public required DateTime StartTime { get; set; }
    public required string CinemaName { get; set; }
    public required int AvailableSeats { get; set; }
    public required int MovieId { get; set; }
}