namespace RandomBooks.Website.Logic.Services.LanguageService
{
    public interface ILanguageService
    {
        List<Language> Languages { get; set; }

        event Action OnChange;

        Task AddLanguage(Language language);
        Language CreateNewLanguage();
        Task GetLanguages(bool getAll = false);
        Task<List<Language>> GetVisibleLanguagesList();
        Task UpdateLanguage(Language language);
    }
}