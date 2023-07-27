namespace EShop.Orders.Domain.Events;

public record OrderCancelledDomainEvent : INotification
{
    public Order Order { get; set; }

    public OrderCancelledDomainEvent(Order order)
    {
        Order = order;
    }
}
