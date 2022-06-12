namespace RandomBooks.API.Services.UserService
{
    public interface IUserService
    {
        Task<ServiceResponse<bool>> ActivateUser(int userId);
        Task<ServiceResponse<string>> AddAddress(Address address);
        Task<ServiceResponse<string>> AddCard(CustomerCard card);
        Task<ServiceResponse<bool>> AddEmployee(NewEmlpoyee employee);
        Task<ServiceResponse<bool>> DeactivateUser(int userId);
        Task<ServiceResponse<string>> DeleteAddress(int addressId);
        Task<ServiceResponse<string>> DeleteCard(int cardId);
        Task<ServiceResponse<EmployeeDetailsResponse>> GetAdminCourierDetails(int userId);
        Task<ServiceResponse<CustomerDetailsResponse>> GetAdminCustomerDetails(int userId);
        Task<ServiceResponse<EmployeeDetailsResponse>> GetAdminEmployeeDetails(int userId);
        Task<ServiceResponse<CourierListResult>> GetCouriers(int page);
        Task<ServiceResponse<CustomerListResult>> GetCustomers(int page);
        Task<ServiceResponse<EmployeeListResult>> GetEmployees(int page);
        Task<ServiceResponse<CustomerDetails>> GetProfile();
        Task<ServiceResponse<User>> GetUser(int userId);
        Task<ServiceResponse<string>> UpdateAddress(Address address);
        Task<ServiceResponse<string>> UpdateCard(CustomerCard card);
        Task<ServiceResponse<string>> UpdateProfile(ProfileEdit edit);
    }
}