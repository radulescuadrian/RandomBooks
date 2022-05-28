using RandomBooks.Shared;

namespace RandomBooks.Website.Logic.Services.CategoryService;

public class CategoryService : ICategoryService
{
    private readonly HttpClient _http;

    public event Action OnChange;
    public List<Category> Categories { get; set; } = new List<Category>();
    public List<Category> AdminCategories { get; set; } = new List<Category>();

    public CategoryService(HttpClient http)
    {
        _http = http;
    }

    public async Task GetCategories()
    {
        var response = await _http.GetFromJsonAsync<ServiceResponse<List<Category>>>("https://localhost:7163/api/category");
        if (response != null && response.Data != null)
        {
            Categories = response.Data;
            Categories.Insert(0, new Category
            {
                Name = "All books"
            });
        }
    }

    public async Task GetAdminCategories()
    {
        var response = await _http.GetFromJsonAsync<ServiceResponse<List<Category>>>("https://localhost:7163/api/category/admin");
        if (response != null && response.Data != null)
            AdminCategories = response.Data;
    }
}
