using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
    [Range(1, 12)]
    public int ExpirationMonth { get; set; }

    [Required]
    [Range(2021, 2050)]
    public int ExpirationYear { get; set; }

    public bool Deleted { get; set; }

    [NotMapped]
    public bool New { get; set; } = false;
    [NotMapped]
    public bool Editing { get; set; } = false;
}
