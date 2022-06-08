namespace RandomBooks.API.Services.UserService;

public class UserService : IUserService
{
    private readonly DataContext _ctx;

    public UserService(DataContext ctx)
    {
        _ctx = ctx;
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
