namespace RandomBooks.Shared.DTOs;

public class CartProductResponse
{
    public int BookId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int BookTypeId { get; set; }
    public string BookType { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public int Stock { get; set; }
}