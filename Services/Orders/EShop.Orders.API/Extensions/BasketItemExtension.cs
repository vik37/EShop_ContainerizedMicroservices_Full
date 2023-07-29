namespace EShop.Orders.API.Extensions;

public static class BasketItemExtension
{

    public static IEnumerable<OrderItemDto> ToOrderItemsDto(this IEnumerable<BasketItem> items)
    {
        foreach (var item in items)
        {
            yield return item.ToOrderItemDto();
        }
    }

    public static OrderItemDto ToOrderItemDto(this BasketItem basketItems)
        => new OrderItemDto
        {
            ProductId = basketItems.ProductId,
            ProductName = basketItems.ProductName,
            UnitPrice = basketItems.UnitPrice,
            Units = basketItems.Quantity,
            PictureUrl = basketItems.PictureUrl??string.Empty
        };
}
