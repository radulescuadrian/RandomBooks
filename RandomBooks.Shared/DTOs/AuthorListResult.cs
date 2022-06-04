using RandomBooks.Shared.DatabaseModels;

namespace RandomBooks.Shared.DTOs;

public class AuthorListResult
{
    public List<Author> Authors { get; set; }
    public int Page { get; set; }
    public int Pages { get; set; }
}
