namespace CinemaLite.Application.DTOs.Session.Request;

public class UpdateSessionRequest
{
    public string CinemaName { get; set; }
    public decimal Price { get; set; }
    public int TotalRows { get; set; }
    public int SeatsPerRow { get; set; }
    public DateTime StartTime { get; set; }
}