namespace RandomBooks.Shared.DTOs;

public class CouriersResponse
{
    public int Id { get; set; } = 0;
    public string Username { get; set; } = string.Empty;
    public int OrdersDelivered { get; set; } = 0;
    public double Rating { get; set; } = 0;
    public DateTime DateHired { get; set; } = DateTime.Now;
    public bool Deactivated { get; set; } = false;
}
