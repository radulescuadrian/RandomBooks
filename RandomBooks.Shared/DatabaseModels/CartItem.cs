namespace RandomBooks.Shared.DatabaseModels;

public class CartItem
{
    public int UserId { get; set; }
    public int BookId { get; set; }
    public int BookTypeId { get; set; }
    public int Quantity { get; set; } = 1;
}