using System.Security.Claims;

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

    public async Task<ServiceResponse<UserListResult>> GetUsers(int page, string role)
    {
        var results = 6;
        var users = new List<User>();
        if (role == "Customer")
            users = await _ctx.Users
                .Where(x => x.Role == role)
                .Include(x => x.CustomerDetails)
                .ThenInclude(x => x.Cards)
                .Include(x => x.CustomerDetails)
                .ThenInclude(x => x.Addresses)
                .OrderBy(x => x.Username)
                .ToListAsync();
        else if (role == "Employee") 
            users = await _ctx.Users
                .Where(x => x.Role == role && x.Role != "Admin")
                .Include(x => x.EmployeeDetails)
                .OrderBy(x => x.Username)
                .ThenBy(x => x.Role)
                .ToListAsync();
        else users = await _ctx.Users
                .Where(x => x.Role == role && x.Role != "Admin")
                .OrderBy(x => x.Username)
                .ThenBy(x => x.Role)
                .ToListAsync();
        var pageCount = Math.Ceiling(users.Count / (double)results);

        if (users == null || users.Count == 0)
            return new ServiceResponse<UserListResult>
            {
                Success = false,
                Message = $"No {role}s found"
            };

        return new ServiceResponse<UserListResult>
        {
            Data = new UserListResult
            {
                Users = users.Skip((page - 1) * results).Take(results).ToList(),
                Page = page,
                Pages = (int)pageCount
            }
        };
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
    #endregion
}
