namespace RandomBooks.API.Services.ChartService;

public interface IChartService
{
    Task<ServiceResponse<List<OrderCounter>>> LastMonthOrders();
}