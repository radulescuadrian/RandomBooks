namespace RandomBooks.API.Services.ChartService;

public class ChartService : IChartService
{
    private readonly DataContext _ctx;

    public ChartService(DataContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<ServiceResponse<List<OrderCounter>>> LastMonthOrders()
    {
        var allOrders = await _ctx.Orders
            .Include(x => x.Books)
            .Where(x => x.DatePlaced >= DateTime.Now.AddDays(-31))
            .ToListAsync();

        var ordersDataset = new List<OrderCounter>();

        for (int i = -9; i <= 0; i++)
        {
            var ordersCount = allOrders.Where(x => x.DatePlaced.Date == DateTime.Now.AddDays(i).Date)
                .Count();

            ordersDataset.Add(new OrderCounter
            {
                Orders = ordersCount,
                Date = DateTime.Now.AddDays(i).Date
            });
        }

        return new ServiceResponse<List<OrderCounter>> { Data = ordersDataset };
    }
}
