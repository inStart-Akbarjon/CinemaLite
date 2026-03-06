namespace CinemaLite.Contracts.Events;

public class UserEmailReminderEvent
{
    public string UserEmail { get; set; }
    public string Message { get; set; }
    public string Subject { get; set; }
}