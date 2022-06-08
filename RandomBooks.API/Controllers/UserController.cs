using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RandomBooks.API.Controllers;

[Route("api/[controller]"), Authorize]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("{userId}"), Authorize(Roles = "Admin")]
    public async Task<ActionResult<ServiceResponse<User>>> GetAuthor(int userId)
    {
        var result = await _userService.GetUser(userId);
        return Ok(result);
    }

    [HttpGet("~/api/{role}s/{page}"), Authorize(Roles = "Admin")]
    public async Task<ActionResult<ServiceResponse<UserListResult>>> GetUsers(string role, int page = 1)
    {
        var result = await _userService.GetUsers(page, role);
        return Ok(result);
    }
}
