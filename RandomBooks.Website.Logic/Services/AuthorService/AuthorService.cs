namespace RandomBooks.Website.Logic.Services.AuthorService;

public class AuthorService : IAuthorService
{
    public List<Author> Authors { get; set; } = new();

    private readonly HttpClient _http;

    public event Action OnChange;
    public string Message { get; set; } = "Loading authors...";
    public int Page { get; set; } = 1;
    public int PageCount { get; set; } = 0;

    public AuthorService(HttpClient http)
    {
        _http = http;
    }

    public async Task<ServiceResponse<Author>> GetAuthor(int authorId)
    {
        var response = await _http.GetFromJsonAsync<ServiceResponse<Author>>($"https://localhost:7163/api/author/{authorId}");
        return response;
    }

    public async Task GetAuthors()
    {
        var response = await _http.GetFromJsonAsync<ServiceResponse<AuthorListResult>>($"https://localhost:7163/api/authors/{Page}");

        if (response != null && response.Data != null)
        {
            Authors = response.Data.Authors;
            Page = response.Data.Page;
            PageCount = response.Data.Pages;
        }

        if (Authors.Count == 0)
            Message = "No authors found";

        OnChange?.Invoke();
    }

    public async Task<Author> AddAuthor(Author author)
    {
        var response = await _http.PostAsJsonAsync("https://localhost:7163/api/author", author);
        return (await response.Content
            .ReadFromJsonAsync<ServiceResponse<Author>>()).Data;
    }

    public async Task<Author> UpdateAuthor(Author author)
    {
        var response = await _http.PutAsJsonAsync("https://localhost:7163/api/author", author);
        return (await response.Content
            .ReadFromJsonAsync<ServiceResponse<Author>>()).Data;
    }

    public Author CreateNewAuthor()
    {
        var newAuthor = new Author { New = true, Editing = true, Image = "user.svg" };

        Authors.Insert(0, newAuthor);
        OnChange.Invoke();

        return newAuthor;
    }
}
