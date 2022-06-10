using Blazored.LocalStorage;
using RandomBooks.Website.Logic.Services.AuthService;

namespace RandomBooks.Website.Logic.Services.CartService;

public class CartService : ICartService
{
    private readonly ILocalStorageService _localStorage;
    private readonly HttpClient _http;
    private readonly IAuthService _authService;

    public Address SelectedAddress { get; set; }
    public CustomerCard? SelectedCard { get; set; }
    public string? Notes { get; set; }

    public event Action OnChange;

    public CartService(ILocalStorageService localStorage, HttpClient http, IAuthService authService)
    {
        _localStorage = localStorage;
        _http = http;
        _authService = authService;
    }

    public async Task AddToCart(CartItem cartItem)
    {
        if (await _authService.IsUserAuthenticated())
            await _http.PostAsJsonAsync("https://localhost:7163/api/cart/add", cartItem);
        else
        {
            var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
            if (cart == null)
                cart = new List<CartItem>();

            var sameItem = cart.Find(x => x.BookId == cartItem.BookId &&
                                          x.BookTypeId == cartItem.BookTypeId);

            if (sameItem == null)
                cart.Add(cartItem);
            else sameItem.Quantity += cartItem.Quantity;

            await _localStorage.SetItemAsync("cart", cart);
        }

        await GetNumberOfItems();
    }

    public async Task RemoveBook(int bookId, int bookTypeId)
    {
        if (await _authService.IsUserAuthenticated())
            await _http.DeleteAsync($"https://localhost:7163/api/cart/{bookId}/{bookTypeId}");
        else
        {
            var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
            if (cart == null)
                return;

            var cartItem = cart.Find(x => x.BookId == bookId &&
                                     x.BookTypeId == bookTypeId);
            if (cartItem != null)
            {
                cart.Remove(cartItem);
                await _localStorage.SetItemAsync("cart", cart);
            }
        }
    }

    public async Task<List<CartProductResponse>> GetProducts()
    {
        if (await _authService.IsUserAuthenticated())
        {
            var response = await _http.GetFromJsonAsync<ServiceResponse<List<CartProductResponse>>>("https://localhost:7163/api/cart");
            return response.Data;
        }
        else
        {
            var cartItems = await _localStorage.GetItemAsync<List<CartItem>>("cart");
            if (cartItems == null)
                return new List<CartProductResponse>();

            var response = await _http.PostAsJsonAsync("https://localhost:7163/api/cart/products", cartItems);
            var cartProducts =
                await response.Content.ReadFromJsonAsync<ServiceResponse<List<CartProductResponse>>>();

            return cartProducts.Data;
        }
    }

    public async Task UpdateQuantity(CartProductResponse book)
    {
        if (await _authService.IsUserAuthenticated())
        {
            var request = new CartItem
            {
                BookId = book.BookId,
                Quantity = book.Quantity,
                BookTypeId = book.BookTypeId
            };

            await _http.PutAsJsonAsync("https://localhost:7163/api/cart/update-quantity", request);
        }
        else
        {
            var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
            if (cart == null)
                return;

            var cartItem = cart.Find(x => x.BookId == book.BookId &&
                                          x.BookTypeId == book.BookTypeId);
            if (cartItem != null)
            {
                cartItem.Quantity = book.Quantity;
                await _localStorage.SetItemAsync("cart", cart);
            }
        }
    }

    public async Task StoreItems(bool emptyLocalCart = false)
    {
        var localCart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
        if (localCart == null)
            return;

        await _http.PostAsJsonAsync("https://localhost:7163/api/cart", localCart);

        if (emptyLocalCart)
            await _localStorage.RemoveItemAsync("cart");
    }

    public async Task GetNumberOfItems()
    {
        if (await _authService.IsUserAuthenticated())
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<int>>("https://localhost:7163/api/cart/count");
            var count = result.Data;

            await _localStorage.SetItemAsync<int>("cartItems", count);
        }
        else
        {
            var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
            await _localStorage.SetItemAsync<int>("cartItems", cart != null ? cart.Count : 0);
        }

        OnChange?.Invoke();
    }
}
