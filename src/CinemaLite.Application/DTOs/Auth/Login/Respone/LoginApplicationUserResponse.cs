namespace CinemaLite.Application.DTOs.Auth.Login.Respone;

public class LoginApplicationUserResponse
{
    public string AccessToken { get; set; }
    public int ExpiresIn { get; set; }
}