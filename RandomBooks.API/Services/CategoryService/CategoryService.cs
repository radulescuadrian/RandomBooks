namespace RandomBooks.API.Services.CategoryService;

public class CategoryService : ICategoryService
{
    private readonly DataContext _ctx;

    public CategoryService(DataContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<ServiceResponse<List<Category>>> GetCategories()
    {
        var categories = await _ctx.Categories
            .Where(x => !x.Deleted)
            .ToListAsync();
        return new ServiceResponse<List<Category>> { Data = categories };
    }

    public async Task<ServiceResponse<List<Category>>> GetAdminCategories()
    {
        var categories = await _ctx.Categories
            .ToListAsync();
        return new ServiceResponse<List<Category>> { Data = categories };
    }

    #region Helper Methods
    private async Task<Category> GetCategoryById(int categoryId) =>
        await _ctx.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);

    private async Task<Category> GetCategoryByName(string categoryName) =>
        await _ctx.Categories.FirstOrDefaultAsync(c => c.Name == categoryName);
    #endregion
}