namespace CinemaLite.Application.DTOs.Session.Respone;

public class UpdateSessionResponse
{
    public Guid Id { get; set; }
    public DateTime StartTime { get; set; }
    public string CinemaName { get; set; }
    public decimal Price { get; set; }
    public int AvailableSeats { get; set; }
}