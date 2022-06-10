namespace RandomBooks.API.Services.AuthService;

public interface IAuthService
{
    Task<ServiceResponse<bool>> ChangePassword(int userId, string newPassword);
    Task<bool> EmailExists(string email);
    Task<User> GetUserByUsername(string username);
    int GetUserId();
    string GetUserUsername();
    Task<ServiceResponse<string>> Login(string username, string password);
    Task<ServiceResponse<string>> Register(UserRegister user);
    Task<bool> UserExists(string username);
}