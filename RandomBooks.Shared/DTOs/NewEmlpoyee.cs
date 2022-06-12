using System.ComponentModel.DataAnnotations;

namespace RandomBooks.Shared.DTOs;

public class NewEmlpoyee
{
    public string Role { get; set; } = string.Empty;
    [Required]
    public string Firstname { get; set; } = string.Empty;
    [Required]
    public string Lastname { get; set; } = string.Empty;
    [Required]
    public string PhoneNumber { get; set; } = string.Empty;
}
