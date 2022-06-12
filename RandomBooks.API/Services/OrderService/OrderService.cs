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

    public async Task<ServiceResponse<bool>> RateOrder(OrderRating rating)
    {
        var order = await _ctx.Orders.FirstOrDefaultAsync(x => x.Id == rating.OrderId);
        if (order is null)
            return new ServiceResponse<bool>
            {
                Success = false,
                Message = "Order not found"
            };

        order.Rating = rating.Rating;

        await _ctx.SaveChangesAsync();

        return new ServiceResponse<bool> { Data = true };
    }

    public async Task<ServiceResponse<OrdersResponse>> GetOrder(int orderId)
    {
        Order order = new Order();
        if (_httpContextAccessor.HttpContext.User.IsInRole("Admin"))
        {
            order = await _ctx.Orders
            .Include(x => x.Address)
            .Include(x => x.Payment)
            .Include(x => x.Books)
            .ThenInclude(x => x.Book)
            .Include(x => x.Books)
            .ThenInclude(x => x.BookType)
            .FirstOrDefaultAsync(x => x.Id == orderId);
        }
        else if (_httpContextAccessor.HttpContext.User.IsInRole("Customer"))
        {
            order = await _ctx.Orders
            .Include(x => x.Address)
            .Include(x => x.Payment)
            .Include(x => x.Books)
            .ThenInclude(x => x.Book)
            .Include(x => x.Books)
            .ThenInclude(x => x.BookType)
            .FirstOrDefaultAsync(x => x.Id == orderId);
        }
        else if (_httpContextAccessor.HttpContext.User.IsInRole("Courier"))
        {
            order = await _ctx.Orders
            .Include(x => x.Address)
            .Include(x => x.Payment)
            .Include(x => x.Books)
            .ThenInclude(x => x.Book)
            .Where(x => x.StatusId == 3)
            .FirstOrDefaultAsync(x => x.Id == orderId);
        }
        else return new ServiceResponse<OrdersResponse>
        {
            Success = false,
            Message = "User is not logged in"
        };

        if (order is null)
            return new ServiceResponse<OrdersResponse>
            {
                Success = false,
                Message = "Order not found"
            };

        if (_httpContextAccessor.HttpContext.User.IsInRole("Customer") && order.UserId != _authService.GetUserId())
            return new ServiceResponse<OrdersResponse>()
            {
                Success = false,
                Message = "Cannot get details about other's orders"
            };

        var status = await _ctx.Statuses.FirstOrDefaultAsync(x => x.Id == order.StatusId);
        if (status is null)
            return new ServiceResponse<OrdersResponse>
            {
                Success = false,
                Message = "No status was found on an order"
            };

        var books = new List<OrderBook>();
        var itemText = string.Empty;
        if (order.Books.Count <= 5)
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
            if (_httpContextAccessor.HttpContext.User.IsInRole("Admin"))
                order.Books.ForEach(x => books.Add(new OrderBook
                {
                    Title = x.Book.Title,
                    Type = x.BookType.Name,
                    Quantity = x.Quantity,
                    Price = x.TotalPrice
                }));
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

                itemText = $"and {order.Books.Count - 5} more...";
            }
        }

        var temp = new OrdersResponse
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
            Status = status.Name,
            StatusId = order.StatusId
        };

        if (_httpContextAccessor.HttpContext.User.IsInRole("Admin"))
        {
            var user = await _ctx.Users
                .FirstOrDefaultAsync(x => x.Id == order.UserId);
            if (user is null)
                return new ServiceResponse<OrdersResponse>
                {
                    Success = false,
                    Message = "No user was assigned to this order"
                };

            temp.User = user.Username;
        }

        return new ServiceResponse<OrdersResponse> { Data = temp };
    }

    public async Task<ServiceResponse<List<OrdersResponse>>> GetOrders()
    {
        List<Order> orders = new List<Order>();
        if (_httpContextAccessor.HttpContext.User.IsInRole("Customer"))
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
        else if (_httpContextAccessor.HttpContext.User.IsInRole("Employee"))
        {
            orders = await _ctx.Orders
            .Include(x => x.Address)
            .Include(x => x.Payment)
            .Include(x => x.Books)
            .ThenInclude(x => x.Book)
            .Where(x => x.StatusId == 1)
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

            var temp = new OrdersResponse
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
            };

            if (_httpContextAccessor.HttpContext.User.IsInRole("Admin"))
            {
                var user = await _ctx.Users
                    .FirstOrDefaultAsync(x => x.Id == order.UserId);
                if (user is null)
                    return new ServiceResponse<List<OrdersResponse>>
                    {
                        Success = false,
                        Message = "No user was assigned to this order"
                    };

                temp.User = user.Username;
            }

            orderResponse.Add(temp);
        }

        return new ServiceResponse<List<OrdersResponse>> { Data = orderResponse };
    }

    public async Task<ServiceResponse<OrderListResult>> GetAdminOrders(int page)
    {
        var orders = await _ctx.Orders
            .Include(x => x.Address)
            .Include(x => x.Payment)
            .Include(x => x.Books)
            .ThenInclude(x => x.Book)
            .OrderByDescending(x => x.DatePlaced)
            .ToListAsync();

        var results = 5;
        var orderResponse = new List<OrdersResponse>();
        foreach (var order in orders)
        {
            var status = await _ctx.Statuses.FirstOrDefaultAsync(x => x.Id == order.StatusId);
            if (status is null)
                return new ServiceResponse<OrderListResult>
                {
                    Success = false,
                    Message = "No status was found on an order"
                };

            var user = await _ctx.Users
                    .FirstOrDefaultAsync(x => x.Id == order.UserId);
            if (user is null)
                return new ServiceResponse<OrderListResult>
                {
                    Success = false,
                    Message = "No user was assigned to this order"
                };

            orderResponse.Add(new OrdersResponse
            {
                Id = order.Id,
                DatePlaced = order.DatePlaced,
                LastModified = order.LastModified,
                Total = order.Total,
                ProductsCount = order.Books.Count,
                Status = status.Name,
                User = user.Username,
                Rating = order.Rating
            });
        }
        var pageCount = Math.Ceiling(orderResponse.Count / (double)results);

        return new ServiceResponse<OrderListResult>
        {
            Data = new OrderListResult
            {
                Orders = orderResponse.Skip((page - 1) * results).Take(results).ToList(),
                Page = page,
                Pages = (int)pageCount
            }
        };
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
