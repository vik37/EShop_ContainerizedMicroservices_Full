namespace Orders.UnitTest.Domain;

public class OrderAggregateTest
{
    [Fact]
    public void CreateOrderItem_Success()
    {
        // Arrange
        int productId = 1;
        string productName = "Product Name";
        decimal unitPrice = 12.55m;
        decimal discount = 5;
        string pictureUrl = "url@picture.png.com";
        int units = 2;

        // Action
        var fakeOrderItemWithUnits = new OrderItem(productId, productName, unitPrice, discount, pictureUrl, units);
        var fakeOrderItemWithDefaultUnits = new OrderItem(productId, productName, unitPrice, discount, pictureUrl);

        // Assertion

        Assert.NotNull(fakeOrderItemWithUnits);
        Assert.NotNull(fakeOrderItemWithDefaultUnits);
    }

    [Fact]
    public void InvalideNumberOfUnits_OrderItems_ShouldThrowOrderDomainException()
    {
        // Arrange
        int productId = 1;
        string productName = "Product Name";
        decimal unitPrice = 12.55m;
        decimal discount = 5;
        string pictureUrl = "url@picture.png.com";
        int units = -1;

        // Action - Assert
        Assert.Throws<OrderDomainException>(() => new OrderItem(productId,productName,unitPrice, discount, pictureUrl,units));
    }

    [Fact]
    public void InvalidTotalOfOrderItemLowerThanDiscountApplied_ShouldThrowOrderDomainException()
    {
        // Arrange
        int productId = 1;
        string productName = "Product Name";
        decimal unitPrice = 12.55m;
        decimal discount = 15;
        string pictureUrl = "url@picture.png.com";
        int units = 1;

        // Action - Assertion
        Assert.Throws<OrderDomainException>(() => new OrderItem(productId,productName,unitPrice,discount, pictureUrl,units));
    }

    [Fact]
    public void InvalidDiscountSettings_OrderItem_ShouldThrowOrderDomainException()
    {
        // Arrange
        int productId = 1;
        string productName = "Product Name";
        decimal unitPrice = 12.55m;
        decimal discount = 5;
        string pictureUrl = "url@picture.png.com";
        int units = 2;

        // Action
        var fakeOrderItem = new OrderItem(productId, productName, unitPrice, discount, pictureUrl, units);

        // Assertion
        Assert.Throws<OrderDomainException>(() => fakeOrderItem.SetNewDiscount(-1));
    }

    [Fact]
    public void InvalidUnitsSetting_OrderItem_ShouldThrowOrderDomainException()
    {
        // Arrange
        int productId = 1;
        string productName = "Product Name";
        decimal unitPrice = 12.55m;
        decimal discount = 5;
        string pictureUrl = "url@picture.png.com";
        int units = 2;

        // Action
        var fakeOrderItem = new OrderItem(productId, productName, unitPrice, discount, pictureUrl, units);

        // Assert
        Assert.Throws<OrderDomainException>(() => fakeOrderItem.AddUnits(-1));
    }

    [Fact]
    public void WhenAddTwoItemsOnTheSameItemThenTheTotalOfOrderShouldBeTheSumOfTheTwoItems()
    {
        var address = new AddressBuilder().Build();
        var order = new OrderFakeBuilder(address)
                            .AddOneItem(1, "t shirt", 12.55m, 0, "pic.jpg")
                            .AddOneItem(1, "t shirt", 12.55m, 0, "pic.jpg")
                            .Build();

        Assert.Equal(25.10m, order.GetTotal());
    }

    [Fact]
    public void AddNewOrder_RiseNewEvent()
    {
        // Arrange
        int expectedResult = 1;

        // Arrange
        var address= new AddressBuilder().Build();
        var fakeOrder = new OrderFakeBuilder(address).AddOneItem(1, "cup", 12m, 2, "pic.png",2)
                        .Build();

        // Assert
        Assert.Equal(expectedResult, fakeOrder.DomainEvents.Count);
    }

    [Fact]
    public void AddNewOrder_ExplicitlyRiseNewEvent()
    {
        // Arrange
        int expectedResult = 2;

        // Arrange
        var address = new AddressBuilder().Build();
        var fakeOrder = new OrderFakeBuilder(address).AddOneItem(1, "cup", 12m, 2, "pic.png", 2)
                        .Build();
        fakeOrder.AddDomainEvent(new OrderStartedDomainEvents(fakeOrder, "fakeName", "1", 5, "12", "123", "fakeHolderName", DateTime.Now.AddYears(1)));

        // Assert
        Assert.Equal(expectedResult, fakeOrder.DomainEvents.Count);
    }

    [Fact]
    public void RemoveEvent_Explicitly()
    {
        // Arrange
        int expectedResult = 1;
        int shouldNotBeResult = 2;

        // Arrange
        var address = new AddressBuilder().Build();
        var fakeOrder = new OrderFakeBuilder(address).AddOneItem(1, "cup", 12m, 2, "pic.png", 2)
                        .Build();

        var @fakeEvent = new OrderStartedDomainEvents(fakeOrder, "fakeName", "1", 5, "12", "123", "fakeHolderName", DateTime.Now.AddYears(1));
        fakeOrder.AddDomainEvent(fakeEvent);
        fakeOrder.RemoveDomainEvent(fakeEvent);

        // Assert
        Assert.Equal(expectedResult, fakeOrder.DomainEvents.Count);
        Assert.NotEqual(expectedResult, shouldNotBeResult);
    }
}
