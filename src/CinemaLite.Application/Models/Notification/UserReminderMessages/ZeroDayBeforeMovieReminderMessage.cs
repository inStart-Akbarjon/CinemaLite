namespace CinemaLite.Application.Models.Notification.UserReminderMessages;

public class ZeroDayBeforeMovieReminderMessage : BaseMessage
{
    public ZeroDayBeforeMovieReminderMessage(
        string MovieTitle, 
        DateTime StartTime)
    {
        Message =
            $"Hi! Your movie {MovieTitle} is today! Showtime: {StartTime}. Arrive on time and enjoy the movie!";
    }
}