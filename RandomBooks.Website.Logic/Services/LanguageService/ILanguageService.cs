namespace RandomBooks.Website.Logic.Services.LanguageService
{
    public interface ILanguageService
    {
        List<Language> Languages { get; set; }

        event Action OnChange;

        Task AddLanguage(Language language);
        Language CreateNewLanguage();
        Task DeleteLanguage(int languageId);
        Task GetLanguages(bool getAll = false);
        Task UpdateLanguage(Language language);
    }
}