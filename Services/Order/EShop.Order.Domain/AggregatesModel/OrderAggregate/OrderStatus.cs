﻿using EShop.Order.Domain.Exceptions;
using EShop.Order.Domain.SeedWork;

namespace EShop.Order.Domain.AggregatesModel.OrderAggregate;

public class OrderStatus : Enumeration
{
    public static OrderStatus Submitted = new OrderStatus(1,nameof(Submitted).ToLowerInvariant());

    public static OrderStatus AwaitingValidation = new OrderStatus(2, nameof(AwaitingValidation).ToLowerInvariant());

    public static OrderStatus StockConfirmed = new OrderStatus(3, nameof(StockConfirmed).ToLowerInvariant());

    public static OrderStatus Paid = new OrderStatus(4, nameof(Paid).ToLowerInvariant());

    public static OrderStatus Shipped = new OrderStatus(5, nameof(Shipped).ToLowerInvariant());

    public static OrderStatus Cancelled = new OrderStatus(6, nameof(Cancelled).ToLowerInvariant());

    public OrderStatus(int id, string name) : base(id, name) { }

    public static IEnumerable<OrderStatus> List() 
        => new[] {Submitted, AwaitingValidation, StockConfirmed, Paid, Shipped, Cancelled}; 

    public static OrderStatus FromName(string name)
    {
        var state = List().SingleOrDefault(x => string.Equals(x.Name,name,StringComparison.CurrentCultureIgnoreCase));

        if (state is null)
            throw new OrderDomainException($"Possibe values for order status: {string.Join(",", List().Select(s => s.Name))}");

        return state;
    }

    public static OrderStatus From(int id)
    {
        var state = List().SingleOrDefault(x => x.Id == id);

        if(state is null)
            throw new OrderDomainException($"Possible values of order status: {string.Join(",", List().Select(s => s.Name))}");

        return state;
    }
}
