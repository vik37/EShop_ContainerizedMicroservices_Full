﻿namespace EShop.Orders.API.Application.Models;

public class BasketItems
{
    public string Id { get; set; } = string.Empty;
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public decimal OldUnitPrice { get; set; }
    public int Quantity { get; set; }
    public string? PictureUrl { get; set; }
}
