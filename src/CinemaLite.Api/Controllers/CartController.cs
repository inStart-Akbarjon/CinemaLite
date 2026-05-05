using CinemaLite.Application.CQRS.Cart.Commands.AddToCart;
using CinemaLite.Application.CQRS.Cart.Commands.DeleteCart;
using CinemaLite.Application.CQRS.Cart.Commands.DeleteFromCart;
using CinemaLite.Application.CQRS.Cart.Queries.GetItemsFromCart;
using CinemaLite.Application.CQRS.Cart.Queries.GetUserCart;
using CinemaLite.Application.DTOs.Cart.Request;
using CinemaLite.Application.DTOs.Cart.Response;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CinemaLite.Api.Controllers;

[ApiController]
[Route("api/carts")]
public class CartController(IMediator mediator) : ControllerBase
{
    [Authorize]
    [HttpGet("my")]
    public async Task<GetUserCartResponse> GetUserCart(CancellationToken cancellationToken) 
    {
        var query = new GetUserCartQuery();
        return await mediator.Send(query, cancellationToken);
    }
    
    [Authorize]
    [HttpGet("{cartId}/seat-reservations")]
    public async Task<GetItemsFromCartResponse> GetSeatReservationsFromCart(
        [FromRoute] Guid cartId, 
        CancellationToken cancellationToken) 
    {
        var query = new GetItemsFromCartQuery(cartId);
        return await mediator.Send(query, cancellationToken);
    }
    
    [Authorize]
    [HttpPost("{movieId}/sessions/{sessionId}")]
    public async Task<AddSeatReservationToCartResponse> AddSeatReservationToCart(
        [FromRoute] Guid movieId, 
        [FromRoute] Guid sessionId, 
        [FromBody] AddTicketToCartRequest toCartRequest, 
        CancellationToken cancellationToken) 
    {
        var command = new AddToCartCommand(
            movieId,
            sessionId,
            toCartRequest.SeatRow, 
            toCartRequest.SeatNumber);
        
        return await mediator.Send(command, cancellationToken);
    }
    
    [Authorize]
    [HttpDelete("{cartId}/seat-reservations/{seatReservationId}")]
    public async Task<IActionResult> DeleteSeatReservationFromCart(
        [FromRoute] Guid cartId, 
        [FromRoute] Guid seatReservationId, 
        CancellationToken cancellationToken)
    {
        var command = new DeleteFromCartCommand(
            cartId,
            seatReservationId);
        
        return await mediator.Send(command, cancellationToken);
    }
    
    [Authorize]
    [HttpDelete("{cartId}")]
    public async Task<IActionResult> DeleteCart(
        [FromRoute] Guid cartId, 
        CancellationToken cancellationToken)
    {
        var command = new DeleteCartCommand(
            cartId);
        
        return await mediator.Send(command, cancellationToken);
    }
}