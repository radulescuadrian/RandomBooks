using Microsoft.AspNetCore.Components.Authorization;
using RandomBooks.Shared.Authentication;
using System.Security.Claims;

namespace RandomBooks.Website.Logic.Services.AuthService;

public class AuthService : IAuthService
{
    private readonly HttpClient _http;
    private readonly AuthenticationStateProvider _authStateProvider;

    public AuthService(HttpClient http, AuthenticationStateProvider authStateProvider)
    {
        _http = http;
        _authStateProvider = authStateProvider;
    }

    public async Task<bool> IsUserAuthenticated() =>
            (await _authStateProvider.GetAuthenticationStateAsync()).User.Identity.IsAuthenticated;

    public async Task<string> GetUserRole() =>
            (await _authStateProvider.GetAuthenticationStateAsync()).User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role).Value;

    public async Task<string> GetUsername() =>
            (await _authStateProvider.GetAuthenticationStateAsync()).User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value;

    public async Task<ServiceResponse<string>> Login(UserLogin request)
    {
        var result = await _http.PostAsJsonAsync("api/auth/login", request);
        return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
    }

    public async Task<ServiceResponse<int>> Register(UserRegister request)
    {
        var result = await _http.PostAsJsonAsync("api/auth/register", request);
        return await result.Content.ReadFromJsonAsync<ServiceResponse<int>>();
    }

    public async Task<ServiceResponse<bool>> ChangePassword(UserChangePassword request)
    {
        var result = await _http.PostAsJsonAsync("api/auth/changePassword", request.Password);
        return await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
    }
}
