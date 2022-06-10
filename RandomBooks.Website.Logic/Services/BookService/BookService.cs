using RandomBooks.Website.Logic.Services.AuthorService;
using RandomBooks.Website.Logic.Services.CategoryService;
using RandomBooks.Website.Logic.Services.LanguageService;
using RandomBooks.Website.Logic.Services.PublisherService;

namespace RandomBooks.Website.Logic.Services.BookService;

public class BookService : IBookService
{
    #region Dropdowns
    public List<BookType> BookTypes { get; set; } = new();
    public List<Author> Authors { get; set; } = new();
    public List<Language> Languages { get; set; } = new();
    public List<Category> Categories { get; set; } = new();
    public List<Publisher> Publishers { get; set; } = new();
    #endregion

    public List<Book> AdminBooks { get; set; }
    public List<Book> Books { get; set; }

    private readonly HttpClient _http;
    private readonly IAuthorService _authorService;
    private readonly ICategoryService _categoryService;
    private readonly IPublisherService _publisherService;
    private readonly ILanguageService _languageService;

    public event Action OnChange;

    public string Message { get; set; } = "Loading books...";
    public int Page { get; set; } = 1;
    public int PageCount { get; set; } = 0;
    public string LastSearchText { get; set; } = string.Empty;

    public BookService(HttpClient http, IAuthorService authorService, ICategoryService categoryService, IPublisherService publisherService, ILanguageService languageService)
    {
        _http = http;
        _authorService = authorService;
        _categoryService = categoryService;
        _publisherService = publisherService;
        _languageService = languageService;
    }

    public async Task GetDropdowns()
    {
        BookTypes = (await _http.GetFromJsonAsync<ServiceResponse<List<BookType>>>($"https://localhost:7163/api/book/types")).Data;
        Authors = await _authorService.GetVisibleAuthorsList();
        Languages = await _languageService.GetVisibleLanguagesList();
        Categories = await _categoryService.GetVisibleCategoriesList();
        Publishers = await _publisherService.GetVisiblePublishersList();
    }

    public void InitializePages()
    {
        if (Books != null)
            Books.Clear();
        Message = "Loading books...";
        Page = 1;
        PageCount = 0;
    }

    public async Task<ServiceResponse<Book>> GetBook(int bookId)
    {
        var response = await _http.GetFromJsonAsync<ServiceResponse<Book>>($"https://localhost:7163/api/book/{bookId}");
        return response;
    }

    public async Task GetAdminBooks(bool all = false)
    {
        ServiceResponse<BookListResult> response;

        if (all)
        {
            response = await _http.GetFromJsonAsync<ServiceResponse<BookListResult>>($"https://localhost:7163/api/book/admin/0");
            if (response != null && response.Data != null)
                AdminBooks = response.Data.Books;
        }
        else 
        {
            response = await _http.GetFromJsonAsync<ServiceResponse<BookListResult>>($"https://localhost:7163/api/book/admin/{Page}");
            if (response != null && response.Data != null)
            {
                AdminBooks = response.Data.Books;
                Page = response.Data.Page;
                PageCount = response.Data.Pages;
            }
        }

        if (AdminBooks.Count == 0)
            Message = "No books found";

        OnChange?.Invoke();
    }

    public async Task GetFeaturedBooks()
    {
        var response = await _http.GetFromJsonAsync<ServiceResponse<BookListResult>>($"https://localhost:7163/api/book/featured/{Page}");
        if (response != null && response.Data != null)
        {
            Books = response.Data.Books;
            Page = response.Data.Page;
            PageCount = response.Data.Pages;
        }

        if (Books.Count == 0)
            Message = "No featured books found";

        OnChange?.Invoke();
    }

    public async Task GetBooks(string category = null)
    {
        ServiceResponse<BookListResult> response;
        if(category == null)
            response = await _http.GetFromJsonAsync<ServiceResponse<BookListResult>>($"https://localhost:7163/api/books/{Page}");
        else response = await _http.GetFromJsonAsync<ServiceResponse<BookListResult>>($"https://localhost:7163/api/books/{category}/{Page}");

        if (response != null && response.Data != null)
        {
            Books = response.Data.Books;
            Page = response.Data.Page;
            PageCount = response.Data.Pages;
        }

        if (Books.Count == 0)
            Message = "No books found";

        OnChange?.Invoke();
    }

    public async Task<Book> AddBook(Book book, string image)
    {
        var edit = new BookEdit
        {
            Book = book,
            NewImage = image
        };

        var response = await _http.PostAsJsonAsync("https://localhost:7163/api/book", edit);
        return (await response.Content
            .ReadFromJsonAsync<ServiceResponse<Book>>()).Data;
    }

    public async Task<Book> UpdateBook(Book book, string image)
    {
        var edit = new BookEdit
        {
            Book = book,
            NewImage = image
        };

        var response = await _http.PutAsJsonAsync("https://localhost:7163/api/book", edit);
        return (await response.Content
            .ReadFromJsonAsync<ServiceResponse<Book>>()).Data;
    }

    public async Task SearchBooks(string searchText)
    {
        LastSearchText = searchText;

        var result = await _http.GetFromJsonAsync<ServiceResponse<BookListResult>>($"https://localhost:7163/api/book/search/{searchText}/{Page}");
        if (result != null && result.Data != null)
        {
            Books = result.Data.Books;
            Page = result.Data.Page;
            PageCount = result.Data.Pages;
        }

        if (Books.Count == 0)
            Message = "No books found.";

        OnChange?.Invoke();
    }

    public async Task<List<string>> GetBookSearchSuggestions(string searchText)
    {
        var result = await _http.GetFromJsonAsync<ServiceResponse<List<string>>>($"https://localhost:7163/api/book/searchsuggestions/{searchText}");
        return result.Data;
    }
}
