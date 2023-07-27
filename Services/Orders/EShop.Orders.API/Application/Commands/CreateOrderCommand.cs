namespace EShop.Orders.API.Application.Commands;

[DataContract]
public class CreateOrderCommand : IRequest<bool>
{
    [DataMember]
    private readonly List<OrderItemDto> _orderItems;

    [DataMember]
    public string UserId { get; private set; } = string.Empty;

    [DataMember]
    public string UserName { get; private set; } = string.Empty;

    [DataMember]
    public string? City { get; private set; }

    [DataMember]
    public string? Street { get; private set; } 

    [DataMember]
    public string? State { get; private set; } 

    [DataMember]
    public string? Country { get; private set; }

    [DataMember]
    public string? ZipCode { get; private set; } 

    [DataMember]
    public string CardNumber { get; private set; } = string.Empty;

    [DataMember]
    public string CardHolderName { get; private set; } = string.Empty;

    [DataMember]
    public DateTime CardExpiration { get; private set; }

    [DataMember]
    public string CardSecurityNumber { get; private set; } = string.Empty;

    [DataMember]    
    public int CardTypeId { get; private set; }

    [DataMember]
    public IEnumerable<OrderItemDto> OrderItems => _orderItems;

    public CreateOrderCommand()
      =>   _orderItems = new List<OrderItemDto>();

    public CreateOrderCommand(List<BasketItems> basketItems, string userId, string userName, string city, string state,
                                string country, string zipCode, string cardNumber, string cardHolderName, DateTime cardExpiration,
                                string cardSecurityNumber, int cardTypeId) : this()
    {
        _orderItems = basketItems.ToOrderItemsDto().ToList();
        UserId = userId;
        UserName = userName;
        City = city;
        State = state;
        Country = country;
        ZipCode = zipCode;
        CardNumber = cardNumber;
        CardHolderName = cardHolderName;
        CardExpiration = cardExpiration;
        CardSecurityNumber = cardSecurityNumber;
        CardTypeId = cardTypeId;
    }

    public class OrderItemDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public int Units { get; set; }
        public string PictureUrl { get; set; } = string.Empty;
    }
}
