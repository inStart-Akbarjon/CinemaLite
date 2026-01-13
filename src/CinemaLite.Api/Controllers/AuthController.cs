using CinemaLite.Application.DTOs.Auth.Registration.Request;
using CinemaLite.Application.DTOs.Auth.Registration.Respone;
using CinemaLite.Application.DTOs.Auth.Login.Respone;
using CinemaLite.Application.DTOs.Auth.Login.Request;
using CinemaLite.Application.CQRS.Auth.Login.Commands;
using CinemaLite.Application.CQRS.Auth.Register.Commands;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace CinemaLite.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IMediator mediator) : ControllerBase
{
    [HttpPost("register")]
    public async Task<RegisterApplicationUserResponse> RegisterCustomer([FromBody] RegisterApplicationUserRequest request)
    {
        var command = new RegisterCommand(request.Email, request.Password);
        return await mediator.Send(command);
    }
    
    [HttpPost("login")]
    public async Task<LoginApplicationUserResponse> Login([FromBody] LoginApplicationUserRequest request)
    {
        var command = new LoginCommand(request.Email, request.Password);
        return await mediator.Send(command);
    }
}