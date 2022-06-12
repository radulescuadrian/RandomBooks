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
    public async Task<ActionResult<ServiceResponse<User>>> GetUser(int userId)
    {
        var result = await _userService.GetUser(userId);
        return Ok(result);
    }

    [HttpGet("~/api/customers/{page}"), Authorize(Roles = "Admin")]
    public async Task<ActionResult<ServiceResponse<CustomerListResult>>> GetCustomers(int page = 1)
    {
        var result = await _userService.GetCustomers(page);
        return Ok(result);
    }

    [HttpGet("~/api/employees/{page}"), Authorize(Roles = "Admin")]
    public async Task<ActionResult<ServiceResponse<EmployeeListResult>>> GetEmployees(int page = 1)
    {
        var result = await _userService.GetEmployees(page);
        return Ok(result);
    }

    [HttpGet("~/api/couriers/{page}"), Authorize(Roles = "Admin")]
    public async Task<ActionResult<ServiceResponse<CourierListResult>>> GetCouriers(int page = 1)
    {
        var result = await _userService.GetCouriers(page);
        return Ok(result);
    }

    [HttpGet("customer/{userId}"), Authorize(Roles = "Admin")]
    public async Task<ActionResult<ServiceResponse<CustomerDetailsResponse>>> GetCustomerDetails(int userId)
    {
        var result = await _userService.GetAdminCustomerDetails(userId);
        if (!result.Success)
            return BadRequest(result.Message);
        return Ok(result);
    }

    [HttpGet("employee/{userId}"), Authorize(Roles = "Admin")]
    public async Task<ActionResult<ServiceResponse<EmployeeDetailsResponse>>> GetEmployeeDetails(int userId)
    {
        var result = await _userService.GetAdminEmployeeDetails(userId);
        if (!result.Success)
            return BadRequest(result.Message);
        return Ok(result);
    }

    [HttpGet("courier/{userId}"), Authorize(Roles = "Admin")]
    public async Task<ActionResult<ServiceResponse<EmployeeDetailsResponse>>> GetCourierDetails(int userId)
    {
        var result = await _userService.GetAdminCourierDetails(userId);
        if (!result.Success)
            return BadRequest(result.Message);
        return Ok(result);
    }

    [HttpPost("deactivate"), Authorize(Roles = "Admin")]
    public async Task<ActionResult<ServiceResponse<bool>>> DeactivateUser([FromBody] int userId)
    {
        var result = await _userService.DeactivateUser(userId);
        if (!result.Success)
            return BadRequest(result);
        return Ok(result);
    }

    [HttpPost("activate"), Authorize(Roles = "Admin")]
    public async Task<ActionResult<ServiceResponse<bool>>> ActivateUser([FromBody] int userId)
    {
        var result = await _userService.ActivateUser(userId);
        if (!result.Success)
            return BadRequest(result);
        return Ok(result);
    }

    [HttpPost("addemployee"), Authorize(Roles = "Admin")]
    public async Task<ActionResult<ServiceResponse<bool>>> AddEmployee(NewEmlpoyee employee)
    {
        var result = await _userService.AddEmployee(employee);
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
