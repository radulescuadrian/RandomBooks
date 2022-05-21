namespace RandomBooks.Data.Models;

public class User_Card
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; }
    public string Number { get; set; }
    public int ExpirationMonth { get; set; }
    public int ExpirationYear { get; set; }
    public int CVC { get; set; }
}
