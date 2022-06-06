using RandomBooks.Shared.DatabaseModels;

namespace RandomBooks.Shared.DTOs;

public class AuthorEdit
{
    public Author Author { get; set; } = new Author();
    public string NewImage { get; set; } = string.Empty;
}
