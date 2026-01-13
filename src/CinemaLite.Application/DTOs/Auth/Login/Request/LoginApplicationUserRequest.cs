namespace CinemaLite.Application.DTOs.Auth.Login.Request;

public class LoginApplicationUserRequest
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}