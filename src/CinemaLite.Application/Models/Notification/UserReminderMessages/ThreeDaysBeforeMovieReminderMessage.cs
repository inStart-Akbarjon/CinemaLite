namespace CinemaLite.Application.Models.Notification.UserReminderMessages;

public class ThreeDaysBeforeMovieReminderMessage : BaseMessage
{
    public ThreeDaysBeforeMovieReminderMessage(
        string MovieTitle, 
        DateTime StartTime)
    {
        Message =
            $"Hi! Your movie {MovieTitle} is tomorrow. Showtime: {StartTime}. We look forward to seeing you at the cinema!";
    }
}