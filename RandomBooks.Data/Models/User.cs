namespace RandomBooks.Data.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public byte[] Password { get; set; }
    public DateTime DateCreated { get; set; } = DateTime.Now;
    public string Role { get; set; } = "Customer";

    public List<Address> Addresses { get; set; } = new List<Address>();
    public List<User_Card> Cards { get; set; } = new List<User_Card>();
}
