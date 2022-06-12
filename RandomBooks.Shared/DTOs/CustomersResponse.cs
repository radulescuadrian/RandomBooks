namespace RandomBooks.Shared.DTOs;

public class CustomersResponse
{
    public int Id { get; set; } = 0;
    public string Username { get; set; } = string.Empty;
    public int OrdersPlaced { get; set; } = 0;
    public DateTime DateJoined { get; set; } = DateTime.Now;
    public bool Deactivated { get; set; } = false;
}
