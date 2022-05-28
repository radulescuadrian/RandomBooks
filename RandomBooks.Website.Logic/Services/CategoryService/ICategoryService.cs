namespace RandomBooks.Website.Logic.Services.CategoryService
{
    public interface ICategoryService
    {
        List<Category> AdminCategories { get; set; }
        List<Category> Categories { get; set; }

        event Action OnChange;

        Task GetAdminCategories();
        Task GetCategories();
    }
}