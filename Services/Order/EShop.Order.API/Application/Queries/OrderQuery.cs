using Dapper;
using System.Data.SqlClient;

namespace EShop.Order.API.Application.Queries;

public class OrderQuery : IOrderQuery
{
    private readonly string _connectionString;

    public OrderQuery(string connectionString)
    {
         _connectionString = connectionString;
    }

    public async Task<IEnumerable<OrderSummary>> getOrdersAsync()
    {
        using(var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            return await connection.QueryAsync<OrderSummary>(
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
    }
}
