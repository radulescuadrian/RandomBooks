namespace RandomBooks.Website.Logic.Services.UserService;

public class UserService : IUserService
{
    public List<User> Customers { get; set; } = new();
    public List<User> Employees { get; set; } = new();
    public List<User> Couriers { get; set; } = new();

    private readonly HttpClient _http;

    public event Action OnChange;
    public string Message { get; set; } = "Loading users...";
    public int Page { get; set; } = 1;
    public int PageCount { get; set; } = 0;

    public UserService(HttpClient http)
    {
        _http = http;
    }

    public void InitializePages()
    {
        Message = "Loading users...";
        Page = 1;
        PageCount = 0;
    }

    public async Task<ServiceResponse<User>> GetUser(int userId)
    {
        var response = await _http.GetFromJsonAsync<ServiceResponse<User>>($"https://localhost:7163/api/user/{userId}");
        return response;
    }

    public async Task GetUsers(string role)
    {
        var response = await _http.GetFromJsonAsync<ServiceResponse<UserListResult>>($"https://localhost:7163/api/{role}s/{Page}");
        if (response.Data != null)
        {
            switch (role)
            {
                case "Customer": Customers = response.Data.Users; break;
                case "Employee": Employees = response.Data.Users; break;
                case "Courier": Couriers = response.Data.Users; break;
                default: break;
            }

            Page = response.Data.Page;
            PageCount = response.Data.Pages;
        }
        else Message = response.Message;

        OnChange?.Invoke();
    }
}
