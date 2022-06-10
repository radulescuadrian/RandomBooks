namespace RandomBooks.API.Services.CartService;

public class CartService : ICartService
{
    private readonly DataContext _context;
    private readonly IAuthService _authService;

    public CartService(DataContext context, IAuthService authService)
    {
        _context = context;
        _authService = authService;
    }

    public async Task<ServiceResponse<List<CartProductResponse>>> GetCartProducts(List<CartItem> cartItems)
    {
        var result = new ServiceResponse<List<CartProductResponse>>
        {
            Data = new List<CartProductResponse>()
        };

        foreach (var item in cartItems)
        {
            var book = await _context.Books
                .Where(x => x.Id == item.BookId)
                .Include(x => x.Image)
                .FirstOrDefaultAsync();
            if (book == null)
                continue;

            var bookCategory = await _context.Categories
                .Where(x => x.Id == book.CategoryId)
                .FirstOrDefaultAsync();
            if (bookCategory == null)
                continue;

            var bookVariant = await _context.BookVariants
                .Where(x => x.BookId == item.BookId &&
                            x.BookTypeId == item.BookTypeId)
                .Include(x => x.BookType)
                .FirstOrDefaultAsync();
            if (bookVariant == null)
                continue;

            var cartProduct = new CartProductResponse
            {
                BookId = book.Id,
                Title = book.Title,
                Category = bookCategory.Name,
                Image = book.Image != null ? book.Image.Data : "book.svg",
                Price = bookVariant.HasOffer ? bookVariant.Offer : bookVariant.Price,
                BookType = bookVariant.BookType.Name,
                BookTypeId = bookVariant.BookTypeId,
                Quantity = bookVariant.Stock < item.Quantity ? bookVariant.Stock : item.Quantity,
                Stock = bookVariant.Stock
            };

            result.Data.Add(cartProduct);
        }

        return result;
    }

    public async Task<ServiceResponse<List<CartProductResponse>>> StoreCartItems(List<CartItem> cartItems)
    {
        cartItems.ForEach(x => x.UserId = _authService.GetUserId());
        _context.CartItems.AddRange(cartItems);
        await _context.SaveChangesAsync();

        return await GetDbCartProducts();
    }

    public async Task<ServiceResponse<int>> GetCartItemsCount()
    {
        var count = (await _context.CartItems.Where(x => x.UserId == _authService.GetUserId()).ToListAsync()).Count();
        return new ServiceResponse<int> { Data = count };
    }

    public async Task<ServiceResponse<List<CartProductResponse>>> GetDbCartProducts(int? userId = null)
    {
        if (userId == null)
            userId = _authService.GetUserId();

        return await GetCartProducts(await _context.CartItems
            .Where(x => x.UserId == userId).ToListAsync());
    }

    public async Task<ServiceResponse<bool>> AddToCart(CartItem cartItem)
    {
        cartItem.UserId = _authService.GetUserId();

        var sameItem = await _context.CartItems
            .FirstOrDefaultAsync(x => x.BookId == cartItem.BookId &&
                                      x.BookTypeId == cartItem.BookTypeId &&
                                      x.UserId == cartItem.UserId);

        var bookVariant = await _context.BookVariants
            .FirstOrDefaultAsync(x => x.BookId == cartItem.BookId &&
                                      x.BookTypeId == cartItem.BookTypeId);

        if (sameItem == null)
            _context.CartItems.Add(cartItem);
        else if (sameItem.Quantity >= bookVariant.Stock)
            sameItem.Quantity = bookVariant.Stock;
        else sameItem.Quantity += cartItem.Quantity;

        await _context.SaveChangesAsync();
        return new ServiceResponse<bool> { Data = true };
    }

    public async Task<ServiceResponse<bool>> UpdateQuantity(CartItem cartItem)
    {
        var dbCartItem = await _context.CartItems
            .FirstOrDefaultAsync(x => x.BookId == cartItem.BookId &&
                                      x.BookTypeId == cartItem.BookTypeId &&
                                      x.UserId == _authService.GetUserId());

        var bookVariant = await _context.BookVariants
            .FirstOrDefaultAsync(x => x.BookId == cartItem.BookId &&
                                      x.BookTypeId == cartItem.BookTypeId);

        if (dbCartItem == null)
            return new ServiceResponse<bool>
            {
                Data = false,
                Success = false,
                Message = "Cart item does not exist."
            };

        if (cartItem.Quantity <= bookVariant.Stock)
            dbCartItem.Quantity = cartItem.Quantity;

        await _context.SaveChangesAsync();
        return new ServiceResponse<bool> { Data = true };
    }

    public async Task<ServiceResponse<bool>> RemoveItemFromCart(int bookId, int bookTypeId)
    {
        var dbCartItem = await _context.CartItems
            .FirstOrDefaultAsync(x => x.BookId == bookId &&
                                      x.BookTypeId == bookTypeId &&
                                      x.UserId == _authService.GetUserId());
        if (dbCartItem == null)
            return new ServiceResponse<bool>
            {
                Data = false,
                Success = false,
                Message = "Cart item does not exist."
            };

        _context.CartItems.Remove(dbCartItem);
        await _context.SaveChangesAsync();

        return new ServiceResponse<bool> { Data = true };
    }
}
