namespace RandomBooks.Website.Logic.Services.UserService
{
    public interface IUserService
    {
        string Message { get; set; }
        int Page { get; set; }
        int PageCount { get; set; }
        List<CustomersResponse> Customers { get; set; }
        List<EmployeesResponse> Employees { get; set; }
        List<CouriersResponse> Couriers { get; set; }

        event Action OnChange;

        Task<ServiceResponse<bool>> ActivateUser(int userId);
        Task<ServiceResponse<bool>> AddEmployee(NewEmlpoyee employee);
        Task<ServiceResponse<bool>> DeactivateUser(int userId);
        Task<ServiceResponse<EmployeeDetailsResponse>> GetCourierDetails(int userId);
        Task GetCouriers();
        Task<ServiceResponse<CustomerDetailsResponse>> GetCustomerDetails(int userId);
        Task GetCustomers();
        Task<ServiceResponse<EmployeeDetailsResponse>> GetEmployeeDetails(int userId);
        Task GetEmployees();
        Task<ServiceResponse<User>> GetUser(int userId);
        void InitializePages();
    }
}