using CinemaLite.Application.Services.Implementations.RedisDistributedLock;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using StackExchange.Redis;

namespace CinemaLite.Application.Services.Interfaces.RedisDistributedLock;

public class DistributedLockService : IDistributedLockService
{
    private readonly RedLockFactory _factory;

    public DistributedLockService(IConnectionMultiplexer multiplexer)
    {
        _factory = RedLockFactory.Create([new RedLockMultiplexer(multiplexer)]);
    }
    
    public async Task<IDisposable?> AcquireAsync(
        string key,
        TimeSpan expiry,
        CancellationToken token = default)
    {
        
        var redLock = await _factory.CreateLockAsync(
            resource: key,
            expiryTime: expiry,
            waitTime: TimeSpan.FromMicroseconds(500),
            retryTime: TimeSpan.FromMicroseconds(200),
            cancellationToken: token);

        if (!redLock.IsAcquired)
        {
            return null;
        }
        
        return redLock;
    }
}