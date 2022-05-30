namespace RandomBooks.Website.Logic.Services.CategoryService;

public class CategoryService : ICategoryService
{
    public List<Category> Categories { get; set; } = new();

    private readonly HttpClient _http;

    public event Action OnChange;

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
        else Categories = new List<Category>();
    }

    public async Task GetAdminCategories()
    {
        var response = await _http.GetFromJsonAsync<ServiceResponse<List<Category>>>("https://localhost:7163/api/category/admin");
        if (response != null && response.Data != null)
            Categories = response.Data;
        else Categories = new List<Category>();
    }

    public async Task AddCategory(Category category)
    {
        var response = await _http.PostAsJsonAsync("https://localhost:7163/api/category/admin", category);
        if(response != null)
        {
            Categories = (await response.Content
                .ReadFromJsonAsync<ServiceResponse<List<Category>>>()).Data;
            OnChange.Invoke();
            return;
        }
        else Categories = new List<Category>();
    }

    public async Task UpdateCategory(Category category)
    {
        var response = await _http.PutAsJsonAsync("https://localhost:7163/api/category/admin", category);
        if (response != null)
        {
            Categories = (await response.Content
                .ReadFromJsonAsync<ServiceResponse<List<Category>>>()).Data;
            OnChange.Invoke();
            return;
        }
        else Categories = new List<Category>();
    }

    public async Task DeleteCategory(int categoryId)
    {
        var response = await _http.DeleteAsync($"https://localhost:7163/api/category/admin/{categoryId}");
        if (response != null)
        {
            Categories = (await response.Content
                .ReadFromJsonAsync<ServiceResponse<List<Category>>>()).Data;
            OnChange.Invoke();
            return;
        }
        else Categories = new List<Category>();
    }

    public Category CreateNewCategory()
    {
        var newCategory = new Category { New = true, Editing = true };

        Categories.Insert(0, newCategory);
        OnChange.Invoke();

        return newCategory;
    }
}
