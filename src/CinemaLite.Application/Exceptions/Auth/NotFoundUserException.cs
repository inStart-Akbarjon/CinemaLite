namespace CinemaLite.Application.Exceptions.Auth;

public class NotFoundUserException(string email) : Exception($"The user with this email '{email}' not found.")
{
    
}