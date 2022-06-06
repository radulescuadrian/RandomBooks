using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RandomBooks.Shared.DatabaseModels;

public class Book
{
    public int Id { get; set; }

    [Required]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string ISBN { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime? PublishedAt { get; set; } = DateTime.Now;
    public int Pages { get; set; }
    public Blob? Image { get; set; }
    public bool Featured { get; set; } = false;

    public Category? Category { get; set; }
    public int CategoryId { get; set; }
    public Publisher? Publisher { get; set; }
    public int PublisherId { get; set; }

    public List<BookVariants> Variants { get; set; } = new List<BookVariants>();
    public List<BookAuthors> Authors { get; set; } = new List<BookAuthors>();
    public List<BookLanguages> Languages { get; set; } = new List<BookLanguages>();

    public bool Deleted { get; set; } = false;

    [NotMapped]
    public bool Editing { get; set; } = false;
    [NotMapped]
    public bool New { get; set; } = false;
}
