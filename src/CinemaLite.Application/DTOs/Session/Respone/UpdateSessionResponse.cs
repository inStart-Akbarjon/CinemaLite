namespace CinemaLite.Application.DTOs.Session.Respone;

public class UpdateSessionResponse
{
    public Guid Id { get; set; }
    public string CinemaName { get; set; }
    public int AvailableSeats { get; set; }
    public int TotalRows { get; set; }
    public int SeatsPerRow { get; set; }
    public decimal Price { get; set; }
    public DateTime StartTime { get; set; }
}