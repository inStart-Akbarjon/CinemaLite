using CinemaLite.Application.DTOs.Auth.Login.Respone;
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
    public async Task<IActionResult> RegisterCustomer([FromBody] RegisterCommand request)
    {
        var command = new RegisterCommand(request.Email, request.Password);
        return await mediator.Send(command);
    }
    
    [HttpPost("login")]
    public async Task<LoginApplicationUserResponse> Login([FromBody] LoginCommand request)
    {
        var command = new LoginCommand(request.Email, request.Password);
        return await mediator.Send(command);
    }
}