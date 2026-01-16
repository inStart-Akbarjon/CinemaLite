namespace CinemaLite.Application.DTOs.Session.Request;

public class UpdateSessionRequest
{
    public Guid MovieId { get; set; }
    public string CinemaName { get; set; }
    public decimal Price { get; set; }
    public int AvailableSeats { get; set; }
    public DateTime StartTime { get; set; }
}