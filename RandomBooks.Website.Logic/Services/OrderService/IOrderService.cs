namespace RandomBooks.Website.Logic.Services.OrderService
{
    public interface IOrderService
    {
        int Page { get; set; }
        int PageCount { get; set; }
        string Message { get; set; }
        List<OrdersResponse> Orders { get; set; }

        event Action OnChange;

        Task GetAdminOrders();
        Task<OrdersResponse> GetOrder(int orderId);
        Task<List<OrdersResponse>> GetOrders();
        void InitializePages();
        Task<string> PlaceOrder();
        Task<bool> RateOrder(int orderId, int rating);
    }
}