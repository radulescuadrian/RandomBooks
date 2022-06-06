namespace RandomBooks.Website.Logic.Services.CategoryService
{
    public interface ICategoryService
    {
        List<Category> Categories { get; set; }

        event Action OnChange;

        Task AddCategory(Category category);
        Category CreateNewCategory();
        Task GetAdminCategories();
        Task GetCategories();
        Task<List<Category>> GetVisibleCategoriesList();
        Task UpdateCategory(Category category);
    }
}