namespace CinemaLite.Application.DTOs.Session.Respone;

public class CreateSessionResponse
{
    public Guid Id { get; set; }
    public string CinemaName { get; set; }
    public int AvailableSeats { get; set; }
    public decimal Price { get; set; }
    public DateTime StartTime { get; set; }
}   