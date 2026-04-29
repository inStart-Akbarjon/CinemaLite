namespace CinemaLite.Application.Services.Interfaces.Common;

public interface IOpenSeatsStatus
{
    public Task OpenSeatsStatusFromCart(Guid cartId, CancellationToken cancellationToken);
    public Task OpenSeatsStatusFromOrder(Guid orderId, CancellationToken cancellationToken);
}