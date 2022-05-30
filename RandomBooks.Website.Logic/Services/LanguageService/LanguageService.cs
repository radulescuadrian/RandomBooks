namespace RandomBooks.Website.Logic.Services.LanguageService;

public class LanguageService : ILanguageService
{
    public List<Language> Languages { get; set; } = new();

    private readonly HttpClient _http;

    public event Action OnChange;

    public LanguageService(HttpClient http)
    {
        _http = http;
    }

    public async Task GetLanguages(bool getAll = false)
    {
        var response = new ServiceResponse<List<Language>>();
        if (getAll)
            response = await _http.GetFromJsonAsync<ServiceResponse<List<Language>>>("https://localhost:7163/api/language/all");
        else response = await _http.GetFromJsonAsync<ServiceResponse<List<Language>>>("https://localhost:7163/api/language");

        if (response != null && response.Data != null)
            Languages = response.Data;
        else Languages = new List<Language>();
    }

    public async Task AddLanguage(Language language)
    {
        var response = await _http.PostAsJsonAsync("https://localhost:7163/api/language", language);
        if (response != null)
        {
            Languages = (await response.Content
                .ReadFromJsonAsync<ServiceResponse<List<Language>>>()).Data;
            OnChange.Invoke();
            return;
        }
        else Languages = new List<Language>();
    }

    public async Task UpdateLanguage(Language language)
    {
        var response = await _http.PutAsJsonAsync("https://localhost:7163/api/language", language);
        if (response != null)
        {
            Languages = (await response.Content
                .ReadFromJsonAsync<ServiceResponse<List<Language>>>()).Data;
            OnChange.Invoke();
            return;
        }
        else Languages = new List<Language>();
    }

    public async Task DeleteLanguage(int languageId)
    {
        var response = await _http.DeleteAsync($"https://localhost:7163/api/language/{languageId}");
        if (response != null)
        {
            Languages = (await response.Content
                .ReadFromJsonAsync<ServiceResponse<List<Language>>>()).Data;
            OnChange.Invoke();
            return;
        }
        else Languages = new List<Language>();
    }

    public Language CreateNewLanguage()
    {
        var newLanguage = new Language { New = true, Editing = true };

        Languages.Insert(0, newLanguage);
        OnChange.Invoke();

        return newLanguage;
    }
}
