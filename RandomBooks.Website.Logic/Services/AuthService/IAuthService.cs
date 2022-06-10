using RandomBooks.Shared.Authentication;

namespace RandomBooks.Website.Logic.Services.AuthService;

public interface IAuthService
{
    Task<ServiceResponse<bool>> ChangePassword(UserChangePassword request);
    Task<int?> GetUserId();
    Task<string> GetUsername();
    Task<string> GetUserRole();
    Task<bool> IsUserAuthenticated();
    Task<ServiceResponse<string>> Login(UserLogin request);
    Task<ServiceResponse<string>> Register(UserRegister request);
}