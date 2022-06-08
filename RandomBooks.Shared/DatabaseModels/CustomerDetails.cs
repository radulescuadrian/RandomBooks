namespace RandomBooks.Shared.DatabaseModels;

public class CustomerDetails
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public DateTime JoinDate { get; set; } = DateTime.Now;
    public Blob? Image { get; set; }

    public List<Address> Addresses { get; set; } = new List<Address>();
    public List<CustomerCard> Cards { get; set; } = new List<CustomerCard>();
}
