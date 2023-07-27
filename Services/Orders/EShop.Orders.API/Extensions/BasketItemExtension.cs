using EShop.Orders.API.Application.Models;
using static EShop.Orders.API.Application.Commands.CreateOrderCommand;

namespace EShop.Orders.API.Extensions;

public static class BasketItemExtension
{

    public static IEnumerable<OrderItemDto> ToOrderItemsDto(this IEnumerable<BasketItems> items)
    {
        foreach (var item in items)
        {
            yield return item.ToOrderItemDto();
        }
    }

    public static OrderItemDto ToOrderItemDto(this BasketItems basketItems)
        => new OrderItemDto
        {
            ProductId = basketItems.ProductId,
            ProductName = basketItems.ProductName,
            UnitPrice = basketItems.UnitPrice,
            Units = basketItems.Quantity,
            PictureUrl = basketItems.PictureUrl??string.Empty
        };
}
