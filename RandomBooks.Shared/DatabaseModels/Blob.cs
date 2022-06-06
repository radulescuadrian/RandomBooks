namespace RandomBooks.Shared.DatabaseModels;

public class Blob
{
    public int Id { get; set; }
    public string Data { get; set; }
    public string Type { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime ModifiedAt { get; set; } = DateTime.Now;
}
