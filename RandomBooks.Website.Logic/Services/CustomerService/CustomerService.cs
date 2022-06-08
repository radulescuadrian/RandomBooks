namespace RandomBooks.Website.Logic.Services.CustomerService;

public class CustomerService : ICustomerService
{
    public CustomerDetails User { get; set; }
    public CustomerDetails TempUser { get; set; }
    public string Image { get; set; }
    public string TempImage { get; set; }

    private readonly HttpClient _http;
    public event Action OnChange;

    public CustomerService(HttpClient http)
    {
        _http = http;
    }

    public async Task GetProfile()
    {
        var response = await _http.GetFromJsonAsync<ServiceResponse<CustomerDetails>>("https://localhost:7163/api/user/profile");
        if (response.Data != null)
            User = response.Data;
        TempUser = User.Clone();

        if (User.Image is null)
            Image = "user.svg";
        else Image = User.Image.Data;
        TempImage = Image;

        OnChange?.Invoke();
    }

    public async Task<string> UpdateProfile(CustomerDetails user, string image)
    {
        var edit = new ProfileEdit
        {
            User = user,
            NewImage = image
        };

        var response = await _http.PutAsJsonAsync("https://localhost:7163/api/user/profile", edit);

        if (response.IsSuccessStatusCode)
            return (await response.Content
                .ReadFromJsonAsync<ServiceResponse<string>>()).Data;

        return (await response.Content
                .ReadFromJsonAsync<ServiceResponse<string>>()).Message;
    }

    #region Addresses
    public Address CreateNewAddress()
    {
        var address = User.Addresses.FirstOrDefault(x => x.New);
        if (address != null)
            return new Address { New = true, Editing = true };

        var newAddress = new Address { New = true, Editing = true };

        User.Addresses.Insert(0, newAddress);
        OnChange.Invoke();

        return newAddress;
    }

    public async Task<string> AddAddress(Address address)
    {
        var response = await _http.PostAsJsonAsync("https://localhost:7163/api/user/address", address);
        if (response.IsSuccessStatusCode)
        {
            await GetProfile();

            return (await response.Content
                .ReadFromJsonAsync<ServiceResponse<string>>()).Data;
        }

        return (await response.Content
                .ReadFromJsonAsync<ServiceResponse<string>>()).Message;
    }

    public async Task<string> UpdateAddress(Address address)
    {
        var response = await _http.PutAsJsonAsync("https://localhost:7163/api/user/address", address);
        if (response.IsSuccessStatusCode)
        {
            await GetProfile();

            return (await response.Content
                .ReadFromJsonAsync<ServiceResponse<string>>()).Data;
        }

        return (await response.Content
                .ReadFromJsonAsync<ServiceResponse<string>>()).Message;
    }

    public async Task<string> DeleteAddress(int addressId)
    {
        var response = await _http.DeleteAsync($"https://localhost:7163/api/user/address/{addressId}");
        if (response.IsSuccessStatusCode)
        {
            await GetProfile();

            return (await response.Content
                .ReadFromJsonAsync<ServiceResponse<string>>()).Data;
        }

        return (await response.Content
                .ReadFromJsonAsync<ServiceResponse<string>>()).Message;
    }
    #endregion

    #region Cards
    public CustomerCard CreateNewCard()
    {
        var card = User.Cards.FirstOrDefault(x => x.New);
        if (card != null)
            return new CustomerCard { New = true, Editing = true };

        var newCard = new CustomerCard { New = true, Editing = true };

        User.Cards.Insert(0, newCard);
        OnChange.Invoke();

        return newCard;
    }

    public async Task<string> AddCard(CustomerCard card)
    {
        var response = await _http.PostAsJsonAsync("https://localhost:7163/api/user/card", card);
        if (response.IsSuccessStatusCode)
        {
            await GetProfile();

            return (await response.Content
                .ReadFromJsonAsync<ServiceResponse<string>>()).Data;
        }

        return (await response.Content
                .ReadFromJsonAsync<ServiceResponse<string>>()).Message;
    }

    public async Task<string> UpdateCard(CustomerCard card)
    {
        var response = await _http.PutAsJsonAsync("https://localhost:7163/api/user/card", card);
        if (response.IsSuccessStatusCode)
        {
            await GetProfile();

            return (await response.Content
                .ReadFromJsonAsync<ServiceResponse<string>>()).Data;
        }

        return (await response.Content
                .ReadFromJsonAsync<ServiceResponse<string>>()).Message;
    }

    public async Task<string> DeleteCard(int cardId)
    {
        var response = await _http.DeleteAsync($"https://localhost:7163/api/user/card/{cardId}");
        if (response.IsSuccessStatusCode)
        {
            await GetProfile();

            return (await response.Content
                .ReadFromJsonAsync<ServiceResponse<string>>()).Data;
        }

        return (await response.Content
                .ReadFromJsonAsync<ServiceResponse<string>>()).Message;
    }
    #endregion
}
