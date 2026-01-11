namespace CinemaLite.Domain.Models;

public class Movie : BaseEntity
{
    public required string Title { get; set; }
    public required int DurationMinutes { get; set; }
    public required string Genre { get; set; }
    public string Status { get; set; }
    public ICollection<Session>? Sessions { get; set; }
}