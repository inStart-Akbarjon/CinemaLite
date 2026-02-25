using CinemaLite.Domain.Enums;

namespace CinemaLite.Domain.Models;

public class Movie : BaseEntity
{
    public required string Title { get; set; }
    public required int DurationMinutes { get; set; }
    public required string Genre { get; set; }
    public MovieStatus Status { get; set; }
    public decimal MinPrice { get; set; }
    public bool IsTop { get; set; }
    public int TopSubscriptionPeriod { get; set; }
    public DateTime? TopSubscriptionStartDate { get; set; }
    public List<Session> Sessions { get; set; } = [];
}