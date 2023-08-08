namespace Orders.UnitTest.Domain;

public class Test_OrderAggregate
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
        fakeOrderItemWithUnits.Should().NotBeNull();
        fakeOrderItemWithDefaultUnits.Should().NotBeNull();
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

        // Action
        Action action = () => new OrderItem(productId, productName, unitPrice, discount, pictureUrl, units);

        // Assert
        action.Should().Throw<OrderDomainException>().WithMessage("Invalid number of units");
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

        // Action
        Action action = () => new OrderItem(productId, productName, unitPrice, discount, pictureUrl, units);

        // Assertion
        action.Should().Throw<OrderDomainException>().WithMessage("The total oforder items is lower than applied discount");
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
        fakeOrderItem.Invoking(x => x.SetNewDiscount(-1)).Should().Throw<OrderDomainException>().WithMessage("Invalid Discount");
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
        fakeOrderItem.Invoking(x => x.AddUnits(-1)).Should().Throw<OrderDomainException>().WithMessage("Invalid Units");
    }

    [Fact]
    public void WhenAddTwoItemsOnTheSameItemThenTheTotalOfOrderShouldBeTheSumOfTheTwoItems()
    {
        var address = new AddressBuilder().Build();
        var order = new OrderFakeBuilder(address)
                            .AddOneItem(1, "t shirt", 12.55m, 0, "pic.jpg")
                            .AddOneItem(1, "t shirt", 12.55m, 0, "pic.jpg")
                            .Build();

        order.GetTotal().Should().Be(25.10m);
    }

    [Fact]
    public void AddNewOrder_RiseNewEvent()
    {
        // Arrange
        int expectedResult = 1;

        // Action
        var address= new AddressBuilder().Build();
        var fakeOrder = new OrderFakeBuilder(address).AddOneItem(1, "cup", 12m, 2, "pic.png",2)
                        .Build();

        // Assert
        fakeOrder.DomainEvents.Count.Should().Be(expectedResult);
    }

    [Fact]
    public void AddNewOrder_ExplicitlyRiseNewEvent()
    {
        // Arrange
        int expectedResult = 2;

        // Action
        var address = new AddressBuilder().Build();
        var fakeOrder = new OrderFakeBuilder(address).AddOneItem(1, "cup", 12m, 2, "pic.png", 2)
                        .Build();
        fakeOrder.AddDomainEvent(new OrderStartedDomainEvents(fakeOrder, "fakeName", "1", 5, "12", "123", "fakeHolderName", DateTime.Now.AddYears(1)));

        // Assert
        fakeOrder.DomainEvents.Count.Should().Be(expectedResult);
    }

    [Fact]
    public void RemoveEvent_Explicitly()
    {
        // Arrange
        int expectedResult = 1;
        int shouldNotBeResult = 2;

        // Action
        var address = new AddressBuilder().Build();
        var fakeOrder = new OrderFakeBuilder(address).AddOneItem(1, "cup", 12m, 2, "pic.png", 2)
                        .Build();

        var @fakeEvent = new OrderStartedDomainEvents(fakeOrder, "fakeName", "1", 5, "12", "123", "fakeHolderName", DateTime.Now.AddYears(1));
        fakeOrder.AddDomainEvent(@fakeEvent);
        fakeOrder.RemoveDomainEvent(@fakeEvent);

        // Assert
        fakeOrder.DomainEvents.Count.Should().Be(expectedResult);
        fakeOrder.DomainEvents.Count.Should().NotBe(shouldNotBeResult);
    }
}
