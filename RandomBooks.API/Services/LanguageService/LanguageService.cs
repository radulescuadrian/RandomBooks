namespace RandomBooks.API.Services.LanguageService;

public class LanguageService : ILanguageService
{
    private readonly DataContext _ctx;

    public LanguageService(DataContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<ServiceResponse<List<Language>>> GetLanguages()
    {
        var languages = await _ctx.Languages
            .OrderBy(x => x.Name)
            .Where(x => !x.Deleted)
            .ToListAsync();
        return new ServiceResponse<List<Language>> { Data = languages };
    }

    public async Task<ServiceResponse<List<Language>>> GetAllLanguages()
    {
        var languages = await _ctx.Languages
            .OrderBy(x => x.Name)
            .ToListAsync();
        return new ServiceResponse<List<Language>> { Data = languages };
    }

    public async Task<ServiceResponse<List<Language>>> AddLanguage(Language language)
    {
        language.Editing = language.New = false;
        _ctx.Languages.Add(language);
        await _ctx.SaveChangesAsync();

        return await GetAllLanguages();
    }

    public async Task<ServiceResponse<List<Language>>> UpdateLanguage(Language language)
    {
        var dbLanguage = await _ctx.Languages.FindAsync(language.Id);
        if (dbLanguage == null)
            return new ServiceResponse<List<Language>>
            {
                Success = false,
                Message = "Language not found."
            };

        dbLanguage.Name = language.Name;
        dbLanguage.DisplayName = language.DisplayName;
        dbLanguage.Deleted = language.Deleted;

        await _ctx.SaveChangesAsync();

        return await GetAllLanguages();
    }
}
