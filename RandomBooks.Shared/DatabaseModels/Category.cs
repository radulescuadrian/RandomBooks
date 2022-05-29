using System.ComponentModel.DataAnnotations.Schema;

namespace RandomBooks.Shared.DatabaseModels;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public bool Deleted { get; set; } = false;

    [NotMapped]
    public bool Editing { get; set; } = false;
    [NotMapped]
    public bool New { get; set; } = false;
}
