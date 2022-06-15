namespace RandomBooks.API.Services.OrderService
{
    public interface IOrderService
    {
        Task<ServiceResponse<OrderListResult>> GetAdminOrders(int page);
        Task<ServiceResponse<byte[]>> GetInvoice(int orderId);
        Task<ServiceResponse<OrdersResponse>> GetOrder(int orderId);
        Task<ServiceResponse<List<OrdersResponse>>> GetOrders();
        Task<ServiceResponse<bool>> PlaceOrder(OrderRequest request);
        Task<ServiceResponse<bool>> RateOrder(OrderRating rating);
    }
}