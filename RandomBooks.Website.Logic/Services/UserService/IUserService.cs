namespace RandomBooks.Website.Logic.Services.UserService
{
    public interface IUserService
    {
        string Message { get; set; }
        int Page { get; set; }
        int PageCount { get; set; }
        List<User> Customers { get; set; }
        List<User> Employees { get; set; }
        List<User> Couriers { get; set; }

        event Action OnChange;

        Task<ServiceResponse<User>> GetUser(int userId);
        Task GetUsers(string role);
        void InitializePages();
    }
}