using CinemaLite.Domain.Enums;

namespace CinemaLite.Domain.Models;

public class Movie : BaseEntity
{
    public required string Title { get; set; }
    public required int DurationMinutes { get; set; }
    public required string Genre { get; set; }
    public required MovieStatus Status { get; set; }
    public decimal MinPrice { get; set; } // the session's price which has the lowest (most cheap one) value will be set in this property 
    public List<Session> Sessions { get; set; } = [];
}