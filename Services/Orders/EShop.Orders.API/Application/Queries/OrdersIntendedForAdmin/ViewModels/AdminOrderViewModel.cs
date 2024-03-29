﻿namespace EShop.Orders.API.Application.Queries.OrdersIntendedForAdmin.ViewModels;

public class AdminOrderViewModel
{
    private string _status;

    public int OrderNumber { get; set; }
    public DateTime OrderDate { get; set; }
    public string Description { get; set; }
    public string Country { get; set; }
    public string State { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public string ZipCode { get; set; }

    public string Status 
    { 
        get => this._status; 
        set { this._status = value.EditOrderStatusName(); } 
    }

    public string BuyerName { get; set; }
    public double TotalPrice { get; set; }
    public int TotalProducts { get; set; }
    public double MaximumPrice { get; set; }
    public double MinimumPrice { get; set; }
    public double AveragePrice { get; set; }

    public List<OrderItemViewModels> OrderItems { get; set; }
        = new List<OrderItemViewModels>();
}