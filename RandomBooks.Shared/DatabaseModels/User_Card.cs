using System.ComponentModel.DataAnnotations;

namespace RandomBooks.Shared.DatabaseModels;

public class User_Card
{
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Number { get; set; } = string.Empty;

    [Required]
    public int ExpirationMonth { get; set; }

    [Required]
    public int ExpirationYear { get; set; }
}
