namespace EShop.Orders.API.Application.Queries.OrdersIntendedForAdmin;

public interface IAdminOrderQuery
{
    Task<IEnumerable<AdminOrderSummaryViewModel>> GetAllTheLatestNotOlderThenTwoDaysAgoOrderSummary();
    Task<List<AdminOrderSummaryViewModel>> GetAllOlderThenTwoDaysAgoOrderSummary();
    Task<AdminOrderViewModel> GetOrderByOrderNumber(int orderNumber);
    Task<IEnumerable<OrderStatusViewModel>> GetAllOrderStatus();
    Task<IEnumerable<AdminOrdersByOrderStatus>> GetOrdersByStatus(int statusId);
}