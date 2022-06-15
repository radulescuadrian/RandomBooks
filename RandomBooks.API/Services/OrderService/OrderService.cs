using IronPdf;

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

    public async Task<ServiceResponse<byte[]>> GetInvoice(int orderId)
    {
        //if (!_httpContextAccessor.HttpContext.User.IsInRole("Admin"))
        //    return new ServiceResponse<byte[]>
        //    {
        //        Success = false,
        //        Message = "Unauthorized"
        //    };

        var order = await _ctx.Orders
            .Include(x => x.Address)
            .Include(x => x.Payment)
            .Include(x => x.Books)
            .ThenInclude(x => x.Book)
            .Include(x => x.Books)
            .ThenInclude(x => x.BookType)
            .FirstOrDefaultAsync(x => x.Id == orderId);
        if (order is null)
            return new ServiceResponse<byte[]>
            {
                Success = false,
                Message = "Order not found"
            };

        var status = await _ctx.Statuses.FirstOrDefaultAsync(x => x.Id == order.StatusId);
        if (status is null)
            return new ServiceResponse<byte[]>
            {
                Success = false,
                Message = "No status was found on an order"
            };

        var books = new List<OrderBook>();
        var itemText = string.Empty;
        order.Books.ForEach(x => books.Add(new OrderBook
        {
            Title = x.Book.Title,
            Type = x.BookType.Name,
            Quantity = x.Quantity,
            Price = x.TotalPrice
        }));

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
            StatusId = order.StatusId
        };

        var user = await _ctx.Users
                .Include(x => x.CustomerDetails)
                .FirstOrDefaultAsync(x => x.Id == order.UserId);
        if (user is null)
            return new ServiceResponse<byte[]>
            {
                Success = false,
                Message = "No user was assigned to this order"
            };

        temp.FullUser = user;

        var html = $@"
        <!DOCTYPE html>
        <html lang=""en"">
        <head>
            <link href=""https://cdn.jsdelivr.net/npm/bootstrap@5.2.0-beta1/dist/css/bootstrap.min.css"" rel=""stylesheet"" integrity=""sha384-0evHe/X+R7YkIZDRvuzKMRqM+OrBnVFBL6DOitfPri4tjfHxaWutUpFmBp4vmVor"" crossorigin=""anonymous"">
            <link rel=""stylesheet"" href=""https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.1.1/css/all.min.css"" integrity=""sha512-KfkfwYDsLkIlwQp6LFnl8zNdLGxu9YAA1QvwINks4PhcElQSvqcyVLLD9aMhXd13uQjoXtEKNosOWaZqXgel0g=="" crossorigin=""anonymous"" referrerpolicy=""no-referrer"" />
            <script src=""https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/js/bootstrap.bundle.min.js"" integrity=""sha384-MrcW6ZMFYlzcLA8Nl+NtUVF0sA7MsXsP1UyJoMp4YLEuNSfAP+JcXn/tWtIaxVXM"" crossorigin=""anonymous""></script>
        </head>
        <body>
            <div class=""d-flex p-4 pt-0"">
                <p class=""text-black my-2"" style=""font-size: 45px;"">
                    <i class=""fas fa-book-open""></i> Random Books
                </p>

                <div class=""ms-auto d-flex"">
                    <div class=""fw-bold"" style=""display: table;"">
                        <p class=""mb-0"" style=""display: table-cell; vertical-align: bottom; padding-top: 30px; font-size: 22px;"">Order #</p>
                    </div>
                    <div class=""ms-2"" style=""display: table;"">
                        <p class=""mb-0"" style=""display: table-cell; vertical-align: bottom; padding-top: 30px; font-size: 22px;"">1002</p>
                    </div>
                </div>
            </div>
		<div class=""d-flex px-4"">
                <div>
			  <h3 class=""fw-bold"">Date Placed</h3>
                    <p class=""mb-0 pt-1"">10 June 2022</p>
                </div>
		    <div class=""ms-auto"">
                    <h3 class=""fw-bold"">Date Delivered</h3>
                    <p class=""mb-0 pt-1"">-</p>
                </div>
            </div>
            <div class=""d-flex p-4 pt-4"">
                <div>
                    <h3 class=""fw-bold"">Delivery Address</h3>
                    <div class=""ms-auto d-flex"">
                        <div class=""fw-bold"">
                            <p class=""mb-0"">Country</p>
                            <p class=""mb-0"">City</p>
                            <p class=""mb-0"">Street</p>
                            <p class=""mb-0"">Zip</p>
                        </div>
                        <div class=""ms-2"">
                            <p class=""mb-0"">{temp.Address.Country}</p>
                            <p class=""mb-0"">{temp.Address.City}</p>
                            <p class=""mb-0"">{temp.Address.Street}</p>
                            <p class=""mb-0"">{temp.Address.Zip}</p>
                        </div>
                    </div>
                </div>
                <div class=""ms-auto"">
                    <h3 class=""fw-bold"">Delivery For</h3>
                    <div class=""ms-auto"">
                        <p class=""mb-0"">{temp.FullUser.CustomerDetails.Lastname} {temp.FullUser.CustomerDetails.Firstname}</p>
                        <p class=""mb-0"">{temp.FullUser.CustomerDetails.Email}</p>
                        <p class=""mb-0"">{temp.FullUser.CustomerDetails.PhoneNumber}</p>
                    </div>
                </div>
            </div>
            <div class=""m-4 mb-0 p-1 ps-2"" style=""background: #CFCFCF; border-radius: 5px;"">
                <p class=""mb-0 fw-bold"">Products</p>
            </div>
            <table class=""mb-0 mt-0 mx-auto"" style=""width: 550px;"">
                <thead>
                    <tr class=""my-1"">
                        <td style=""width: 200px;""></td>
                        <td></td>
                        <td style=""text-align: center;"">Quantity</td>
                        <td style=""text-align: right; width: 70px;"">Price</td>
                    </tr>
                </thead>
                <tbody>";
        for(int i = 0; i<order.Books.Count(); i++)
        {
            html += $@"
                    <tr style=""background: {(i % 2 != 0 ? "#EFEFEF" : "White")};"">
                        <td><p class=""mb-0"" style=""width: 200px; word-wrap: break-word;"">{temp.Books[i].Title}</p></td>
                        <td><p class=""mx-3 mb-0"">{temp.Books[i].Type}</p></td>
                        <td style=""text-align: center;"">{temp.Books[i].Quantity}</td>
                        <td class=""d-flex"">
                            $
                            <p class=""mb-0 ms-auto"">{temp.Books[i].Price}</p>
                        </td>
                    </tr>";
        }
        html += $@"
                </tbody>
            </table>

            <div class=""m-4 mb-0 p-1 ps-2"" style=""background: #CFCFCF; border-radius: 5px;"">
                <div class=""d-flex"">
                    <p class=""mb-0 me-2 fw-bold"">Payment</p>
                    <p class=""mb-0"">{(temp.Payment == "On Delivery" ? "CASH" : "CARD")}</p>
                    <p class=""mb-0 ms-auto me-2 fw-bold"">Total:</p>
                    <p class=""mb-0 me-1"">$ {temp.Total}</p>
                </div>
            </div>

            <div class=""m-4 mb-0 p-1 ps-2"" style=""background: #CFCFCF; border-radius: 5px; margin-top: 80px !important;"">
                <p class=""mb-0 fw-bold"">Notes</p>
            </div>
            <div class=""m-4 my-0 p-1 ps-2"">
                <p class=""mb-0"" style=""word-wrap: break-word;"">{temp.Notes}</p>
            </div>
        </body>
        </html>
        ";

        var Renderer = new ChromePdfRenderer();
        using var PDF = Renderer.RenderHtmlAsPdf(html);
        using var stream = PDF.Stream;

        try
        {
            return new ServiceResponse<byte[]> { Data = stream.ToArray() };
        }
        catch (Exception ex)
        {
            return new ServiceResponse<byte[]> 
            { 
                Success = false, 
                Message = "Failed generating PDF"
            };
        }
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

            user = await _ctx.Users
                .FirstOrDefaultAsync(x => x.Id == order.EmployeeId);
            if (user is null)
                temp.Employee = "none";
            else temp.Employee = user.Username;

            user = await _ctx.Users
                .FirstOrDefaultAsync(x => x.Id == order.CourierId);
            if (user is null)
                temp.Courier = "none";
            else temp.Courier = user.Username;
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
