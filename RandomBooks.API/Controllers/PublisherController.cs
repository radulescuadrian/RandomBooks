using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RandomBooks.API.Controllers;

[Route("api/[controller]"), Authorize(Roles = "Admin")]
[ApiController]
public class PublisherController : ControllerBase
{
    private readonly IPublisherService _publisherService;

    public PublisherController(IPublisherService publisherService)
    {
        _publisherService = publisherService;
    }

    [HttpGet]
    public async Task<ActionResult<ServiceResponse<List<Publisher>>>> GetPublishers()
    {
        var result = await _publisherService.GetPublishers();
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ServiceResponse<List<Publisher>>>> AddPublisher(Publisher publisher)
    {
        var result = await _publisherService.AddPublisher(publisher);
        return Ok(result);
    }

    [HttpPut]
    public async Task<ActionResult<ServiceResponse<List<Publisher>>>> UpdatePublisher(Publisher publisher)
    {
        var result = await _publisherService.UpdatePublisher(publisher);
        return Ok(result);
    }

    [HttpDelete("{publisherId}")]
    public async Task<ActionResult<ServiceResponse<List<Publisher>>>> DeletePublisher(int publisherId)
    {
        var result = await _publisherService.DeletePublisher(publisherId);
        return Ok(result);
    }
}
