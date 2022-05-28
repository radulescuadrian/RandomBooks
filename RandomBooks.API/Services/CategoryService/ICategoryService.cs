namespace RandomBooks.API.Services.CategoryService;

public interface ICategoryService
{
    Task<ServiceResponse<List<Category>>> GetAdminCategories();
    Task<ServiceResponse<List<Category>>> GetCategories();
}