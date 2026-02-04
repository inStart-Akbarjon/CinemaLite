namespace CinemaLite.Application.CQRS.Movie.Events;

public record MovieCacheInvalidationEvent
{
    public Guid Id { get; set; }
}