using System.ComponentModel.DataAnnotations;

namespace RandomBooks.Shared.Authentication;

public class UserLogin
{
    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}
