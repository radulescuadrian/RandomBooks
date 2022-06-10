using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RandomBooks.API.Controllers;

[Route("api/[controller]"), Authorize]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    public async Task<ActionResult<ServiceResponse<List<OrdersResponse>>>> GetOrders()
    {
        var result = await _orderService.GetOrders();
        if (!result.Success)
            return BadRequest(result.Message);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ServiceResponse<bool>>> PlaceOrder(OrderRequest order)
    {
        var result = await _orderService.PlaceOrder(order);
        if (!result.Success)
            return BadRequest(result.Message);
        return Ok(result);
    }
}
