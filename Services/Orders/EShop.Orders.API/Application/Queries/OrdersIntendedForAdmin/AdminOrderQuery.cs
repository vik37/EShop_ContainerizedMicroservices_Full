namespace EShop.Orders.API.Application.Queries.OrdersIntendedForAdmin;

public class AdminOrderQuery : IAdminOrderQuery
{
    private readonly string _connectionString;

    public AdminOrderQuery(string connectionString)
        =>   _connectionString = connectionString;    

    public async Task<IEnumerable<AdminOrderSummaryViewModel>> GetAllTheLatestNotOlderThenTwoDaysAgoOrderSummary()
    {
        using var connection = new SqlConnection(_connectionString);

        connection.Open();
        return await connection.QueryAsync<AdminOrderSummaryViewModel>(
                @"select o.Id as OrderNumber, o.OrderDate, o.[Description], os.[Name] as [Status], b.[Name] as BuyerName, 
                    sum(oi.UnitPrice * oi.Units) as Total
                from [ordering].[Orders] as o
                left join [ordering].[Buyers] as b
                on o.BuyerId = b.Id
                left join [ordering].[OrderStatus] as os
                on o.OrderStatusId = os.Id
                left join [ordering].[OrderItems] as oi
                on oi.OrderId = o.Id
                where OrderDate >= DATEADD(DAY, -2, GETDATE())
                group by o.Id, o.OrderDate, o.[Description], os.Id, os.[Name], b.[Name]"
            );
    }

    public async Task<List<AdminOrderSummaryViewModel>> GetAllOlderThenTwoDaysAgoOrderSummary()
    {
        using var connection = new SqlConnection(_connectionString);

        connection.Open();

        var result = await connection.QueryAsync<AdminOrderSummaryViewModel>(
                @"select o.Id as OrderNumber, o.OrderDate, o.[Description], os.[Name] as [Status], b.[Name] as BuyerName, 
                    sum(oi.UnitPrice * oi.Units) as Total
                from [ordering].[Orders] as o
                left join [ordering].[Buyers] as b
                on o.BuyerId = b.Id
                left join [ordering].[OrderStatus] as os
                on o.OrderStatusId = os.Id
                left join [ordering].[OrderItems] as oi
                on oi.OrderId = o.Id
                where OrderDate < DATEADD(DAY, -2, GETDATE())
                group by o.Id, o.OrderDate, o.[Description], os.Id, os.[Name], b.[Name]"
            );
        return result.ToList();
    }

    public async Task<AdminOrderViewModel> GetOrderByOrderNumber(int orderNumber)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);

            connection.Open();

            var adminOrderVM = await connection.QueryFirstAsync<AdminOrderViewModel>(
                    @"select * from [ordering].[VW_OrderIntendedForAdminByOrderNumber] where OrderNumber = @orderNumber",
                    new { orderNumber }
                );
            var orderItems = await connection.QueryAsync<OrderItemsViewModels>(
                    @"select ProductName, Units, UnitPrice, PictureUrl
                from [ordering].[OrderItems] where OrderId = @orderNumber",
                    new { orderNumber }
                );
            foreach (var item in orderItems)
            {
                adminOrderVM.OrderItems.Add(item);
            }
            return adminOrderVM;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message,ex);
        }
    }

    public async Task<IEnumerable<OrderStatusViewModel>> GetAllOrderStatus()
    {
        using var connection = new SqlConnection(_connectionString);

        connection.Open();

        return await connection.QueryAsync<OrderStatusViewModel>("SELECT * FROM [ordering].[OrderStatus]");
    }

    public async Task<IEnumerable<AdminOrdersByOrderStatus>> GetOrdersByStatus(int statusId)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);

            connection.Open();

           var result = await connection.QueryAsync<dynamic>(
                    @"select * from [ordering].[VW_AdminOrdersByOrderStatus] where StatusId = @statusId  order by OrderDate desc",
                    new { statusId }
                );
            if (result == null)
                return null;
            return MapToAdminOrdersByOrderStatus(result);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    private static IEnumerable<AdminOrdersByOrderStatus> MapToAdminOrdersByOrderStatus(dynamic result)
    {
        foreach (var orderByStatus in result)
        {
            yield return new AdminOrdersByOrderStatus
            {
                StatusId = orderByStatus.StatusId,
                StatusName = orderByStatus.StatusName,
                OrderNumber = orderByStatus.OrderNumber,
                OrderDate = orderByStatus.OrderDate,
                BuyerName = orderByStatus.BuyerName
            };
        }
    }

    
}