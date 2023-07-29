namespace EShop.Orders.API.Application.Models.Dto;

public record OrderItemDto
{
    public int ProductId { get; init; }
    public string ProductName { get; init; } = string.Empty;
    public decimal UnitPrice { get; init; }
    public decimal Discount { get; init; }
    public int Units { get; init; }
    public string PictureUrl { get; init; } = string.Empty;
}
