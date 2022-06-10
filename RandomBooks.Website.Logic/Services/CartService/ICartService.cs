namespace RandomBooks.Website.Logic.Services.CartService
{
    public interface ICartService
    {
        CustomerCard? SelectedCard { get; set; }
        Address SelectedAddress { get; set; }
        string? Notes { get; set; }

        event Action OnChange;

        Task AddToCart(CartItem cartItem);
        Task GetNumberOfItems();
        Task<List<CartProductResponse>> GetProducts();
        Task RemoveBook(int bookId, int bookTypeId);
        Task StoreItems(bool emptyLocalCart = false);
        Task UpdateQuantity(CartProductResponse book);
    }
}