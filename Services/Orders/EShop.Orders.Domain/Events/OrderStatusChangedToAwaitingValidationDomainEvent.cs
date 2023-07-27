namespace EShop.Orders.Domain.Events;

public record OrderStatusChangedToAwaitingValidationDomainEvent : INotification
{
    public int OrderId { get; }
    public IEnumerable<OrderItem> OrderItems { get; set; }

    public OrderStatusChangedToAwaitingValidationDomainEvent(int orderId, IEnumerable<OrderItem> orderItems)
    {
        OrderId = orderId;
        OrderItems = orderItems;
    }
}
