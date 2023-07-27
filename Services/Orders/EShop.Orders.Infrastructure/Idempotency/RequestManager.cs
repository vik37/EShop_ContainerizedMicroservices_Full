namespace EShop.Orders.Infrastructure.Idempotency;

public class RequestManager : IRequestManager
{
    private readonly OrderContext _context;
    public RequestManager(OrderContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<bool> ExistAsync(Guid id)
    {
        var request = await _context.FindAsync<ClientRequest>(id);

        return request is not null;
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