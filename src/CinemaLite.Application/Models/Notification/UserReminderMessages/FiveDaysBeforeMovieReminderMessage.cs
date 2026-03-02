namespace CinemaLite.Application.Models.Notification.UserReminderMessages;

public class FiveDaysBeforeMovieReminderMessage : BaseMessage
{
    public FiveDaysBeforeMovieReminderMessage(
        string MovieTitle, 
        DateTime StartTime)
    {
        Message =
            $"Hi! Your movie {MovieTitle} is coming up in 5 days. Showtime: {StartTime}. Don't forget to plan ahead and grab your snacks!";
    }
}