namespace EShop.Orders.Domain.AggregatesModel.OrderAggregate;

public class Address : ValueObject
{
    public String Street { get; private set; } = string.Empty;
    public String City { get; private set; } = string.Empty;
    public String State { get; private set; } = string.Empty;
    public String Country { get; private set; } = string.Empty; 
    public String ZipCode { get; private set; } = string.Empty;

    public Address() { }


    public Address(string street, string city, string state, string country, string zipCode) 
    {
        Street = street;
        City = city;
        State = state;
        Country = country;
        ZipCode = zipCode;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Street;
        yield return City;
        yield return State;
        yield return Country;
        yield return ZipCode;
    }
}
