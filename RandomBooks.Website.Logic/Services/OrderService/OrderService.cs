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
    public string Message { get; set; } = "Loading orders...";
    public int Page { get; set; } = 1;
    public int PageCount { get; set; } = 0;

    public OrderService(HttpClient http, ICartService cartService, IAuthService authService, NavigationManager navigationManager)
    {
        _http = http;
        _cartService = cartService;
        _authService = authService;
        _navigationManager = navigationManager;
    }

    public void InitializePages()
    {
        if (Orders != null)
            Orders.Clear();
        Page = 1;
        PageCount = 0;
    }

    public async Task<OrdersResponse> GetOrder(int orderId)
    {
        var result = await _http.GetFromJsonAsync<ServiceResponse<OrdersResponse>>($"https://localhost:7163/api/order/{orderId}");
        return result.Data;
    }

    public async Task<List<OrdersResponse>> GetOrders()
    {
        var result = await _http.GetFromJsonAsync<ServiceResponse<List<OrdersResponse>>>("https://localhost:7163/api/order");
        return result.Data;
    }

    public async Task GetAdminOrders()
    {
        var result = await _http.GetFromJsonAsync<ServiceResponse<OrderListResult>>($"https://localhost:7163/api/order/admin/{Page}");
        if (result != null && result.Data != null)
        {
            Orders = result.Data.Orders;
            Page = result.Data.Page;
            PageCount = result.Data.Pages;
        }

        if (Orders.Count == 0)
            Message = "No orders found";

        OnChange?.Invoke();
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

    public async Task<bool> RateOrder(int orderId, int rating)
    {
        var orderRating = new OrderRating
        {
            OrderId = orderId,
            Rating = rating
        };

        var result = await _http.PostAsJsonAsync($"https://localhost:7163/api/order/rating", orderRating);
        if (result.IsSuccessStatusCode)
            return true;
        return false;
    }
}
