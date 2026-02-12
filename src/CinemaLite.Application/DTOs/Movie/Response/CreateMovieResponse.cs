using CinemaLite.Domain.Enums;

namespace CinemaLite.Application.DTOs.Movie.Response;

public class CreateMovieResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public int DurationMinutes { get; set; }
    public string? Status { get; set; }
    public string Genre { get; set; }
    public bool IsTop { get; set; }
    public int TopSubscriptionPeriod { get; set; }
}