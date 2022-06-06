using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RandomBooks.API.Controllers;

[Route("api/[controller]"), Authorize]
[ApiController]
public class AuthorController : ControllerBase
{
    private readonly IAuthorService _authorService;

    public AuthorController(IAuthorService authorService)
    {
        _authorService = authorService;
    }

    [HttpGet("{authorId}"), Authorize(Roles = "Admin")]
    public async Task<ActionResult<ServiceResponse<Author>>> GetAuthor(int authorId)
    {
        var result = await _authorService.GetAuthor(authorId);
        return Ok(result);
    }

    [HttpGet("visible")]
    public async Task<ActionResult<ServiceResponse<List<Author>>>> GetVisibleAuthors()
    {
        var result = await _authorService.GetVisibleAuthors();
        return Ok(result);
    }

    [HttpGet("~/api/Authors/{page}"), Authorize(Roles = "Admin")]
    public async Task<ActionResult<ServiceResponse<AuthorListResult>>> GetAuthors(int page = 1)
    {
        var result = await _authorService.GetAuthors(page);
        return Ok(result);
    }

    [HttpPost, Authorize(Roles = "Admin")]
    public async Task<ActionResult<ServiceResponse<Author>>> AddAuthor(AuthorEdit edit)
    {
        var result = await _authorService.AddAuthor(edit);
        return Ok(result);
    }

    [HttpPut, Authorize(Roles = "Admin")]
    public async Task<ActionResult<ServiceResponse<Author>>> UpdateAuthor(AuthorEdit edit)
    {
        var result = await _authorService.UpdateAuthor(edit);
        return Ok(result);
    }
}
