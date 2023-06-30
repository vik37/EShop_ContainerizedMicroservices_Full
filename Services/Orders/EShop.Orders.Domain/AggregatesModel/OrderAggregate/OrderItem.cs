namespace EShop.Orders.Domain.AggregatesModel.OrderAggregate;

public class OrderItem : Entity
{
    private string _productName = string.Empty;
    private string _pictureUrl = string.Empty;
    private decimal _unitPrice;
    private decimal _discount;
    private int _units;

    public int ProductId { get; set; }

    protected OrderItem()
    {}

    public OrderItem(int productId, string productName, decimal unitPrice, decimal discount, string picturUrl, int units = 1)
    {
        if (units <= 0)
            throw new OrderDomainException("Invalid number of units");
        if ((unitPrice * units) < discount)
            throw new OrderDomainException("The total oforder items is lower than applied discount");
        ProductId = productId;
        _productName = productName;
        _unitPrice = unitPrice;
        _discount = discount;
        _pictureUrl = picturUrl;
        _units = units;
    }

    public string GetPictureUrl => _pictureUrl;

    public decimal GetUnits() => _unitPrice;

    public decimal GetUnitPrice() => _unitPrice;

    public decimal GetCurrentDiscount() => _discount;

    public string GetOrderItemProductName() => _productName;

    public void SetNewDiscount(decimal discount)
    {
        if (discount < 0)
            throw new OrderDomainException("Invalid Discount");
        _discount = discount;
    }

    public void AddUnits(int units)
    {
        if (units < 0) 
            throw new OrderDomainException("Invalid Units");
        
        _units += units;
    }
}
