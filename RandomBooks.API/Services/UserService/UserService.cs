using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace RandomBooks.API.Services.UserService;

public class UserService : IUserService
{
    private readonly DataContext _ctx;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(DataContext ctx, IHttpContextAccessor httpContextAccessor)
    {
        _ctx = ctx;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ServiceResponse<User>> GetUser(int userId)
    {
        var user = new User();
        switch (await GetRole(userId))
        {
            case "Customer":
                user = await _ctx.Users
                    .Include(x => x.CustomerDetails)
                    .ThenInclude(x => x.Cards)
                    .Include(x => x.CustomerDetails)
                    .ThenInclude(x => x.Addresses)
                    .FirstOrDefaultAsync(x => x.Id == userId);
                break;
            case "Employee":
                user = await _ctx.Users
                    .Include(x => x.EmployeeDetails)
                    .FirstOrDefaultAsync(x => x.Id == userId);
                break;

            default:
                return new ServiceResponse<User>
                {
                    Success = false,
                    Message = "This user could not be found."
                };
        }

        return new ServiceResponse<User> { Data = user };
    }

    public async Task<ServiceResponse<CustomerDetailsResponse>> GetAdminCustomerDetails(int userId)
    {
        var user = await _ctx.Users.FirstOrDefaultAsync(x => x.Id == userId);
        if (user is null)
            return new ServiceResponse<CustomerDetailsResponse>
            {
                Success = false,
                Message = "User not found"
            };

        var details = await _ctx.CustomerDetails
            .Include(x => x.Addresses)
            .Include(x => x.Cards)
            .Include(x => x.Image)
            .FirstOrDefaultAsync(x => x.UserId == userId);

        var orders = await _ctx.Orders
            .Where(x => x.UserId == userId)
            .ToListAsync();

        var ordersResponse = new List<OrdersResponse>();
        if (orders.Count > 0)
            orders.ForEach(x => ordersResponse.Add(new OrdersResponse
            {
                Id = x.Id,
                Total = x.Total,
                DatePlaced = x.DatePlaced,
                Payment = x.Payment is null ? "CASH" : "CARD"
            }));

        return new ServiceResponse<CustomerDetailsResponse>
        {
            Data = new CustomerDetailsResponse
            {
                Username = user.Username,
                Firstname = details.Firstname,
                Lastname = details.Lastname,
                Email = details.Email,
                PhoneNumber = details.PhoneNumber,
                Image = details.Image is null ? null : details.Image.Data,
                Addresses = details.Addresses,
                NumberOfCards = details.Cards.Count(),
                DateJoined = details.JoinDate,
                Deactivated = user.Deactivated,
                Orders = ordersResponse
            }
        };
    }

    public async Task<ServiceResponse<EmployeeDetailsResponse>> GetAdminEmployeeDetails(int userId)
    {
        var user = await _ctx.Users.FirstOrDefaultAsync(x => x.Id == userId);
        if (user is null)
            return new ServiceResponse<EmployeeDetailsResponse>
            {
                Success = false,
                Message = "User not found"
            };

        var details = await _ctx.EmployeeDetails
            .Include(x => x.Image)
            .FirstOrDefaultAsync(x => x.UserId == userId);

        var orders = await _ctx.Orders
            .Where(x => x.EmployeeId == userId && x.StatusId == 3)
            .ToListAsync();

        var ordersResponse = new List<OrdersResponse>();
        if (orders.Count > 0)
            orders.ForEach(x => ordersResponse.Add(new OrdersResponse
            {
                Id = x.Id,
                Total = x.Total,
                DatePlaced = x.DatePlaced,
                Payment = x.Payment is null ? "CASH" : "CARD"
            }));

        return new ServiceResponse<EmployeeDetailsResponse>
        {
            Data = new EmployeeDetailsResponse
            {
                Username = user.Username,
                Firstname = details.Firstname,
                Lastname = details.Lastname,
                Email = details.Email,
                PhoneNumber = details.PhoneNumber,
                Image = details.Image is null ? null : details.Image.Data,
                DateJoined = details.HireDate,
                Deactivated = user.Deactivated,
                Orders = ordersResponse
            }
        };
    }

    public async Task<ServiceResponse<EmployeeDetailsResponse>> GetAdminCourierDetails(int userId)
    {
        var user = await _ctx.Users.FirstOrDefaultAsync(x => x.Id == userId);
        if (user is null)
            return new ServiceResponse<EmployeeDetailsResponse>
            {
                Success = false,
                Message = "User not found"
            };

        var details = await _ctx.EmployeeDetails
            .Include(x => x.Image)
            .FirstOrDefaultAsync(x => x.UserId == userId);

        var orders = await _ctx.Orders
            .Where(x => x.CourierId == userId && x.StatusId == 5)
            .ToListAsync();

        var ordersResponse = new List<OrdersResponse>();
        if (orders.Count > 0)
            orders.ForEach(x => ordersResponse.Add(new OrdersResponse
            {
                Id = x.Id,
                Total = x.Total,
                DatePlaced = x.DatePlaced,
                Payment = x.Payment is null ? "CASH" : "CARD"
            }));

        double rating = 0;
        int ratedOrders = 0;
        foreach (var order in orders)
            if(order.Rating != null)
            {
                rating += (int)order.Rating;
                ratedOrders++;
            }

        return new ServiceResponse<EmployeeDetailsResponse>
        {
            Data = new EmployeeDetailsResponse
            {
                Username = user.Username,
                Firstname = details.Firstname,
                Lastname = details.Lastname,
                Email = details.Email,
                PhoneNumber = details.PhoneNumber,
                Image = details.Image is null ? null : details.Image.Data,
                DateJoined = details.HireDate,
                Deactivated = user.Deactivated,
                Orders = ordersResponse,
                Rating = ratedOrders == 0 ? 0 : rating / ratedOrders
            }
        };
    }

    public async Task<ServiceResponse<CustomerListResult>> GetCustomers(int page)
    {
        var results = 6;
        var users = await _ctx.Users
            .Where(x => x.Role == "Customer")
            .Include(x => x.CustomerDetails)
            .OrderBy(x => x.Username)
            .ToListAsync();
        var pageCount = Math.Ceiling(users.Count / (double)results);

        if (users == null || users.Count == 0)
            return new ServiceResponse<CustomerListResult>
            {
                Success = false,
                Message = $"No customers found"
            };

        var customers = new List<CustomersResponse>();
        foreach (var customer in users)
        {
            var orders = await _ctx.Orders
                .Where(x => x.UserId == customer.Id)
                .ToListAsync();

            customers.Add(new CustomersResponse
            {
                Id = customer.Id,
                Username = customer.Username,
                DateJoined = customer.CustomerDetails.JoinDate,
                OrdersPlaced = orders.Count(),
                Deactivated = customer.Deactivated
            });
        }

        return new ServiceResponse<CustomerListResult>
        {
            Data = new CustomerListResult
            {
                Customers = customers.Skip((page - 1) * results).Take(results).ToList(),
                Page = page,
                Pages = (int)pageCount
            }
        };
    }

    public async Task<ServiceResponse<EmployeeListResult>> GetEmployees(int page)
    {
        var results = 6;
        var users = await _ctx.Users
            .Where(x => x.Role == "Employee")
            .Include(x => x.EmployeeDetails)
            .OrderBy(x => x.Username)
            .ToListAsync();
        var pageCount = Math.Ceiling(users.Count / (double)results);

        if (users == null || users.Count == 0)
            return new ServiceResponse<EmployeeListResult>
            {
                Success = false,
                Message = $"No employees found"
            };

        var employees = new List<EmployeesResponse>();
        foreach (var employee in users)
        {
            var orders = await _ctx.Orders
                .Where(x => x.EmployeeId == employee.Id && x.StatusId == 3)
                .ToListAsync();

            employees.Add(new EmployeesResponse
            {
                Id = employee.Id,
                Username = employee.Username,
                DateHired = employee.EmployeeDetails.HireDate,
                OrdersFulfilled = orders.Count(),
                Deactivated = employee.Deactivated
            });
        }

        return new ServiceResponse<EmployeeListResult>
        {
            Data = new EmployeeListResult
            {
                Employees = employees.Skip((page - 1) * results).Take(results).ToList(),
                Page = page,
                Pages = (int)pageCount
            }
        };
    }

    public async Task<ServiceResponse<CourierListResult>> GetCouriers(int page)
    {
        var results = 6;
        var users = await _ctx.Users
            .Where(x => x.Role == "Courier")
            .Include(x => x.EmployeeDetails)
            .OrderBy(x => x.Username)
            .ToListAsync();
        var pageCount = Math.Ceiling(users.Count / (double)results);

        if (users == null || users.Count == 0)
            return new ServiceResponse<CourierListResult>
            {
                Success = false,
                Message = $"No couriers found"
            };

        var couriers = new List<CouriersResponse>();
        foreach (var courier in users)
        {
            var orders = await _ctx.Orders
                .Where(x => x.CourierId == courier.Id && x.StatusId == 5)
                .ToListAsync();

            double rating = 0;
            var ratedOrders = 0;
            foreach (var order in orders)
            {
                if (order.Rating != null)
                {
                    rating += (int)order.Rating;
                    ratedOrders++;
                }
            }

            couriers.Add(new CouriersResponse
            {
                Id = courier.Id,
                Username = courier.Username,
                DateHired = courier.EmployeeDetails.HireDate,
                OrdersDelivered = orders.Count(),
                Rating = ratedOrders == 0 ? 0 : rating / ratedOrders,
                Deactivated = courier.Deactivated
            });
        }

        return new ServiceResponse<CourierListResult>
        {
            Data = new CourierListResult
            {
                Couriers = couriers.Skip((page - 1) * results).Take(results).ToList(),
                Page = page,
                Pages = (int)pageCount
            }
        };
    }

    public async Task<ServiceResponse<bool>> DeactivateUser(int userId)
    {
        var user = await _ctx.Users.FirstOrDefaultAsync(x => x.Id == userId);
        if (user is null)
            return new ServiceResponse<bool>
            {
                Success = false,
                Message = "User not found"
            };

        user.Deactivated = true;

        await _ctx.SaveChangesAsync();

        return new ServiceResponse<bool> { Data = true };
    }

    public async Task<ServiceResponse<bool>> ActivateUser(int userId)
    {
        var user = await _ctx.Users.FirstOrDefaultAsync(x => x.Id == userId);
        if (user is null)
            return new ServiceResponse<bool>
            {
                Success = false,
                Message = "User not found"
            };

        user.Deactivated = false;

        await _ctx.SaveChangesAsync();

        return new ServiceResponse<bool> { Data = true };
    }

    public async Task<ServiceResponse<bool>> AddEmployee(NewEmlpoyee employee)
    {
        try
        {
            var username = $"{employee.Firstname}.{employee.Lastname}";
            if (await _ctx.Users.AnyAsync(user => user.Username.Equals(username)))
                return new ServiceResponse<bool>
                {
                    Success = false,
                    Message = "User already exists"
                };

            CreatePasswordHash(username, out byte[] password);
            var role = char.ToUpper(employee.Role[0]) + employee.Role.Substring(1);

            await _ctx.Users.AddAsync(new User
            {
                Username = username,
                Password = password,
                Role = role,
                EmployeeDetails = new EmployeeDetails
                {
                    Firstname = employee.Firstname,
                    Lastname = employee.Lastname,
                    Email = $"{username}@company.com",
                    PhoneNumber = employee.PhoneNumber
                }
            });

            await _ctx.SaveChangesAsync();

            return new ServiceResponse<bool> { Data = true };
        }
        catch (Exception ex)
        {
            return new ServiceResponse<bool>
            {
                Success = false,
                Message = ex.Message
            };
        }
    }

    #region Profile Related
    public async Task<ServiceResponse<CustomerDetails>> GetProfile()
    {
        var userId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        var user = await _ctx.CustomerDetails
                    .Include(x => x.Cards)
                    .Include(x => x.Addresses)
                    .Include(x => x.Image)
                    .FirstOrDefaultAsync(x => x.UserId == userId);

        return new ServiceResponse<CustomerDetails> { Data = user };
    }

    public async Task<ServiceResponse<string>> UpdateProfile(ProfileEdit edit)
    {
        try
        {
            var userId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = await _ctx.CustomerDetails
                        .Include(x => x.Image)
                        .FirstOrDefaultAsync(x => x.UserId == userId);

            user.Firstname = edit.User.Firstname;
            user.Lastname = edit.User.Lastname;
            user.Email = edit.User.Email;
            user.PhoneNumber = edit.User.PhoneNumber;

            if (!string.IsNullOrWhiteSpace(edit.NewImage))
                if (user.Image != null)
                    user.Image.Data = edit.NewImage;
                else user.Image = new Blob
                {
                    Data = edit.NewImage,
                    Type = "picture",
                    Description = "customer"
                };

            await _ctx.SaveChangesAsync();

            return new ServiceResponse<string> { Data = "OK" };
        }
        catch (Exception ex)
        {
            return new ServiceResponse<string>
            {
                Success = false,
                Message = ex.Message
            };
        }
    }
    #endregion

    #region Addresses
    public async Task<ServiceResponse<string>> AddAddress(Address address)
    {
        if (!address.New)
            return new ServiceResponse<string>
            {
                Success = false,
                Message = "A new address is required"
            };

        var userId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        var user = await _ctx.CustomerDetails
                    .Include(x => x.Addresses)
                    .FirstOrDefaultAsync(x => x.UserId == userId);

        user.Addresses.Add(new Address
        {
            Country = address.Country,
            City = address.City,
            Street = address.Street,
            Zip = address.Zip
        });

        await _ctx.SaveChangesAsync();

        return new ServiceResponse<string> { Data = "OK" };
    }

    public async Task<ServiceResponse<string>> UpdateAddress(Address address)
    {
        var dbAddress = await _ctx.Addresses
                .FirstOrDefaultAsync(x => x.Id == address.Id);
        if (dbAddress == null)
            return new ServiceResponse<string>
            {
                Success = false,
                Message = "No address found with the specified ID"
            };

        dbAddress.Country = address.Country;
        dbAddress.City = address.City;
        dbAddress.Street = address.Street;
        dbAddress.Zip = address.Zip;

        await _ctx.SaveChangesAsync();

        return new ServiceResponse<string> { Data = "OK" };
    }

    public async Task<ServiceResponse<string>> DeleteAddress(int addressId)
    {
        var dbAddress = await _ctx.Addresses
                .FirstOrDefaultAsync(x => x.Id == addressId);
        if (dbAddress == null)
            return new ServiceResponse<string>
            {
                Success = false,
                Message = "No address found with the specified ID"
            };

        dbAddress.Deleted = true;

        await _ctx.SaveChangesAsync();

        return new ServiceResponse<string> { Data = "OK" };
    }
    #endregion

    #region Cards
    public async Task<ServiceResponse<string>> AddCard(CustomerCard card)
    {
        if (!card.New)
            return new ServiceResponse<string>
            {
                Success = false,
                Message = "A new address is required"
            };

        var userId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        var user = await _ctx.CustomerDetails
                    .Include(x => x.Cards)
                    .FirstOrDefaultAsync(x => x.UserId == userId);

        user.Cards.Add(new CustomerCard
        {
            Number = card.Number,
            Name = card.Name,
            ExpirationMonth = card.ExpirationMonth,
            ExpirationYear = card.ExpirationYear
        });

        await _ctx.SaveChangesAsync();

        return new ServiceResponse<string> { Data = "OK" };
    }

    public async Task<ServiceResponse<string>> UpdateCard(CustomerCard card)
    {
        var dbCard = await _ctx.CustomerCards
                .FirstOrDefaultAsync(x => x.Id == card.Id);
        if (dbCard == null)
            return new ServiceResponse<string>
            {
                Success = false,
                Message = "No card found with the specified ID"
            };

        dbCard.Number = card.Number;
        dbCard.Name = card.Name;
        dbCard.ExpirationMonth = card.ExpirationMonth;
        dbCard.ExpirationYear = card.ExpirationYear;

        await _ctx.SaveChangesAsync();

        return new ServiceResponse<string> { Data = "OK" };
    }

    public async Task<ServiceResponse<string>> DeleteCard(int cardId)
    {
        var dbAddress = await _ctx.CustomerCards
                .FirstOrDefaultAsync(x => x.Id == cardId);
        if (dbAddress == null)
            return new ServiceResponse<string>
            {
                Success = false,
                Message = "No cards found with the specified ID"
            };

        dbAddress.Deleted = true;

        await _ctx.SaveChangesAsync();

        return new ServiceResponse<string> { Data = "OK" };
    }
    #endregion

    #region Helper Methods
    private async Task<string?> GetRole(int userId)
    {
        var user = await _ctx.Users.FirstOrDefaultAsync(x => x.Id == userId);
        if (user == null)
            return null;

        return user.Role;
    }

    private void CreatePasswordHash(string password, out byte[] passwordHash)
    {
        using var hmac = new HMACSHA512(new byte[16]);
        passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }
    #endregion
}
