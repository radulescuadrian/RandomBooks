using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RandomBooks.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<ActionResult<ServiceResponse<List<Category>>>> GetCategories()
    {
        var result = await _categoryService.GetCategories();
        return Ok(result);
    }

    [HttpGet("admin"), Authorize(Roles = "Admin")]
    public async Task<ActionResult<ServiceResponse<List<Category>>>> GetAdminCategories()
    {
        var result = await _categoryService.GetAdminCategories();
        return Ok(result);
    }
}
