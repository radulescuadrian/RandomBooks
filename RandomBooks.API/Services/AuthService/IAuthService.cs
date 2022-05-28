namespace RandomBooks.API.Services.AuthService;

public interface IAuthService
{
    Task<ServiceResponse<bool>> ChangePassword(int userId, string newPassword);
    Task<User> GetUserByUsername(string username);
    int GetUserId();
    string GetUserUsername();
    Task<ServiceResponse<string>> Login(string username, string password);
    Task<ServiceResponse<string>> Register(User user, string password);
    Task<bool> UserExists(string username);
}