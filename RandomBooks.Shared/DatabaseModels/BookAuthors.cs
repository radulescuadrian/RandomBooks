using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RandomBooks.Shared.DatabaseModels;

public class BookAuthors
{
    [JsonIgnore]
    public Book? Book { get; set; }
    public int BookId { get; set; }

    public Author? Author { get; set; }
    public int AuthorId { get; set; }

    public bool Deleted { get; set; } = false;

    [NotMapped]
    public bool New { get; set; } = false;
}
