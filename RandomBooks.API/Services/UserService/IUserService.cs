namespace RandomBooks.API.Services.UserService
{
    public interface IUserService
    {
        Task<ServiceResponse<User>> GetUser(int userId);
        Task<ServiceResponse<UserListResult>> GetUsers(int page, string role);
    }
}