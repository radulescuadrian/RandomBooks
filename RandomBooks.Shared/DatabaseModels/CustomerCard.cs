using System.ComponentModel.DataAnnotations;

namespace RandomBooks.Shared.DatabaseModels;

public class CustomerCard
{
    public int Id { get; set; }
    public int CustomerDetailsId { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Number { get; set; } = string.Empty;

    [Required]
    public int ExpirationMonth { get; set; }

    [Required]
    public int ExpirationYear { get; set; }

    public bool Deleted { get; set; }
}
