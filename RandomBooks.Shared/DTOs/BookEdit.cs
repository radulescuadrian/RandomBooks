using RandomBooks.Shared.DatabaseModels;

namespace RandomBooks.Shared.DTOs;

public class BookEdit
{
    public Book Book { get; set; } = new();
    public string NewImage { get; set; } = string.Empty;
}
