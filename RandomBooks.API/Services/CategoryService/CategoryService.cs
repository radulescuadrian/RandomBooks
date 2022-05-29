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

    public async Task<ServiceResponse<List<Category>>> AddCategory(Category category)
    {
        category.Editing = category.New = false;
        _ctx.Categories.Add(category);
        await _ctx.SaveChangesAsync();

        return await GetAdminCategories();
    }

    public async Task<ServiceResponse<List<Category>>> UpdateCategory(Category category)
    {
        var dbCategory = await _ctx.Categories.FindAsync(category.Id);
        if (dbCategory == null)
            return new ServiceResponse<List<Category>>
            {
                Success = false,
                Message = "Category not found."
            };

        dbCategory.Name = category.Name;
        dbCategory.Deleted = category.Deleted;

        await _ctx.SaveChangesAsync();

        return await GetAdminCategories();
    }

    public async Task<ServiceResponse<List<Category>>> DeleteCategory(int categoryId)
    {
        var category = await _ctx.Categories.FindAsync(categoryId);
        if (category == null)
            return new ServiceResponse<List<Category>>
            {
                Success = false,
                Message = "Category not found."
            };

        category.Editing = category.New = false;
        category.Deleted = true;
        await _ctx.SaveChangesAsync();

        return await GetAdminCategories();
    }
}