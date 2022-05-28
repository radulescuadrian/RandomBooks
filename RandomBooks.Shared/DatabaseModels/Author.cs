using System.ComponentModel.DataAnnotations;

namespace RandomBooks.Shared.DatabaseModels;

public class Author
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public bool Deleted { get; set; } = false;
}
