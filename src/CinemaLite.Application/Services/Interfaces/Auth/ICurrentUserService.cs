namespace CinemaLite.Application.Services.Interfaces.Auth;

public interface ICurrentUserService
{
    int UserId { get; }
    string UserPhone { get; }
    string UserEmail { get; }
}