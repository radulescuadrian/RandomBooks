namespace RandomBooks.API.Services.BookService
{
    public interface IBookService
    {
        Task<ServiceResponse<Book>> AddBook(BookEdit edit);
        Task<ServiceResponse<BookListResult>> GetAdminBooks(int page);
        Task<ServiceResponse<List<Author>>> GetAuthors();
        Task<ServiceResponse<Book>> GetBook(int bookId);
        Task<ServiceResponse<BookListResult>> GetBooks(int page);
        Task<ServiceResponse<BookListResult>> GetBooksByCategory(string category, int page);
        Task<ServiceResponse<List<string>>> GetBookSearchSuggestions(string searchText);
        Task<ServiceResponse<List<BookType>>> GetBookTypes();
        Task<ServiceResponse<BookListResult>> GetFeaturedBooks(int page);
        Task<ServiceResponse<List<Language>>> GetLanguages();
        Task<ServiceResponse<BookListResult>> SearchBooks(string searchText, int page);
        Task<ServiceResponse<Book>> UpdateBook(BookEdit edit);
    }
}