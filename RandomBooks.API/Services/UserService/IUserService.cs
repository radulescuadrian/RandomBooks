namespace RandomBooks.API.Services.UserService
{
    public interface IUserService
    {
        Task<ServiceResponse<string>> AddAddress(Address address);
        Task<ServiceResponse<string>> AddCard(CustomerCard card);
        Task<ServiceResponse<string>> DeleteAddress(int addressId);
        Task<ServiceResponse<string>> DeleteCard(int cardId);
        Task<ServiceResponse<CustomerDetails>> GetProfile();
        Task<ServiceResponse<User>> GetUser(int userId);
        Task<ServiceResponse<UserListResult>> GetUsers(int page, string role);
        Task<ServiceResponse<string>> UpdateAddress(Address address);
        Task<ServiceResponse<string>> UpdateCard(CustomerCard card);
        Task<ServiceResponse<string>> UpdateProfile(ProfileEdit edit);
    }
}