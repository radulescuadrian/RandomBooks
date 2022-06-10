using Microsoft.AspNetCore.Components;
using RandomBooks.Website.Logic.Services.AuthService;
using RandomBooks.Website.Logic.Services.CartService;

namespace RandomBooks.Website.Logic.Services.OrderService;

public class OrderService : IOrderService
{
    private readonly HttpClient _http;
    private readonly ICartService _cartService;
    private readonly IAuthService _authService;
    private readonly NavigationManager _navigationManager;

    public List<OrdersResponse> Orders { get; set; }

    public event Action OnChange;

    public OrderService(HttpClient http, ICartService cartService, IAuthService authService, NavigationManager navigationManager)
    {
        _http = http;
        _cartService = cartService;
        _authService = authService;
        _navigationManager = navigationManager;
    }

    public async Task<List<OrdersResponse>> GetOrders()
    {
        var result = await _http.GetFromJsonAsync<ServiceResponse<List<OrdersResponse>>>($"https://localhost:7163/api/order");
        return result.Data;
    }

    public async Task<string> PlaceOrder()
    {
        var order = new OrderRequest
        {
            UserId = await _authService.GetUserId(),
            Address = _cartService.SelectedAddress,
            Card = _cartService.SelectedCard,
            Notes = _cartService.Notes
        };

        var response = await _http.PostAsJsonAsync("https://localhost:7163/api/order", order);
        if (response.IsSuccessStatusCode)
            return "OK";

        return (await response.Content
                .ReadFromJsonAsync<ServiceResponse<bool>>()).Message;
    }
}
