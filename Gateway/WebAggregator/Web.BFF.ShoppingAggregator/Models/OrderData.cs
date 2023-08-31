namespace Web.BFF.ShoppingAggregator.Models;

public class OrderData
{
    public string OrderNumber { get; set; } = string.Empty;

    public DateTime Date { get; set; }

    public string Status { get; set; } = string.Empty;

    public decimal Total { get; set; }

    public string Description { get; set; } = string.Empty;

    public string? City { get; set; }

    public string? Street { get; set; }

    public string? State { get; set; }

    public string? Country { get; set; }

    public string? ZipCode { get; set; }

    public string CardNumber { get; set; } = string.Empty;

    public string CardHolderName { get; set; } = string.Empty;

    public bool IsDraft { get; set; }

    public DateTime CardExpiration { get; set; }

    public string CardExpirationShort { get; set; } = string.Empty;

    public string CardSecurityNumber { get; set; } = string.Empty;

    public int CardTypeId { get; set; }

    public string Buyer { get; set; } = string.Empty;

    public List<OrderDataItems> OrderItems { get; set; } = new();
}