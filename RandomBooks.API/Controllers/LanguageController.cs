using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RandomBooks.API.Controllers
{
    [Route("api/[controller]"), Authorize(Roles = "Admin")]
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

        [HttpGet("all")]
        public async Task<ActionResult<ServiceResponse<List<Language>>>> GetAllLanguages()
        {
            var result = await _languageService.GetAllLanguages();
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<List<Language>>>> AddLanguage(Language language)
        {
            var result = await _languageService.AddLanguage(language);
            return Ok(result);
        }

        [HttpPut]
        public async Task<ActionResult<ServiceResponse<List<Language>>>> UpdateCategory(Language language)
        {
            var result = await _languageService.UpdateLanguage(language);
            return Ok(result);
        }

        [HttpDelete("{languageId}")]
        public async Task<ActionResult<ServiceResponse<List<Language>>>> DeleteLanguage(int languageId)
        {
            var result = await _languageService.DeleteLanguage(languageId);
            return Ok(result);
        }
    }
}
