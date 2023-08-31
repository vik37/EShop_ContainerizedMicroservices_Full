namespace EShop.Orders.API.Application.Models.Dto;

public record OrderDraftDto
{
    public IEnumerable<OrderItemDto>? OrderItems { get; init; }
    public decimal Total { get; set; }

    public static OrderDraftDto FromOrder(Order order)
        => new()
        {
            OrderItems = order.OrderItems.Select(oi => new OrderItemDto
            {
                Discount = oi.GetCurrentDiscount(),
                ProductId = oi.ProductId,
                UnitPrice = oi.GetUnitPrice(),
                PictureUrl = oi.GetPictureUrl,
                Units = oi.GetUnits(),
                ProductName = oi.GetOrderItemProductName()
            }),
            Total = order.GetTotal()
        };
}