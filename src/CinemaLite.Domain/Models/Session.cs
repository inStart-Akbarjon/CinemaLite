namespace CinemaLite.Domain.Models;

public class Session : BaseEntity
{
    public override Guid Id { get; set; } = Guid.NewGuid();
    public required Guid MovieId { get; set; }
    public required string CinemaName { get; set; }
    public required decimal Price { get; set; }
    public required int AvailableSeats { get; set; }
    public required DateTime StartTime { get; set; }
    
    public void ReduceAvailableSeatsByOne(int currentSeats)
    {
        AvailableSeats = currentSeats - 1;
    }
}