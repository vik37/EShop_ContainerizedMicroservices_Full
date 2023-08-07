namespace Orders.UnitTest.Domain;

public class BuyerAggregateTest
{
    [Fact]
    public void CreateBuyerItem_Success()
    {
        // Arrange
        var identity = Guid.NewGuid().ToString();
        var name = "Fake UserName";

        // Action
        var fakeBuyerItem = new Buyer(identity, name);

        // Assertion
        Assert.NotNull(fakeBuyerItem);
    }

    [Fact]
    public void CreateBuyerItem_Fail_ShouldThrowArgumentNullException()
    {
        // Arrange
        var identity = string.Empty;
        var name = "Fake UserName";

        // Action - Assert
        Assert.Throws<ArgumentNullException>(() => new Buyer(identity,name));
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
        Assert.NotNull(result);
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
        var fakePaymentMethod = new PaymentMethod(cardTypeId, alias, cardNumber, securityNumber, cardHolderName, expirationDate);


        // Action
        var result = new PaymentMethod(cardTypeId, alias, cardNumber, securityNumber, cardHolderName, expirationDate);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void CreatePaymentMethod_Failure_ShouldThrowOrderDomainException()
    {
        // Arrange
        int cardTypeId = 1;
        string alias = "FakeAlias";
        string cardNumber = "123";
        string securityNumber = "1234";
        string cardHolderName = "FakeHolderName";
        DateTime expirationDate = DateTime.Now.AddYears(-1);

        // Action - Assert
        Assert.Throws<OrderDomainException>(() => new PaymentMethod(cardTypeId, alias, cardNumber, securityNumber, cardHolderName, expirationDate));
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
        Assert.True(result);
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
        Assert.Equal(expectedResult,fakeBuyer.DomainEvents.Count);
    }
}