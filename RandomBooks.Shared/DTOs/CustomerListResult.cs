namespace RandomBooks.Shared.DTOs;

public class CustomerListResult
{
    public List<CustomersResponse> Customers { get; set; }
    public int Page { get; set; } = 1;
    public int Pages { get; set; } = 0;
}
