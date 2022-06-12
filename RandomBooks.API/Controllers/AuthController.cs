using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RandomBooks.Shared.Authentication;
using System.Security.Claims;

namespace RandomBooks.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<ServiceResponse<string>>> Login(UserLogin request)
    {
        var response = await _authService.Login(request.Username, request.Password);
        if (!response.Success)
            return BadRequest(response);

        return Ok(response);
    }

    [HttpPost("register")]
    public async Task<ActionResult<ServiceResponse<string>>> Register(UserRegister request)
    {
        var response = await _authService.Register(request);

        if (!response.Success)
            return BadRequest(response);

        return Ok(response);
    }

    [HttpPost("change-password"), Authorize]
    public async Task<ActionResult<ServiceResponse<bool>>> ChangePassword(ChangePasswordRequest request)
    {
        var response = await _authService.ChangePassword(request.Id, request.Password);
        if (!response.Success)
            return BadRequest(response);

        return Ok(response);
    }
}
