namespace Orders.UnitTest;

public class OrderFakeBuilder
{
    private readonly Order order;

    public OrderFakeBuilder(Address address)
    {
        order = new Order(
                userId: "userId",
                userName: "user name",
                address,
                cardTypeId: 5,
                cardNumber: "18",
                cardSecurityNumber: "123",
                cardHolderName: "holder name",
                cardExpirationDate: DateTime.UtcNow
            );
    }

    public OrderFakeBuilder AddOneItem(
            int productId, string productName,
            decimal unitPrice, decimal discount,
            string prictureUrl, int units = 1
        )
    {
        order.AddOrderItem(productId,productName, unitPrice, discount, prictureUrl, units);
        return this;
    }

    public Order Build() => order;
}

public class AddressBuilder
{
    public Address Build()
        => new Address("street", "city", "state", "country", "zipcode");
}