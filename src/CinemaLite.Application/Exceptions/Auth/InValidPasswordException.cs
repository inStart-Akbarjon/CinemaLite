namespace CinemaLite.Application.Exceptions.Auth;

public class InValidPasswordException(string email) : Exception($"The password provided by user with this email '{email}' is not correct.")
{
    
}