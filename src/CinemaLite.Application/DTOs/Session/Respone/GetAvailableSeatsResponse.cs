using CinemaLite.Domain.Enums;
using CinemaLite.Domain.Models;

namespace CinemaLite.Application.DTOs.Session.Respone;

public class GetAvailableSeatsResponse
{
    public Guid MovieId { get; set; }
    public string Title { get; set; }
    public int DurationMinutes { get; set; }
    public MovieStatus Status {get; set; }
    public decimal MinPrice {get; set; }
    public string Genre { get; set; }
    public List<GetSessionWithAvailableSeatsResponse>? Sessions { get; set; }
}

public class GetSessionWithAvailableSeatsResponse
{
    public Guid Id { get; set; }
    public DateTime StartTime { get; set; }
    public string CinemaName { get; set; }
    public int AvailableSeats { get; set; }
    public int TotalRows { get; set; }
    public int SeatsPerRow { get; set; }
    public decimal Price { get; set; }
    public List<Seat> Seats { get; set; } = new();
}