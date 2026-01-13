namespace CinemaLite.Application.Exceptions.Auth;

public class DuplicateUserEmailException(string email) : Exception($"User with this email '{email}' already exists.")
{
    
}