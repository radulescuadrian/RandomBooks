using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RandomBooks.API.Controllers;

[Route("api/[controller]"), Authorize]
[ApiController]
public class ChartController : ControllerBase
{
    private readonly IChartService _chartService;

    public ChartController(IChartService chartService)
    {
        _chartService = chartService;
    }
    
    [HttpGet("last-month-orders"), Authorize(Roles = "Admin")]
    public async Task<ActionResult<ServiceResponse<List<OrderCounter>>>> LastMonthOrders()
    {
        var result = await _chartService.LastMonthOrders();
        return Ok(result);
    }
}
