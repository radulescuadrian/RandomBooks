using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RandomBooks.API.Controllers;

[Route("api/[controller]"), Authorize]
[ApiController]
public class PublisherController : ControllerBase
{
    private readonly IPublisherService _publisherService;

    public PublisherController(IPublisherService publisherService)
    {
        _publisherService = publisherService;
    }

    [HttpGet, Authorize(Roles = "Admin")]
    public async Task<ActionResult<ServiceResponse<List<Publisher>>>> GetPublishers()
    {
        var result = await _publisherService.GetPublishers();
        return Ok(result);
    }

    [HttpGet("visible")]
    public async Task<ActionResult<ServiceResponse<List<Publisher>>>> GetVisiblePublishers()
    {
        var result = await _publisherService.GetVisiblePublishers();
        return Ok(result);
    }

    [HttpPost, Authorize(Roles = "Admin")]
    public async Task<ActionResult<ServiceResponse<List<Publisher>>>> AddPublisher(Publisher publisher)
    {
        var result = await _publisherService.AddPublisher(publisher);
        return Ok(result);
    }

    [HttpPut, Authorize(Roles = "Admin")]
    public async Task<ActionResult<ServiceResponse<List<Publisher>>>> UpdatePublisher(Publisher publisher)
    {
        var result = await _publisherService.UpdatePublisher(publisher);
        return Ok(result);
    }
}
