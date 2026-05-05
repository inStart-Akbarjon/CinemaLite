using CinemaLite.Application.CQRS.Cart.Commands.DeleteCart;
using CinemaLite.Contracts.Events;
using MassTransit;
using MediatR;

namespace CinemaLite.Infrastructure.Consumers;

public class CartExpireConsumer(IMediator mediator) : IConsumer<CartCreatedEvent>
{
    public async Task Consume(ConsumeContext<CartCreatedEvent> context)
    {
        var command = new DeleteCartCommand(context.Message.CartId);
        
        await mediator.Send(command);
    }
}