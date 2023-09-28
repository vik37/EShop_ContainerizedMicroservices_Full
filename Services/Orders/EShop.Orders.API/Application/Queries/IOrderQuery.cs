namespace EShop.Orders.API.Application.Queries;

public interface IOrderQuery
{
    Task<IEnumerable<OrderSummaryViewModel>> GetOrdersAsync();

    Task<OrderViewModel> GetOrderByIdAsync(int id, Guid userId);

    Task<IEnumerable<OrderSummaryViewModel>> GetOrdersByUserAsync(Guid userId);

    Task<IEnumerable<OrderItemViewModel>> GetAllProductsByUserAsync(Guid userId);

    Task<IEnumerable<CardTypeViewModel>> GetCardTypesAsync();
}
