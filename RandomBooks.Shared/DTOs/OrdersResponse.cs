﻿using System.ComponentModel.DataAnnotations.Schema;

namespace RandomBooks.Shared.DTOs;

public class OrdersResponse
{
    public int Id { get; set; }
    public DateTime DatePlaced { get; set; } = DateTime.Now;
    public DateTime LastModified { get; set; } = DateTime.Now;
    public string? Notes { get; set; }
    public int? Rating { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Total { get; set; } = 0;
    public OrderAddress Address { get; set; } = new OrderAddress();
    public List<OrderBook> Books { get; set; } = new List<OrderBook>();
    public string Payment { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string ItemsText { get; set; } = string.Empty;
}

public class OrderAddress
{
    public string Country { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public string Zip { get; set; } = string.Empty;
}

public class OrderBook
{
    public string Title { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public int Quantity { get; set; } = 1;
    public decimal Price { get; set; } = 0;
}