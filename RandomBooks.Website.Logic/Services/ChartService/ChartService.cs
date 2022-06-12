namespace RandomBooks.Website.Logic.Services.ChartService;

public class ChartService : IChartService
{
    private readonly HttpClient _http;

    public ChartService(HttpClient http)
    {
        _http = http;
    }

    public async Task<ServiceResponse<List<OrderCounter>>> GetLastMonthOrders()
    {
        var response = await _http.GetFromJsonAsync<ServiceResponse<List<OrderCounter>>>("https://localhost:7163/api/chart/last-month-orders");
        return response;
    }
}
