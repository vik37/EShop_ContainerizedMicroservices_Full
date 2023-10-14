namespace EShop.Orders.API.Application.Queries.OrderQueries;

public interface IOrderQuery
{
    Task<OrderViewModel> GetOrderByIdAsync(int id, Guid userId);

    Task<IEnumerable<OrderSummaryViewModel>> GetOrdersByUserAsync(Guid userId);

    Task<IEnumerable<OrderItemsViewModel>> GetAllProductsByUserAsync(Guid userId);

    Task<IEnumerable<CardTypeViewModel>> GetCardTypesAsync();
}
