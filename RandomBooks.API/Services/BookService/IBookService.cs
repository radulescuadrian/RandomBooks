namespace RandomBooks.API.Services.BookService
{
    public interface IBookService
    {
        Task<ServiceResponse<Book>> AddBook(BookEdit edit);
        Task<ServiceResponse<BookListResult>> GetAdminBooks(int page);
        Task<ServiceResponse<List<Author>>> GetAuthors();
        Task<ServiceResponse<Book>> GetBook(int bookId);
        Task<ServiceResponse<List<Book>>> GetBooks();
        Task<ServiceResponse<List<BookType>>> GetBookTypes();
        Task<ServiceResponse<BookListResult>> GetFeaturedBooks(int page);
        Task<ServiceResponse<List<Language>>> GetLanguages();
        Task<ServiceResponse<Book>> UpdateBook(BookEdit edit);
    }
}