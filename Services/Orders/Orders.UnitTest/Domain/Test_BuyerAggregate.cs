namespace Orders.UnitTest.Domain;

public class Test_BuyerAggregate
{

    [Fact]
    public void HandleThrowException_WhenThereIsNotBuyerId()
    {
        // Arrange
        string identity = string.Empty;
        string name = "Fake Name";

        // Action
        Action action = () => new Buyer(identity, name);

        // Asssert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(identity));
    }

    [Fact]
    public void HandleThrowException_WhenThereIsNotBuyerName()
    {
        // Arrange
        string identity = Guid.NewGuid().ToString();
        string name = string.Empty;

        // Action
        Action action = () => new Buyer(identity, name);

        // Asssert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(name));
    }

    [Fact]
    public void CreateBuyerItem_Success()
    {
        // Arrange
        var identity = Guid.NewGuid().ToString();
        var name = "Fake UserName";

        // Action
        var fakeBuyerItem = new Buyer(identity, name);

        // Assertion
        fakeBuyerItem.Should().NotBeNull();
    }

    [Fact]
    public void CreateBuyerItem_Fail_ShouldThrowArgumentNullException()
    {
        // Arrange
        var identity = string.Empty;
        var name = "Fake UserName";

        // Action 
        Action action = () => new Buyer(identity,name);

        // Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void AddPayment_Success()
    {
        // Arrange
        int cardTypedId = 1;
        string alias = "FakeAlias";
        string cardNumber = "123";
        string securityNumber = "1234";
        string cardHolderName = "FakeHolderName";
        DateTime expirationDate = DateTime.Now.AddYears(1);
        int orderId = 1;
        string name = "Fake UserName";
        string identity = Guid.NewGuid().ToString();
        var fakeBuyer = new Buyer(identity, name);

        // Action
        var result = fakeBuyer.VerifyOrAddPayment(cardTypedId, alias, cardNumber, securityNumber,
            cardHolderName, expirationDate, orderId);

        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public void CreatePaymentMethod_Success()
    {
        // Arrange
        int cardTypeId = 1;
        string alias = "FakeAlias";
        string cardNumber = "123";
        string securityNumber = "1234";
        string cardHolderName = "FakeHolderName";
        DateTime expirationDate = DateTime.Now.AddYears(1);

        // Action
        var result = new PaymentMethod(cardTypeId, alias, cardNumber, securityNumber, cardHolderName, expirationDate);

        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public void CreatePaymentMethod_FailureExpirationDate_ShouldThrowOrderDomainException()
    {
        // Arrange
        int cardTypeId = 1;
        string alias = "FakeAlias";
        string cardNumber = "123";
        string securityNumber = "1234";
        string cardHolderName = "FakeHolderName";
        DateTime expiration = DateTime.Now.AddYears(-1);

        // Action
        Action action = () => new PaymentMethod(cardTypeId, alias, cardNumber, securityNumber, cardHolderName, expiration);

        // Assert
        action.Should().ThrowExactly<OrderDomainException>().WithMessage(nameof(expiration));
    }

    [Fact]
    public void PaymentMethhod_IsEqualTo()
    {
        // Arrange
        int cardTypeId = 1;
        string alias = "FakeAlias";
        string cardNumber = "123";
        string securityNumber = "1234";
        string cardHolderName = "FakeHolderName";
        DateTime expirationDate = DateTime.Now.AddYears(1);

        //Action
        var fakePaymentMethod = new PaymentMethod(cardTypeId, alias, cardNumber, securityNumber, cardHolderName, expirationDate);
        var result = fakePaymentMethod.IsEqualTo(cardTypeId,cardNumber,expirationDate);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void AddNewPaymentMethod_RaiseNewEvent()
    {
        // Arrange
        int cardTypeId = 5;
        string alias = "FakeAlias";
        string cardNumber = "123";
        string securityNumber = "1234";
        string cardHolderName = "FakeHolderName";
        DateTime expirationDate = DateTime.Now.AddYears(1);
        int orderId = 1;
        int expectedResult = 1;
        string name = "Fake UserName";

        // Action
        var fakeBuyer = new Buyer(Guid.NewGuid().ToString(), name);
        fakeBuyer.VerifyOrAddPayment(cardTypeId, alias, cardNumber, securityNumber, cardHolderName, expirationDate, orderId);

        // Assert
        fakeBuyer.DomainEvents.Count.Should().Be(expectedResult);
    }
}