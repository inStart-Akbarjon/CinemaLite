namespace CinemaLite.Application.DTOs.Auth.Registration.Request;

public class RegisterApplicationUserRequest
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}