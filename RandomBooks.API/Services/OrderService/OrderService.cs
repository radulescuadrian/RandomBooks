namespace RandomBooks.API.Services.OrderService;

public class OrderService : IOrderService
{
    private readonly DataContext _ctx;
    private readonly ICartService _cartService;
    private readonly IAuthService _authService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public OrderService(DataContext ctx, ICartService cartService, IAuthService authService, IHttpContextAccessor httpContextAccessor)
    {
        _ctx = ctx;
        _cartService = cartService;
        _authService = authService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ServiceResponse<List<OrdersResponse>>> GetOrders()
    {
        List<Order> orders = new List<Order>();
        if (_httpContextAccessor.HttpContext.User.IsInRole("Admin"))
        {
            orders = await _ctx.Orders
            .Include(x => x.Address)
            .Include(x => x.Payment)
            .Include(x => x.Books)
            .ThenInclude(x => x.Book)
            .OrderByDescending(x => x.DatePlaced)
            .ToListAsync();
        }
        else if (_httpContextAccessor.HttpContext.User.IsInRole("Customer"))
        {
            orders = await _ctx.Orders
            .Include(x => x.Address)
            .Include(x => x.Payment)
            .Include(x => x.Books)
            .ThenInclude(x => x.Book)
            .Include(x => x.Books)
            .ThenInclude(x => x.BookType)
            .Where(x => x.UserId == _authService.GetUserId())
            .OrderByDescending(x => x.DatePlaced)
            .ToListAsync();
        }
        else if (_httpContextAccessor.HttpContext.User.IsInRole("Courier"))
        {
            orders = await _ctx.Orders
            .Include(x => x.Address)
            .Include(x => x.Payment)
            .Include(x => x.Books)
            .ThenInclude(x => x.Book)
            .Where(x => x.StatusId == 3)
            .OrderBy(x => x.DatePlaced)
            .ToListAsync();
        }
        else return new ServiceResponse<List<OrdersResponse>>
        {
            Success = false,
            Message = "User is not logged in"
        };

        var orderResponse = new List<OrdersResponse>();
        foreach (var order in orders)
        {
            var status = await _ctx.Statuses.FirstOrDefaultAsync(x => x.Id == order.StatusId);
            if (status is null)
                return new ServiceResponse<List<OrdersResponse>>
                {
                    Success = false,
                    Message = "No status was found on an order"
                };

            var books = new List<OrderBook>();
            var itemText = string.Empty;
            if(order.Books.Count < 5)
            {
                for (int i = 0; i < order.Books.Count; i++)
                    books.Add(new OrderBook
                    {
                        Title = order.Books[i].Book.Title,
                        Type = order.Books[i].BookType.Name,
                        Quantity = order.Books[i].Quantity,
                        Price = order.Books[i].TotalPrice
                    });
            }
            else
            {
                for (int i = 0; i < 5; i++)
                    books.Add(new OrderBook
                    {
                        Title = order.Books[i].Book.Title,
                        Type = order.Books[i].BookType.Name,
                        Quantity = order.Books[i].Quantity,
                        Price = order.Books[i].TotalPrice
                    });
            }

            if (order.Books.Count > 5)
                itemText = $"and {order.Books.Count - 5} more...";


            orderResponse.Add(new OrdersResponse
            {
                Id = order.Id,
                DatePlaced = order.DatePlaced,
                LastModified = order.LastModified,
                Total = order.Total,
                Books = books,
                ItemsText = itemText,
                ProductsCount = order.Books.Count,
                Address = new OrderAddress
                {
                    Country = order.Address.Country,
                    City = order.Address.City,
                    Street = order.Address.Street,
                    Zip = order.Address.Zip
                },
                Payment = order.Payment is null ?
                    "On Delivery" :
                    $"**** **** **** {order.Payment.Number.Substring(order.Payment.Number.Length - 4)}",
                Notes = order.Notes,
                Rating = order.Rating,
                Status = status.Name
            });
        }

        return new ServiceResponse<List<OrdersResponse>> { Data = orderResponse };
    }

    public async Task<ServiceResponse<bool>> PlaceOrder(OrderRequest request)
    {
        if (request.UserId == null)
            return new ServiceResponse<bool>
            {
                Success = false,
                Message = "No user specified."
            };

        if (request.Address == null)
            return new ServiceResponse<bool>
            {
                Success = false,
                Message = "Need a delivery address."
            };

        decimal total = 0;
        var books = (await _cartService.GetDbCartProducts(request.UserId)).Data;
        if (books is null)
            return new ServiceResponse<bool>
            {
                Success = false,
                Message = "The cart is empty."
            };

        var orderItems = new List<OrderItem>();
        books.ForEach(x => total += x.Price * x.Quantity);
        books.ForEach(x => orderItems.Add(new OrderItem
        {
            BookId = x.BookId,
            BookTypeId = x.BookTypeId,
            Quantity = x.Quantity,
            TotalPrice = x.Price * x.Quantity
        }));

        foreach (var book in books)
        {
            var bookVariant = await _ctx.BookVariants
                .FirstOrDefaultAsync(x => x.BookId == book.BookId && 
                                          x.BookTypeId == book.BookTypeId);
            if (bookVariant == null)
                return new ServiceResponse<bool>
                {
                    Success = false,
                    Message = "A variant was not found"
                };

            bookVariant.Stock -= book.Quantity;
        }

        var today = DateTime.Now;
        var order = new Order
        {
            UserId = (int)request.UserId,
            DatePlaced = today,
            LastModified = today,
            Total = total,
            Books = orderItems,
            Notes = request.Notes
        };

        if (request.Address.Id == 0)
            order.Address = new Address
            {
                Country = request.Address.Country,
                City = request.Address.City,
                Street = request.Address.Street,
                Zip = request.Address.Zip
            };
        else order.AddressId = request.Address.Id;

        if (request.Card != null)
        {
            if (request.Card?.Id == 0)
                order.Payment = new CustomerCard
                {
                    Number = request.Card.Number,
                    Name = request.Card.Name,
                    ExpirationMonth = request.Card.ExpirationMonth,
                    ExpirationYear = request.Card.ExpirationYear
                };
            else order.PaymentId = request.Card.Id;
        }

        _ctx.Orders.Add(order);

        _ctx.CartItems.RemoveRange(
            _ctx.CartItems.Where(x => x.UserId == request.UserId));

        await _ctx.SaveChangesAsync();

        return new ServiceResponse<bool> { Data = true };
    }
}
