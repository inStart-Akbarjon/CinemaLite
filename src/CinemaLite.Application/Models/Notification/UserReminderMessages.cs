namespace CinemaLite.Application.Models.Notification;

public class UserReminderMessages(string movieTitle, DateTime startTime)
{
    public readonly string FiveDaysBeforeMovieReminderMessage =
        $"Hi! Your movie {movieTitle} is coming up in 5 days. Showtime: {startTime}. Don't forget to plan ahead and grab your snacks!";
    public readonly string ThreeDaysBeforeMovieReminderMessage =
        $"Hi! Your movie {movieTitle} is tomorrow. Showtime: {startTime}. We look forward to seeing you at the cinema!";
    public readonly string ZeroDayBeforeMovieReminderMessage =
        $"Hi! Your movie {movieTitle} is today! Showtime: {startTime}. Arrive on time and enjoy the movie!";
}