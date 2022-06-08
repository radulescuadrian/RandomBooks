namespace RandomBooks.Shared.DatabaseModels;

public class Address
{
    public int Id { get; set; }
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Zip { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;

    public bool Deleted { get; set; } = false;
}
