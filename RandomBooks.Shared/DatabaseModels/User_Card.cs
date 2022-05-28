namespace RandomBooks.Shared.DatabaseModels;

public class User_Card
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Last4Numbers { get; set; } = string.Empty;
    public string Number { get; set; }
    public int ExpirationMonth { get; set; }
    public int ExpirationYear { get; set; }
}
