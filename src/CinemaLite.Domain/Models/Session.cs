namespace CinemaLite.Domain.Models;

public class Session : BaseEntity
{
    public override Guid Id { get; set; } = Guid.NewGuid();
    public required string CinemaName { get; set; }
    public required decimal Price { get; set; }
    public required int AvailableSeats { get; set; }
    public required DateTime StartTime { get; set; }
    public int TotalRows { get; set; }
    public int SeatsPerRow { get; set; }
    public List<Seat> Seats { get; set; } = new();
    
    public void ReduceAvailableSeatsByOne(int currentSeats)
    {
        AvailableSeats = currentSeats - 1;
    }
}

public class Seat
{
    public int SeatRow { get; set; } 
    public int SeatNumber { get; set; }
    public bool IsBooked { get; set; }
}