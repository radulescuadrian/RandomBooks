namespace RandomBooks.Website.Logic.Services.CategoryService
{
    public interface ICategoryService
    {
        List<Category> Categories { get; set; }

        event Action OnChange;

        Task AddCategory(Category category);
        Category CreateNewCategory();
        Task DeleteCategory(int categoryId);
        Task GetAdminCategories();
        Task GetCategories();
        Task UpdateCategory(Category category);
    }
}