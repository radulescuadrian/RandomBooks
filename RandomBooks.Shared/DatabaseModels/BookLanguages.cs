using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RandomBooks.Shared.DatabaseModels;

public class BookLanguages
{
    [JsonIgnore]
    public Book? Book { get; set; }
    public int BookId { get; set; }

    public Language? Language { get; set; }
    public int LanguageId { get; set; }

    public bool Deleted { get; set; } = false;

    [NotMapped]
    public bool New { get; set; } = false;
}
