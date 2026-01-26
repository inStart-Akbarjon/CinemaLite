namespace CinemaLite.Application.Services.Implementations.RedisDistributedLock;

public interface IDistributedLockService
{
    Task<IDisposable?> AcquireAsync(string key, TimeSpan expiry, CancellationToken token = default);
}