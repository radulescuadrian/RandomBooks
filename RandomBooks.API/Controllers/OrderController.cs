using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace RandomBooks.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet("{orderId}"), Authorize]
    public async Task<ActionResult<ServiceResponse<OrdersResponse>>> GetOrder(int orderId)
    {
        var result = await _orderService.GetOrder(orderId);
        if (!result.Success)
            return BadRequest(result.Message);
        return Ok(result);
    }


    [HttpGet("invoice/{orderId}")]
    public async Task<IActionResult> GetInvoice(int orderId)
    {
        var result = await _orderService.GetInvoice(orderId);
        if (!result.Success)
            return BadRequest(result.Message);

        Response.ContentLength = result.Data.Length;
        return File(result.Data, "application/octet-stream", $"Invoice #{orderId}.pdf");
    }

    [HttpGet, Authorize]
    public async Task<ActionResult<ServiceResponse<List<OrdersResponse>>>> GetOrders()
    {
        var result = await _orderService.GetOrders();
        if (!result.Success)
            return BadRequest(result.Message);
        return Ok(result);
    }

    [HttpGet("admin/{page}"), Authorize(Roles = "Admin")]
    public async Task<ActionResult<ServiceResponse<List<OrdersResponse>>>> GetAdminOrders(int page = 1)
    {
        var result = await _orderService.GetAdminOrders(page);
        if (!result.Success)
            return BadRequest(result.Message);
        return Ok(result);
    }

    [HttpPost, Authorize]
    public async Task<ActionResult<ServiceResponse<bool>>> PlaceOrder(OrderRequest order)
    {
        var result = await _orderService.PlaceOrder(order);
        if (!result.Success)
            return BadRequest(result.Message);
        return Ok(result);
    }

    [HttpPost("rating"), Authorize]
    public async Task<ActionResult<ServiceResponse<bool>>> RateOrder(OrderRating rating)
    {
        var result = await _orderService.RateOrder(rating);
        if (!result.Success)
            return BadRequest(result.Message);
        return Ok(result);
    }
}
