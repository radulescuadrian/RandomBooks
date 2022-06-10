using RandomBooks.Shared.DatabaseModels;

namespace RandomBooks.Shared.DTOs;

public class OrderRequest
{
    public int? UserId { get; set; }
    public Address? Address { get; set; }
    public CustomerCard? Card { get; set; }
    public string? Notes { get; set; }
}
