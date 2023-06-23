namespace EShop.Order.API.Application.Queries;

public interface IOrderQuery
{
    Task<IEnumerable<OrderSummary>> getOrdersAsync();
}
