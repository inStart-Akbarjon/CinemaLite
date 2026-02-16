namespace CinemaLite.Application.DTOs.Movie.Request;

public class UpdateMovieRequest
{
    public string Title { get; set; }
    public int DurationMinutes { get; set; }
    public string Genre { get; set; }
    public bool IsTop { get; set; }
    public int TopSubscriptionPeriod { get; set; }
}