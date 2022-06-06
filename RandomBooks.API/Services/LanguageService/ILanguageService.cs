namespace RandomBooks.API.Services.LanguageService
{
    public interface ILanguageService
    {
        Task<ServiceResponse<List<Language>>> AddLanguage(Language language);
        Task<ServiceResponse<List<Language>>> GetAllLanguages();
        Task<ServiceResponse<List<Language>>> GetLanguages();
        Task<ServiceResponse<List<Language>>> UpdateLanguage(Language language);
    }
}