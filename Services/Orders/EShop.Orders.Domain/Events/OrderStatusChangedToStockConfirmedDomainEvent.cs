namespace EShop.Orders.Domain.Events;

/// <summary>
///     Event used when the order stock items are confirmed
/// </summary>
public record OrderStatusChangedToStockConfirmedDomainEvent : INotification
{
    public int OrderId { get; init; }

    public OrderStatusChangedToStockConfirmedDomainEvent(int orderId)
        =>    OrderId = orderId;   
}