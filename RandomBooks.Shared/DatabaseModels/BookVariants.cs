using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RandomBooks.Shared.DatabaseModels;

public class BookVariants
{
    [JsonIgnore]
    public Book? Book { get; set; }
    public int BookId { get; set; }

    public BookType? BookType { get; set; }
    public int BookTypeId { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Offer { get; set; }
    public bool HasOffer { get; set; } = false;

    [Required]
    public int Stock { get; set; }

    public bool Deleted { get; set; } = false;

    [NotMapped]
    public bool New { get; set; } = false;
}
