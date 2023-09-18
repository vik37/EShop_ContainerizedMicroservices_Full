namespace EShop.Web.MVC.Services.ModelDTOs;

public class OrderCheckoutDto
{
    public string UserId { get;  set; } = string.Empty;

    public string UserName { get; set; } = string.Empty;

    public string City { get; set; }

    public string Street { get;  set; }

    public string State { get; set; }

    public string Country { get; set; }

    public string ZipCode { get; set; }

    public string CardNumber { get; set; } = string.Empty;

    public string CardHolderName { get; set; } = string.Empty;

    public DateTime CardExpiration { get; set; }

    public string CardSecurityNumber { get; set; } = string.Empty;

    public int CardTypeId { get; set; }

    public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}