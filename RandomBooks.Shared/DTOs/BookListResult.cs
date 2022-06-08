using RandomBooks.Shared.DatabaseModels;

namespace RandomBooks.Shared.DTOs;

public class BookListResult
{
    public List<Book> Books { get; set; }
    public int Page { get; set; }
    public int Pages { get; set; }
}
