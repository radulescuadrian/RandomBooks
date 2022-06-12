namespace RandomBooks.Shared.DTOs;

public class OrderListResult
{
    public List<OrdersResponse> Orders { get; set; }
    public int Page { get; set; }
    public int Pages { get; set; }
}