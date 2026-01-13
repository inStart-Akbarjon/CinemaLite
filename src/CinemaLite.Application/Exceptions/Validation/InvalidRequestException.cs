namespace CinemaLite.Application.Exceptions.Validation;

public class InvalidRequestException(string message) : Exception($"Invalid request: '{message}'")
{
    
}