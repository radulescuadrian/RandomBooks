using RandomBooks.Shared.DatabaseModels;

namespace RandomBooks.Shared.DTOs;

public class CustomerDetailsResponse
{
    public string Username { get; set; } = string.Empty;
    public string Firstname { get; set; } = string.Empty;
    public string Lastname { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string? Image { get; set; }
    public List<Address> Addresses { get; set; } = new List<Address>();
    public int NumberOfCards { get; set; } = 0;
    public DateTime DateJoined { get; set; } = DateTime.Now;
    public bool Deactivated { get; set; } = false;
    public List<OrdersResponse> Orders { get; set; } = new List<OrdersResponse>();
}
