namespace EShop.Orders.API.Application.Queries;

public interface IOrderQuery
{
    Task<IEnumerable<OrderSummaryViewModel>> GetOrdersAsync();

    Task<OrderViewModel> GetOrderByIdAsync(int id);

    Task<IEnumerable<OrderSummaryViewModel>> GetOrdersFromUserAsync(Guid userId);

    Task<IEnumerable<CardTypeViewModel>> GetCardTypesAsync();
}
