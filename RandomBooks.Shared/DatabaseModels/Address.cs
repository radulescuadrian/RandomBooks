using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RandomBooks.Shared.DatabaseModels;

public class Address
{
    public int Id { get; set; }

    [Required]
    public string Street { get; set; } = string.Empty;

    [Required]
    public string City { get; set; } = string.Empty;

    [Required]
    public string Zip { get; set; } = string.Empty;

    [Required]
    public string Country { get; set; } = string.Empty;

    public bool Deleted { get; set; } = false;

    [NotMapped]
    public bool New { get; set; } = false;
    [NotMapped]
    public bool Editing { get; set; } = false;
    [NotMapped]
    public bool Selected { get; set; } = false;
}
