using EShop.Order.Domain.SeedWork;

namespace EShop.Order.Domain.AggregatesModel.BuyerAggregate;

public interface IBuyerRepository : IRepository<Buyer>
{
    Buyer AddBuyer(Buyer buyer);
    Buyer Update(Buyer buyer);
    Task<Buyer> FindAsync(string buyerIdentityGuid);
    Task<Buyer> FindByIdAsync(string id);
}
