using EShop.Order.Domain.Exceptions;
using EShop.Order.Domain.SeedWork;

namespace EShop.Order.Domain.AggregatesModel.BuyerAggregate;

public class PaymentMethod : Entity
{
    private string? _alias;
    private string? _cardNumber;
    private string? _securityNumber;
    private string? _cardHolderName;
    private DateTime _expiration;

    private int _cardTypeId;

    public int CardTypeId { get; private set; }

    protected PaymentMethod() { }

    public PaymentMethod(int cardTypeId, string alias, string cardNumber, string securityNumber, string cardHolderName, DateTime expiration)
    {
        _cardNumber = !string.IsNullOrEmpty(cardNumber) ? cardNumber :throw new ArgumentNullException(nameof(cardNumber));
        _cardHolderName = !string.IsNullOrEmpty(cardHolderName) ? cardHolderName : throw new ArgumentNullException(nameof(cardHolderName));
        _securityNumber = !string.IsNullOrEmpty(securityNumber) ? securityNumber :throw new ArgumentNullException(nameof(securityNumber));

        if(expiration < DateTime.UtcNow)
            throw new OrderDomainException(nameof(expiration));

        _alias = alias;
        _expiration = expiration;
        _cardTypeId = cardTypeId;
    }

    public bool IsEqualTo(int cardTypeId, string cardNumber, DateTime expiration)
        => _cardTypeId == cardTypeId && _cardNumber == cardNumber && _expiration == expiration;
}
