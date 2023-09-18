namespace EShop.Orders.Domain.AggregatesModel.BuyerAggregate;

public class Buyer: Entity, IAggregateRoot
{
    public string? IdentityGuid { get; private set; }

    public string? Name { get; private set; }

    private List<PaymentMethod> _paymentMethods;

    public IEnumerable<PaymentMethod> PaymentMethods => _paymentMethods;

    protected Buyer()
    {
        _paymentMethods = new List<PaymentMethod>();
    }

    public Buyer(string identity, string name) : this()
    {
        IdentityGuid = !string.IsNullOrEmpty(identity) ? identity : throw new ArgumentNullException(nameof(identity));
        Name = !string.IsNullOrEmpty(name) ? name : throw new ArgumentNullException(nameof(name));
    }

    public PaymentMethod VerifyOrAddPayment(int cardTypeId, string alias, string cardNumber, string securityNumber, string cardHolderName,
                                            DateTime expiration, int orderId)
    {
        var existPayment = _paymentMethods.SingleOrDefault(x => x.IsEqualTo(cardTypeId, cardNumber, expiration));

        if(existPayment is not null)
        {
            AddDomainEvent(new BuyerAndPeymentMethodVerifiedDomainEvent(this,existPayment,orderId));
            return existPayment;
        }

        var payment = new PaymentMethod(cardTypeId,alias,cardNumber,securityNumber,cardHolderName,expiration);
        _paymentMethods.Add(payment);
        AddDomainEvent(new BuyerAndPeymentMethodVerifiedDomainEvent(this,payment,orderId));
        return payment;
    }
}
