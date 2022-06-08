using RandomBooks.Shared.DatabaseModels;

namespace RandomBooks.Shared.DTOs;

public class UserListResult
{
    public List<User> Users { get; set; }
    public int Page { get; set; }
    public int Pages { get; set; }
}
