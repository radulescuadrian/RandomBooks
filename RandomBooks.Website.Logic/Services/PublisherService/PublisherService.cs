namespace RandomBooks.Website.Logic.Services.PublisherService;

public class PublisherService : IPublisherService
{
    public List<Publisher> Publishers { get; set; } = new();

    private readonly HttpClient _http;

    public event Action OnChange;

    public PublisherService(HttpClient http)
    {
        _http = http;
    }

    public async Task GetPublishers()
    {
        var response = await _http.GetFromJsonAsync<ServiceResponse<List<Publisher>>>("https://localhost:7163/api/publisher");

        if (response != null && response.Data != null)
            Publishers = response.Data;
        else Publishers = new List<Publisher>();
    }

    public async Task<List<Publisher>> GetVisiblePublishersList()
    {
        var response = await _http.GetFromJsonAsync<ServiceResponse<List<Publisher>>>($"https://localhost:7163/api/publisher/visible");

        if (response != null && response.Data != null)
            return response.Data;
        else return new List<Publisher>();
    }

    public async Task AddPublisher(Publisher publisher)
    {
        var response = await _http.PostAsJsonAsync("https://localhost:7163/api/publisher", publisher);
        if (response != null)
        {
            Publishers = (await response.Content
                .ReadFromJsonAsync<ServiceResponse<List<Publisher>>>()).Data;
            OnChange.Invoke();
            return;
        }
        else Publishers = new List<Publisher>();
    }

    public async Task UpdatePublisher(Publisher publisher)
    {
        var response = await _http.PutAsJsonAsync("https://localhost:7163/api/publisher", publisher);
        if (response != null)
        {
            Publishers = (await response.Content
                .ReadFromJsonAsync<ServiceResponse<List<Publisher>>>()).Data;
            OnChange.Invoke();
            return;
        }
        else Publishers = new List<Publisher>();
    }

    public Publisher CreateNewPublisher()
    {
        var newPublisher = new Publisher { New = true, Editing = true };

        Publishers.Insert(0, newPublisher);
        OnChange.Invoke();

        return newPublisher;
    }
}
