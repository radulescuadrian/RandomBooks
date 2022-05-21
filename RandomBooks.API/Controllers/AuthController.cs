using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RandomBooks.Data.Models;
using RandomBooks.Shared.Authentication;
using System.Security.Claims;

namespace RandomBooks.API.Controllers
{
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
        public async Task<ActionResult<ServiceResponse<int>>> Register(UserRegister request)
        {
            var response = await _authService.Register(new User { Username = request.Username },
                                                       request.Password);

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpPost("changePassword"), Authorize]
        public async Task<ActionResult<ServiceResponse<bool>>> ChangePassword([FromBody] string newPassword)
        {
            // get the userid from the claims
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _authService.ChangePassword(int.Parse(userId), newPassword);
            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }
    }
}
