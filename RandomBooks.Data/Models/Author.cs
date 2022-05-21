using System.ComponentModel.DataAnnotations;

namespace RandomBooks.Data.Models;

public class Author
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }
    public string Description { get; set; }
}
