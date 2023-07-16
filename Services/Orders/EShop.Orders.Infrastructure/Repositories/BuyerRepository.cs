namespace EShop.Orders.Infrastructure.Repositories;

public class BuyerRepository : IBuyerRepository
{
    private readonly OrderContext _db;

    public BuyerRepository(OrderContext db)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
    }
    public IUnitOfWork UnitOfWork
    {
        get { return _db; }
    }

    public Buyer AddBuyer(Buyer buyer)
    {
        if(buyer.IsTransient())
            return _db.Buyers.Add(buyer).Entity;

        return buyer;
    }

    public async Task<Buyer> FindAsync(string buyerIdentityGuid)
             =>    await _db.Buyers.Include(b => b.PaymentMethods)
                                   .Where(b=> b.IdentityGuid == buyerIdentityGuid)
                                   .SingleOrDefaultAsync();

    public async Task<Buyer> FindByIdAsync(string id)
          =>  await _db.Buyers.Include(b => b.PaymentMethods)
                               .Where(b => b.Id == int.Parse(id))
                               .SingleOrDefaultAsync();


    public Buyer Update(Buyer buyer)
        => _db.Buyers.Update(buyer).Entity;
}
