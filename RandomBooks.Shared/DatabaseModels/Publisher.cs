namespace RandomBooks.Shared.DatabaseModels;

public class Publisher
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public bool Deleted { get; set; } = false;
}
