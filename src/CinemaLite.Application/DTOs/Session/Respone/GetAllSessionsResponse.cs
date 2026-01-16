using CinemaLite.Domain.Enums;

namespace CinemaLite.Application.DTOs.Session.Respone;

public class GetAllSessionsFromMovieResponse
{
    public Guid MovieId { get; set; }
    public string Title { get; set; }
    public int DurationMinutes { get; set; }
    public MovieStatus Status {get; set; }
    public decimal MinPrice {get; set; }
    public string Genre { get; set; }
    public List<GetAllSessionsResponse>? Sessions { get; set; }
}

public class GetAllSessionsResponse
{
    public Guid SessionId { get; set; }
    public DateTime StartTime { get; set; }
    public string CinemaName { get; set; }
    public int AvailableSeats { get; set; }
    public decimal Price { get; set; }
}