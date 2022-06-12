namespace RandomBooks.Shared.DTOs;

public class CourierListResult
{
    public List<CouriersResponse> Couriers  { get; set; }
    public int Page { get; set; } = 1;
    public int Pages { get; set; } = 0;
}
