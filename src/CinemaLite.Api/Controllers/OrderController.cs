using CinemaLite.Application.CQRS.Order.Commands.CancelOrder;
using CinemaLite.Application.CQRS.Order.Commands.CreateOrder;
using CinemaLite.Application.CQRS.Order.Queries.GetAllUserOrders;
using CinemaLite.Application.CQRS.Order.Queries.GetItemsFromOrder;
using CinemaLite.Application.CQRS.Order.Queries.GetUserOrderById;
using CinemaLite.Application.CQRS.Payment.Commands.CreatePaymentTransaction;
using CinemaLite.Application.CQRS.Payment.Queries.GetOrderPaymentTransaction;
using CinemaLite.Application.DTOs.Order.Response;
using CinemaLite.Application.DTOs.Pagination;
using CinemaLite.Application.DTOs.Payment.Response;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CinemaLite.Api.Controllers;

[ApiController]
[Route("api/orders")]
public class OrderController(IMediator mediator) : ControllerBase
{
    [Authorize]
    [HttpGet("my")]
    public Task<PaginatedQueryList<GetAllUserOrdersResponse>> GetAllOrders([FromQuery] GetAllUserOrdersQuery request)
    {
        var query = new GetAllUserOrdersQuery(request.Status, request.PageNumber, request.PageSize);
        return mediator.Send(query);
    }
    
    [Authorize]
    [HttpGet("{orderId}/seat-reservations")]
    public Task<GetItemsFromOrderResponse> GetSeatReservationsFromOrder([FromRoute] Guid orderId)
    {
        var query = new GetItemsFromOrderQuery(orderId);
        return mediator.Send(query);
    }
    
    [Authorize]
    [HttpGet("{orderId}")]
    public Task<GetUserOrderByIdResponse> GetUserOrderById([FromRoute] Guid orderId)
    {
        var query = new GetUserOrderByIdQuery(orderId);
        return mediator.Send(query);
    }
    
    [Authorize]
    [HttpPost("create")]
    public Task<CreateOrderResponse> CreateOrder([FromQuery] Guid cartId)
    {
        var command = new CreateOrderCommand(cartId);
        return mediator.Send(command);
    }

    [Authorize]
    [HttpPost("{orderId}/cancel")]
    public Task<IActionResult> CancelOrder([FromRoute] Guid orderId)
    {
        var command = new CancelOrderCommand(orderId);
        return mediator.Send(command);
    }
    
    [Authorize]
    [HttpPost("{orderId}/pay")]
    public async Task<CreatePaymentResponse> CreatePayment([FromRoute] Guid orderId, CancellationToken cancellationToken)
    {
        var command = new CreatePaymentTransactionCommand(orderId);
        return await mediator.Send(command, cancellationToken);
    }
    
    [Authorize]
    [HttpGet("{orderId}/payment")]
    public async Task<GetOrderPaymentTransactionResponse> GetOrderPayment([FromRoute] Guid orderId, CancellationToken cancellationToken)
    {
        var query = new GetOrderPaymentTransactionQuery(orderId);
        return await mediator.Send(query, cancellationToken);
    }
}