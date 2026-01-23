namespace CinemaLite.Application.DTOs.Session.Request;

public class CreateSessionRequest
{
    public DateTime StartTime {get; set;}
    public string CinemaName {get; set;}
    public int TotalRows { get; set; }
    public int SeatsPerRow { get; set; }
    public decimal Price {get; set;}
}