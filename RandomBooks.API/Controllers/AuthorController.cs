using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RandomBooks.API.Controllers;

[Route("api/[controller]"), Authorize(Roles = "Admin")]
[ApiController]
public class AuthorController : ControllerBase
{
    private readonly IAuthorService _authorService;

    public AuthorController(IAuthorService authorService)
    {
        _authorService = authorService;
    }

    [HttpGet("{authorId}")]
    public async Task<ActionResult<ServiceResponse<Author>>> GetAuthor(int authorId)
    {
        var result = await _authorService.GetAuthor(authorId);
        return Ok(result);
    }

    [HttpGet("~/api/Authors/{page}")]
    public async Task<ActionResult<ServiceResponse<AuthorListResult>>> GetAuthors(int page = 1)
    {
        var result = await _authorService.GetAuthors(page);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ServiceResponse<List<Author>>>> AddAuthor(Author author)
    {
        var result = await _authorService.AddAuthor(author);
        return Ok(result);
    }

    [HttpPut]
    public async Task<ActionResult<ServiceResponse<List<Author>>>> UpdateAuthor(Author author)
    {
        var result = await _authorService.UpdateAuthor(author);
        return Ok(result);
    }
}
