namespace EShop.Orders.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly OrderingContext _db;

    public OrderRepository(OrderingContext db)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
    }
    public IUnitOfWork UnitOfWork
    {
        get { return _db; }
    }

    public Order Add(Order order)
        => _db.Orders.Add(order).Entity;

    public async Task<Order> GetAsync(int orderId)
    {
        var order = await _db.Orders.Include(o => o.Address)
                                    .FirstOrDefaultAsync(o => o.Id == orderId);

        if(order is null)
            order = _db.Orders.Local.FirstOrDefault(o => o.Id == orderId);

        if(order is not null)
        {
            await _db.Entry(order).Collection(x => x.OrderItems).LoadAsync();

            await _db.Entry(order).Reference(x => x.OrderStatus).LoadAsync();
        }

        return order;
    }

    public void Update(Order order)
    {
        _db.Entry(order).State = EntityState.Modified;
    }
}
