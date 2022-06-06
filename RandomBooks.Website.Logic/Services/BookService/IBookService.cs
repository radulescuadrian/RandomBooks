namespace RandomBooks.Website.Logic.Services.BookService
{
    public interface IBookService
    {
        List<Book> AdminBooks { get; set; }
        List<Author> Authors { get; set; }
        List<BookType> BookTypes { get; set; }
        List<Language> Languages { get; set; }
        string Message { get; set; }
        List<Category> Categories { get; set; }
        List<Publisher> Publishers { get; set; }

        event Action OnChange;

        Task<Book> AddBook(Book book, string image);
        Task GetAdminBooks();
        Task<ServiceResponse<Book>> GetBook(int bookId);
        Task GetDropdowns();
        Task<Book> UpdateBook(Book book, string image);
    }
}