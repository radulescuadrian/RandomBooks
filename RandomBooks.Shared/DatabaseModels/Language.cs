using System.ComponentModel.DataAnnotations;

namespace RandomBooks.Shared.DatabaseModels;

public class Language
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }
}
