namespace RandomBooks.Website.Logic.Services.ChartService;

public interface IChartService
{
    Task<ServiceResponse<List<OrderCounter>>> GetLastMonthOrders();
}