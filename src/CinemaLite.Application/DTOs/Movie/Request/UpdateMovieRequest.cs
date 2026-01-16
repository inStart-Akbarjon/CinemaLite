namespace CinemaLite.Application.DTOs.Movie.Request;

public class UpdateMovieRequest
{
    public string Title { get; set; }
    public int DurationMinutes { get; set; }
    public string Genre { get; set; }
}