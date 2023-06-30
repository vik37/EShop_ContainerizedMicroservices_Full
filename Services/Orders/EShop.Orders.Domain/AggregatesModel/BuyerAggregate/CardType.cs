namespace EShop.Orders.Domain.AggregatesModel.BuyerAggregate;

public class CardType : Enumeration
{
    public static CardType Amex = new CardType(1,nameof(Amex));
    public static CardType Visa = new CardType(2, name:nameof(Visa));
    public static CardType MasterCard = new CardType(3, name:nameof(MasterCard));

    public CardType(int id, string name) : base(id, name)
    { }
}
