﻿namespace EShop.Orders.Domain.AggregatesModel.OrderAggregate;

public interface IOrderRepository : IRepository<Order>
{
    Order Add(Order order);

    void Update(Order order);

    Task<Order> GetAsync(int orderId);
}