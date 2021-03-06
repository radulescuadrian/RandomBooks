using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RandomBooks.API.Controllers;

[Route("api/[controller]"), Authorize]
[ApiController]
public class LanguageController : ControllerBase
{
    private readonly ILanguageService _languageService;

    public LanguageController(ILanguageService languageService)
    {
        _languageService = languageService;
    }

    [HttpGet]
    public async Task<ActionResult<ServiceResponse<List<Language>>>> GetLanguages()
    {
        var result = await _languageService.GetLanguages();
        return Ok(result);
    }

    [HttpGet("all"), Authorize(Roles = "Admin")]
    public async Task<ActionResult<ServiceResponse<List<Language>>>> GetAllLanguages()
    {
        var result = await _languageService.GetAllLanguages();
        return Ok(result);
    }

    [HttpPost, Authorize(Roles = "Admin")]
    public async Task<ActionResult<ServiceResponse<List<Language>>>> AddLanguage(Language language)
    {
        var result = await _languageService.AddLanguage(language);
        return Ok(result);
    }

    [HttpPut, Authorize(Roles = "Admin")]
    public async Task<ActionResult<ServiceResponse<List<Language>>>> UpdateCategory(Language language)
    {
        var result = await _languageService.UpdateLanguage(language);
        return Ok(result);
    }
}
