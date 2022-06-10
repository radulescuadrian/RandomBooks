namespace RandomBooks.Website.Logic.Services.OrderService
{
    public interface IOrderService
    {
        event Action OnChange;

        Task<List<OrdersResponse>> GetOrders();
        Task<string> PlaceOrder();
    }
}