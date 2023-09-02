namespace EShop.Web.MVC.Models;

public class Order
{
    [JsonConverter(typeof(NumberToStringConverter))]
    public string OrderNumber { get; set; }

    public DateTime Date { get; set; }

    public string Status { get; set; }

    public decimal Total { get; set; }

    public string Description { get; set; }

    [Required]
    public string City { get; set; }
    [Required]
    public string Street { get; set; }
    [Required]
    public string State { get; set; }
    [Required]
    public string Country { get; set; }

    [Required]
    public string ZipCode { get; set; }
    [Required]
    [DisplayName("Card Number")]
    public string CardNumber { get; set; }
    [Required]
    [DisplayName("Cardholder Name")]
    public string CardHolderName { get; set; }

    public DateTime CardExpirationDate { get; set; }

    [RegularExpression(@"(0[1-9]|1[0-2])\/[0-9]{2}", ErrorMessage = "Expiration should match a valid MM/YY value")]
    [CardExpirationValidation(ErrorMessage = "The Card is Expired"), Required]
    [DisplayName("Card Expiration")]
    public string CardExpirationShort { get; set; }

    [Required]
    [DisplayName("Card Security Number")]
    public string CardSecurityNumber { get; set; }

    public int CardTypeId { get; set; }

    public string Buyer { get; set; }

    public List<SelectListItem> ActionCodeSelect =>
        GetActionCodesByCurrentState();

    public List<OrderItem> OrderItems { get; set; }

    public Guid RequestId { get; set; }

    public void CardExpirationShortFormat()
    {
        CardExpirationShort = CardExpirationDate.ToString("mm/yy");
    }

    public void CardExpirationAPIFormat()
    {
        var month = CardExpirationShort.Split('/')[0];
        var year = $"20{CardExpirationShort.Split("/")[1]}";

        CardExpirationDate = new DateTime(int.Parse(year),int.Parse(month),1);
    }

    private List<SelectListItem> GetActionCodesByCurrentState()
    {
        var actions = new List<OrderProcessAction>();
        switch(Status.ToLower())
        {
            case "Paid":
                actions.Add(OrderProcessAction.Ship);
                break;
        }

        var result = new List<SelectListItem>();
        actions.ForEach(a =>
        {
            result.Add(new SelectListItem { Text = a.Name, Value = a.Code });
        });

        return result;
    }
}