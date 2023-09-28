namespace EShop.Orders.API.Application.Queries;

public class OrderQuery : IOrderQuery
{
    private readonly string _connectionString;

    public OrderQuery(string connectionString)
    {
         _connectionString = connectionString;
    }    

    public async Task<IEnumerable<OrderSummaryViewModel>> GetOrdersAsync()
    {
        using var connection = new SqlConnection(_connectionString);

            connection.Open();
            return await connection.QueryAsync<OrderSummaryViewModel>(
                    @"SELECT o.[Id] as OrderNumber,
                        o.[OrderDate] as [Date], os.[Name] as Status,
                        SUM(oi.Units*oi.UnitPrice) as Total
                        FROM [ordering].[Orders] o
                        LEFT JOIN [ordering].[OrderItems] oi on o.Id = oi.OrderId
                        LEFT JOIN [ordering].[OrderStatus] os on o.OrderStatusId = os.Id
                        GROUP BY o.[Id], o.[OrderDate], os.[Name]
                        ORDER BY o.[Id]"
                );
    }

    public async Task<OrderViewModel> GetOrderByIdAsync(int id, Guid userId)
    {
        using var connection = new SqlConnection( _connectionString);
        connection.Open();

        var result = await connection.QueryAsync<dynamic>(
                        @"SELECT o.[Id] as OrderNumber, o.[OrderDate] as Date, o.[Description] as Description, o.[Address_City] as City, o.[Address_Country] as Country,
                                    o.[Address_State] as State, o.[Address_Street] as Street, o.[Address_ZipCode] as ZipCode, 
                                    os.[Name] as Status,
                                    oi.[ProductName] as ProductName, oi.[Units] as Units, oi.[UnitPrice] as UnitPrice, oi.[PictureUrl] as PictureUrl
                        FROM ordering.Orders o
                        LEFT JOIN ordering.OrderItems oi ON o.Id = oi.OrderId
                        LEFT JOIN ordering.OrderStatus os ON o.OrderStatusId = os.Id
                        LEFT JOIN ordering.Buyers b ON o.BuyerId = b.Id
                        WHERE o.Id = @id and b.IdentityGuid = @userId", new { id,userId }
                    );

        if(result.AsList().Count == 0)
            throw new KeyNotFoundException();

        return MapToOrderItem(result);
    }

    public async Task<IEnumerable<OrderSummaryViewModel>> GetOrdersByUserAsync(Guid userId)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        return await connection.QueryAsync<OrderSummaryViewModel>(
                @"SELECT o.[Id] as OrderNumber, o.[OrderDate] as Date, os.[Name] as Status, SUM(oi.Units * oi.UnitPrice) as Total
                    FROM ordering.Orders o
                    LEFT JOIN ordering.OrderItems oi ON o.Id = oi.OrderId
                    LEFT JOIN ordering.OrderStatus os ON o.OrderStatusId = os.Id
                    LEFT JOIN ordering.Buyers b ON o.BuyerId = b.Id
                    WHERE b.IdentityGuid = @userId
                    GROUP BY o.Id, o.OrderDate, os.[Name]
                    ORDER BY o.Id", new { userId }
            );
    }

    public async Task<IEnumerable<OrderItemViewModel>> GetAllProductsByUserAsync(Guid userId)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        var result = await connection.QueryAsync<dynamic>(@"
            select oi.ProductName, oi.UnitPrice, oi.Units, oi.PictureUrl
            from [ordering].[OrderItems] as oi
            left join [ordering].[Orders] as o 
            on o.Id = oi.OrderId
            left join [ordering].[Buyers] as b
            on b.Id = o.BuyerId
            where b.IdentityGuid = @userId
        ", new { userId});

       return  result.Select(row => new OrderItemViewModel
        {
            ProductName = row.ProductName,
            UnitPrice = (double)row.UnitPrice,
            Units = row.Units,
            PictureUrl = row.PictureUrl
        });
    }

    public async Task<IEnumerable<CardTypeViewModel>> GetCardTypesAsync()
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        return await connection.QueryAsync<CardTypeViewModel>("SELECT * FROM ordering.CardTypes");
    }

    private static OrderViewModel MapToOrderItem(dynamic result)
    {
        var order = new OrderViewModel()
        {
            OrderNumber = result[0].OrderNumber,
            Date = result[0].Date,
            Status = result[0].Status,
            Description = result[0].Description,
            Street = result[0].Street,
            City = result[0].City,
            ZipCode = result[0].ZipCode,
            Country = result[0].Country,
            Total = 0
        };

        foreach (dynamic item in result)
        {
            var orderItem = new OrderItemViewModel
            {
                ProductName = item.ProductName,
                PictureUrl = item.PictureUrl,
                Units = item.Units,
                UnitPrice = (double)item.UnitPrice
            };

            order.Total += item.Units * item.UnitPrice;

            order.OrderItems.Add(orderItem);
        }
        
        return order;
    }   
}
