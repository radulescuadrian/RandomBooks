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
        int Page { get; set; }
        int PageCount { get; set; }
        List<Book> Books { get; set; }

        event Action OnChange;

        Task<Book> AddBook(Book book, string image);
        Task GetAdminBooks(bool all = false);
        Task<ServiceResponse<Book>> GetBook(int bookId);
        Task GetBooks(string category = null);
        Task GetDropdowns();
        Task GetFeaturedBooks();
        void InitializePages();
        Task<Book> UpdateBook(Book book, string image);
    }
}