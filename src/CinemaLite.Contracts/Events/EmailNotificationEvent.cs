namespace CinemaLite.Contracts.Events;

public class EmailNotificationEvent
{
    public string UserEmail { get; set; }
    public string Message { get; set; }
    public string Subject { get; set; }
}