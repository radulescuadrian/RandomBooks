namespace RandomBooks.API.Services.OrderService
{
    public interface IOrderService
    {
        Task<ServiceResponse<List<OrdersResponse>>> GetOrders();
        Task<ServiceResponse<bool>> PlaceOrder(OrderRequest request);
    }
}