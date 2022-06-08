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

    #region Profile related
    [HttpGet("profile"), Authorize(Roles = "Customer")]
    public async Task<ActionResult<ServiceResponse<CustomerDetails>>> GetProfile()
    {
        var result = await _userService.GetProfile();
        return Ok(result);
    }

    [HttpPut("profile"), Authorize(Roles = "Customer")]
    public async Task<ActionResult<ServiceResponse<string>>> UpdateProfile(ProfileEdit edit)
    {
        var result = await _userService.UpdateProfile(edit);

        if (result.Data is null)
            return BadRequest(result.Message);

        return Ok(result);
    } 
    #endregion

    #region Addresses
    [HttpPost("address"), Authorize(Roles = "Customer")]
    public async Task<ActionResult<ServiceResponse<string>>> AddAddress(Address address)
    {
        var result = await _userService.AddAddress(address);
        return Ok(result);
    }


    [HttpPut("address"), Authorize(Roles = "Customer")]
    public async Task<ActionResult<ServiceResponse<string>>> UpdateAddress(Address address)
    {
        var result = await _userService.UpdateAddress(address);
        return Ok(result);
    }

    [HttpDelete("address/{addressId}"), Authorize(Roles = "Customer")]
    public async Task<ActionResult<ServiceResponse<string>>> DeleteAddress(int addressId)
    {
        var result = await _userService.DeleteAddress(addressId);
        return Ok(result);
    }
    #endregion

    #region Cards
    [HttpPost("card"), Authorize(Roles = "Customer")]
    public async Task<ActionResult<ServiceResponse<string>>> AddCard(CustomerCard card)
    {
        var result = await _userService.AddCard(card);
        return Ok(result);
    }


    [HttpPut("card"), Authorize(Roles = "Customer")]
    public async Task<ActionResult<ServiceResponse<string>>> UpdateCard(CustomerCard card)
    {
        var result = await _userService.UpdateCard(card);
        return Ok(result);
    }

    [HttpDelete("card/{cardId}"), Authorize(Roles = "Customer")]
    public async Task<ActionResult<ServiceResponse<string>>> DeleteCard(int cardId)
    {
        var result = await _userService.DeleteCard(cardId);
        return Ok(result);
    }
    #endregion
}
