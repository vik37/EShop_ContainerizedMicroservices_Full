namespace EShop.Orders.Domain.Events;

public record OrderStartedDomainEvents : INotification
{
    public string UserId { get; init; }
    public string UserName { get; init; }
    public int CardTypeId { get; init; }
    public string CardNumber { get; init; }
    public string CardSecurityNumber { get; init; }
    public string CardHolderName { get; init; }
    public DateTime CardExpiration { get; init; }
    public Order Order { get; init; }

    public OrderStatus OrderStatus { get; set; }

    public OrderStartedDomainEvents(Order order, string userId, string userName, int cardTypeId, string cardNumber,
                                        string cardSecurityNumber, string cardHolderName, DateTime cardExpiration, 
                                        OrderStatus orderStatus)
    {
        Order = order;
        UserId = userId;
        UserName = userName;
        CardTypeId = cardTypeId;
        CardNumber = cardNumber;
        CardSecurityNumber = cardSecurityNumber;
        CardHolderName = cardHolderName;
        CardExpiration = cardExpiration;
        OrderStatus = orderStatus;
    }
}