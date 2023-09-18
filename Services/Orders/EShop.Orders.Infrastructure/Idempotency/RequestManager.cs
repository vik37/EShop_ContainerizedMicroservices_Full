using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EShop.Orders.Infrastructure.Idempotency;

public class RequestManager : IRequestManager
{
    private readonly OrderingContext _context;
    private readonly ILogger<RequestManager> _logger;
    public RequestManager(OrderingContext context, ILogger<RequestManager> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<bool> ExistAsync(Guid id)
    {
        try
        {
            var request = await _context.FindAsync<ClientRequest>(id);

            return request != null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"Request Manager Exist Async - {message}", ex.Message);
            throw;
        }
    }

    public async Task CreateRequestForCommandAsync<T>(Guid id)
    {

        var exist = await ExistAsync(id);
        var request = exist ? throw new OrderDomainException($"Request with ID {id} allready exist")
            : new ClientRequest
            {
                Id = id,
                Name = typeof(T).Name,
                Time = DateTime.UtcNow
            };

        _context.Add(request);

        await _context.SaveChangesAsync();

    }
}