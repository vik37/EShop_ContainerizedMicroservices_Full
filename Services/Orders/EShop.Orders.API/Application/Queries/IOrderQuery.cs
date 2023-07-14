namespace EShop.Orders.API.Application.Queries;

public interface IOrderQuery
{
    Task<IEnumerable<OrderSummary>> GetOrdersAsync();
}
