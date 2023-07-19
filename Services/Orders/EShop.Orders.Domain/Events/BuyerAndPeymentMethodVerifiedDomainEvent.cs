namespace EShop.Orders.Domain.Events;

public record BuyerAndPeymentMethodVerifiedDomainEvent : INotification
{
    public Buyer Buyer { get; init; }
    public PaymentMethod PaymentMethod { get; set; }
    public int OrderId { get; init; }

    public BuyerAndPeymentMethodVerifiedDomainEvent(Buyer buyer, PaymentMethod paymentMethod, int orderId)
    {
        Buyer = buyer;
        PaymentMethod = paymentMethod;
        OrderId = orderId;
    }
}
