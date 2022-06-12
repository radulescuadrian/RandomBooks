namespace RandomBooks.Website.Logic.Services.UserService;

public class UserService : IUserService
{
    public List<CustomersResponse> Customers { get; set; } = new();
    public List<EmployeesResponse> Employees { get; set; } = new();
    public List<CouriersResponse> Couriers { get; set; } = new();

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

    public async Task<ServiceResponse<CustomerDetailsResponse>> GetCustomerDetails(int userId)
    {
        var response = await _http.GetFromJsonAsync<ServiceResponse<CustomerDetailsResponse>>($"https://localhost:7163/api/user/customer/{userId}");
        return response;
    }

    public async Task<ServiceResponse<EmployeeDetailsResponse>> GetEmployeeDetails(int userId)
    {
        var response = await _http.GetFromJsonAsync<ServiceResponse<EmployeeDetailsResponse>>($"https://localhost:7163/api/user/employee/{userId}");
        return response;
    }

    public async Task<ServiceResponse<EmployeeDetailsResponse>> GetCourierDetails(int userId)
    {
        var response = await _http.GetFromJsonAsync<ServiceResponse<EmployeeDetailsResponse>>($"https://localhost:7163/api/user/courier/{userId}");
        return response;
    }

    public async Task GetCustomers()
    {
        var response = await _http.GetFromJsonAsync<ServiceResponse<CustomerListResult>>($"https://localhost:7163/api/customers/{Page}");
        if (response.Data != null)
        {
            Customers = response.Data.Customers;
            Page = response.Data.Page;
            PageCount = response.Data.Pages;
        }
        else Message = response.Message;

        OnChange?.Invoke();
    }

    public async Task GetEmployees()
    {
        var response = await _http.GetFromJsonAsync<ServiceResponse<EmployeeListResult>>($"https://localhost:7163/api/employees/{Page}");
        if (response.Data != null)
        {
            Employees = response.Data.Employees;
            Page = response.Data.Page;
            PageCount = response.Data.Pages;
        }
        else Message = response.Message;

        OnChange?.Invoke();
    }

    public async Task GetCouriers()
    {
        var response = await _http.GetFromJsonAsync<ServiceResponse<CourierListResult>>($"https://localhost:7163/api/couriers/{Page}");
        if (response.Data != null)
        {
            Couriers = response.Data.Couriers;
            Page = response.Data.Page;
            PageCount = response.Data.Pages;
        }
        else Message = response.Message;

        OnChange?.Invoke();
    }

    public async Task<ServiceResponse<bool>> DeactivateUser(int userId)
    {
        var response = await _http.PostAsJsonAsync("https://localhost:7163/api/user/deactivate", userId);
        return await response.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
    }

    public async Task<ServiceResponse<bool>> ActivateUser(int userId)
    {
        var response = await _http.PostAsJsonAsync("https://localhost:7163/api/user/activate", userId);
        return await response.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
    }

    public async Task<ServiceResponse<bool>> AddEmployee(NewEmlpoyee employee)
    {
        var response = await _http.PostAsJsonAsync("https://localhost:7163/api/user/addemployee", employee);
        return await response.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
    }
}
