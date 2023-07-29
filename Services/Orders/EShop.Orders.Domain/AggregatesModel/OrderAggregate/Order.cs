using EShop.Orders.Domain.Events;

namespace EShop.Orders.Domain.AggregatesModel.OrderAggregate;

public class Order : Entity, IAggregateRoot
{
    private DateTime _orderDate;

    public Address? Address { get; private set; }

    private int? _buyerId;
    public int? GetBuyerId => _buyerId;

    public OrderStatus? OrderStatus { get; private set; }
    private int _orderStatusId;

    private string _description = string.Empty;

    private int? _paymentMethodId;

    private bool _isDraft = false;

    public static Order NewDraft()
    {
        var order = new Order();
        order._isDraft = true;
        return order;
    }

    protected Order()
    {
        _orderItems = new List<OrderItem>();
        _isDraft = false;
    }
    private readonly List<OrderItem> _orderItems;

    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;

    public Order(string userId, string userName, Address address, int cardTypeId, string cardNumber, string cardSecurityNumber,
        string cardHolderName, DateTime cardExpirationDate, int? buyerId = null, int? paymentMethodId = null)
    {
        _orderItems = new List<OrderItem>();
        _buyerId = buyerId;
        _paymentMethodId = paymentMethodId;
        _orderStatusId = OrderStatus.Submitted.Id;
        _orderDate = DateTime.UtcNow;
        Address = address;

        AddOrderStartedDomainEvent(userId, userName, cardTypeId, cardNumber, cardSecurityNumber, cardHolderName, cardExpirationDate);
    }

    public void AddOrderItem(int productId, string productName, decimal unitPrice, decimal discount, string pictureUrl, int units = 1)
    {
        var orderItem =  new OrderItem(productId, productName, unitPrice, discount, pictureUrl, units);
        _orderItems.Add(orderItem);
    }

    public void SetPaymentId(int id)
    {
        _paymentMethodId = id;
    }

    public void SetBuyerId(int buyerId)
    {
        _buyerId = buyerId;
    }

    public void SetAwaitingValidationStatus()
    {
        if(_orderStatusId == OrderStatus.Submitted.Id)
        {
            AddDomainEvent(new OrderStatusChangedToAwaitingValidationDomainEvent(Id,_orderItems));
            _orderStatusId = OrderStatus.AwaitingValidation.Id;
        }
    }

    public void SetStockConfirmedStatus()
    {
        if(_orderStatusId == OrderStatus.StockConfirmed.Id)
        {
            AddDomainEvent(new OrderStatusChangedToStockConfirmedDomainEvent(Id));
            _orderStatusId= OrderStatus.StockConfirmed.Id;
            _description = "All the items were confirmed ith available stock.";
        }
    }

    public void SetPaiedStatus()
    {
        if(_orderStatusId == OrderStatus.Paid.Id)
        {
            AddDomainEvent(new OrderStatusChangedToPaidDomainEvent(Id, OrderItems));
            _orderStatusId = OrderStatus.Paid.Id;
        }
    }

    public void SetShippedStatus()
    {
        if (_orderStatusId == OrderStatus.Paid.Id)
            StatusChangedException(OrderStatus.Shipped);

        _orderStatusId = OrderStatus.Shipped.Id;
        _description = "The order was shipped";

        AddDomainEvent(new OrderShippedDomainEvent(this));
    }

    public void SetCancelledStatus()
    {
        if(_orderStatusId == OrderStatus.Paid.Id ||_orderStatusId == OrderStatus.Shipped.Id)
        {
            StatusChangedException(OrderStatus.Cancelled);
        }

        _orderStatusId = OrderStatus.Cancelled.Id;
        _description = "The Order was Cancelled";
        AddDomainEvent(new OrderCancelledDomainEvent(this));
    }

    public void SetCancelledStatusWhenStockIsRejected(IEnumerable<int> orderStockRejectedItems)
    {
        if(_orderStatusId == OrderStatus.AwaitingValidation.Id)
        {
            _orderStatusId = OrderStatus.Cancelled.Id;

            var itemStockRejectedProductNames = OrderItems.
                                                    Where(x => orderStockRejectedItems.Contains(x.ProductId))
                                                    .Select(x => x.GetOrderItemProductName());

            var itemsStockRejectedDescription = string.Join(", ", itemStockRejectedProductNames);
            _description = $"Product items don't have stock: ({itemStockRejectedProductNames}).";
        }
    }

    public decimal GetTotal()
         => _orderItems.Sum(oi => oi.GetUnits() * oi.GetUnitPrice());

    private void StatusChangedException(OrderStatus orderStatusToChange)
    {
        throw new OrderDomainException($"It is not possible to change the order status from {OrderStatus!.Name} to {orderStatusToChange.Name}");
    }

    private void AddOrderStartedDomainEvent(string userId, string userName, int cardTypeId, string cardNumber, string cardSecurityNumber,
                                                string cardHolderName, DateTime cardExpiration)
    {
        var orderStartedDomainEvent = new OrderStartedDomainEvents(this, userId, userName, cardTypeId, cardNumber, cardSecurityNumber, cardHolderName, cardExpiration);

        this.AddDomainEvent(orderStartedDomainEvent);
    }
}
