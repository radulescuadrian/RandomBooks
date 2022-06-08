namespace RandomBooks.API.Services.BookService;

public class BookService : IBookService
{
    private readonly DataContext _ctx;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public BookService(DataContext ctx, IHttpContextAccessor httpContextAccessor)
    {
        _ctx = ctx;
        _httpContextAccessor = httpContextAccessor;
    }

    #region Dropdowns
    public async Task<ServiceResponse<List<BookType>>> GetBookTypes()
    {
        var bookTypes = await _ctx.BookTypes.ToListAsync();
        if (bookTypes == null || bookTypes.Count == 0)
            return new ServiceResponse<List<BookType>>
            {
                Success = false,
                Message = "No book types found."
            };

        return new ServiceResponse<List<BookType>> { Data = bookTypes };
    }

    public async Task<ServiceResponse<List<Author>>> GetAuthors()
    {
        var authors = await _ctx.Authors
            .Where(x => !x.Deleted)
            .ToListAsync();
        if (authors == null || authors.Count == 0)
            return new ServiceResponse<List<Author>>
            {
                Success = false,
                Message = "No authors found."
            };

        return new ServiceResponse<List<Author>> { Data = authors };
    }

    public async Task<ServiceResponse<List<Language>>> GetLanguages()
    {
        var languages = await _ctx.Languages
            .Where(x => !x.Deleted)
            .ToListAsync();
        if (languages == null || languages.Count == 0)
            return new ServiceResponse<List<Language>>
            {
                Success = false,
                Message = "No languages found."
            };

        return new ServiceResponse<List<Language>> { Data = languages };
    }
    #endregion

    public async Task<ServiceResponse<Book>> GetBook(int bookId)
    {
        Book book = null;

        if (_httpContextAccessor.HttpContext.User.IsInRole("Admin"))
        {
            book = await _ctx.Books
                .Include(x => x.Variants)
                .ThenInclude(x => x.BookType)
                .Include(x => x.Authors)
                .ThenInclude(x => x.Author)
                .Include(x => x.Languages)
                .ThenInclude(x => x.Language)
                .Include(x => x.Image)
                .FirstOrDefaultAsync(b => b.Id == bookId);
        }
        else
        {
            book = await _ctx.Books
                .Include(x => x.Variants.Where(v => !v.Deleted))
                .Include(x => x.Authors.Where(a => !a.Deleted))
                .Include(x => x.Languages.Where(l => !l.Deleted))
                .Include(x => x.Image)
                .FirstOrDefaultAsync(b => b.Id == bookId && !b.Deleted);
        }

        if (book == null)
            return new ServiceResponse<Book>
            {
                Success = false,
                Message = "Sorry, this book does not exist."
            };

        return new ServiceResponse<Book> { Data = book };
    }

    public async Task<ServiceResponse<List<Book>>> GetBooks()
    {
        var books = await _ctx.Books
            .Where(x => !x.Deleted)
            .Include(x => x.Variants.Where(v => !v.Deleted))
            .Include(x => x.Authors.Where(a => !a.Deleted))
            .Include(x => x.Languages.Where(l => !l.Deleted))
            .Include(x => x.Image)
            .ToListAsync();
        if (books == null || books.Count == 0)
            return new ServiceResponse<List<Book>>
            {
                Success = false,
                Message = "No books found"
            };

        return new ServiceResponse<List<Book>> { Data = books };
    }

    public async Task<ServiceResponse<List<Book>>> GetFeaturedBooks()
    {
        var books = await _ctx.Books
                .Where(x => x.Featured && !x.Deleted)
                .Include(x => x.Variants.Where(v => !v.Deleted))
                .Include(x => x.Authors.Where(a => !a.Deleted))
                .Include(x => x.Languages.Where(l => !l.Deleted))
                .Include(x => x.Image)
                .ToListAsync();
        if (books == null || books.Count == 0)
            return new ServiceResponse<List<Book>>
            {
                Success = false,
                Message = "No books found"
            };

        return new ServiceResponse<List<Book>> { Data = books };
    }

    public async Task<ServiceResponse<BookListResult>> GetAdminBooks(int page)
    {
        int results;
        double pageCount;
        var books = new List<Book>();
        if (page == 0)
        {
            results = 1;
            pageCount = 1;
            books = await _ctx.Books
                .Include(x => x.Variants.Where(v => !v.Deleted))
                .ThenInclude(x => x.BookType)
                .Include(x => x.Authors.Where(a => !a.Deleted))
                .ThenInclude(x => x.Author)
                .Include(x => x.Languages.Where(l => !l.Deleted))
                .ThenInclude(x => x.Language)
                .Include(x => x.Image)
                .OrderBy(x => x.Title)
                .ToListAsync();
        }
        else
        {
            results = 8;
            books = await _ctx.Books
                .Include(x => x.Variants.Where(v => !v.Deleted))
                .ThenInclude(x => x.BookType)
                .Include(x => x.Authors.Where(a => !a.Deleted))
                .ThenInclude(x => x.Author)
                .Include(x => x.Languages.Where(l => !l.Deleted))
                .ThenInclude(x => x.Language)
                .Include(x => x.Image)
                .OrderBy(x => x.Title)
                .ToListAsync();
            pageCount = Math.Ceiling(books.Count / (double)results);
        }

        if (books == null || books.Count == 0)
            return new ServiceResponse<BookListResult>
            {
                Success = false,
                Message = "No books found"
            };

        return new ServiceResponse<BookListResult> { Data = new BookListResult
        {
            Books = page != 0 ? books.Skip((page - 1) * results).Take(results).ToList() : 
                                books,
            Page = page,
            Pages = (int)pageCount
        }};
    }

    public async Task<ServiceResponse<Book>> AddBook(BookEdit edit)
    {
        foreach (var variant in edit.Book.Variants)
            variant.BookType = null;

        foreach (var language in edit.Book.Languages)
            language.Language = null;

        foreach (var author in edit.Book.Authors)
            author.Author = null;

        if (!string.IsNullOrWhiteSpace(edit.NewImage))
            edit.Book.Image = new Blob
            {
                Data = edit.NewImage,
                Type = "picture",
                Description = "book"
            };

        _ctx.Books.Add(edit.Book);
        await _ctx.SaveChangesAsync();

        return new ServiceResponse<Book> { Data = edit.Book };
    }

    public async Task<ServiceResponse<Book>> UpdateBook(BookEdit edit)
    {
        var dbBook = await _ctx.Books
            .Include(p => p.Image)
            .FirstOrDefaultAsync(x => x.Id == edit.Book.Id);
        if (dbBook is null)
            return new ServiceResponse<Book>
            {
                Success = false,
                Message = "Book not found."
            };

        dbBook.Title = edit.Book.Title;
        dbBook.Description = edit.Book.Description;
        dbBook.ISBN = edit.Book.ISBN;
        dbBook.PublishedAt = edit.Book.PublishedAt;
        dbBook.Pages = edit.Book.Pages;
        dbBook.Featured = edit.Book.Featured;
        dbBook.CategoryId = edit.Book.CategoryId;
        dbBook.PublisherId = edit.Book.PublisherId;
        dbBook.Deleted = edit.Book.Deleted;

        // variants
        foreach (var variant in edit.Book.Variants)
        {
            var dbVariant = await _ctx.BookVariants
                .SingleOrDefaultAsync(x => x.BookId == variant.BookId &&
                                           x.BookTypeId == variant.BookTypeId);
            if (dbVariant == null)
            {
                variant.BookType = null;
                _ctx.BookVariants.Add(variant);
            }
            else
            {
                dbVariant.BookTypeId = variant.BookTypeId;
                dbVariant.Price = variant.Price;
                dbVariant.Stock = variant.Stock;
                dbVariant.Offer = variant.Offer;
                dbVariant.HasOffer = variant.HasOffer;
                dbVariant.Deleted = variant.Deleted;
            }
        }

        // authors
        foreach (var author in edit.Book.Authors)
        {
            var dbAuthor = await _ctx.BookAuthors
                .SingleOrDefaultAsync(x => x.BookId == author.BookId &&
                                           x.AuthorId == author.AuthorId);
            if (dbAuthor == null)
            {
                author.Author = null;
                _ctx.BookAuthors.Add(author);
            }
            else
            {
                dbAuthor.AuthorId = author.AuthorId;
                dbAuthor.Deleted = author.Deleted;
            }
        }

        // languages
        foreach (var language in edit.Book.Languages)
        {
            var dbVariant = await _ctx.BookLanguages
                .SingleOrDefaultAsync(x => x.BookId == language.BookId &&
                                           x.LanguageId == language.LanguageId);
            if (dbVariant == null)
            {
                language.Language = null;
                _ctx.BookLanguages.Add(language);
            }
            else
            {
                dbVariant.LanguageId = language.LanguageId;
                dbVariant.Deleted = language.Deleted;
            }
        }

        // image
        if (!string.IsNullOrWhiteSpace(edit.NewImage))
            if (dbBook.Image is null)
                dbBook.Image = new Blob
                {
                    Data = edit.NewImage,
                    Type = "picture",
                    Description = "book"
                };
            else
            {
                dbBook.Image.Data = edit.NewImage;
                dbBook.Image.ModifiedAt = DateTime.Now;
            }

        await _ctx.SaveChangesAsync();

        return new ServiceResponse<Book> { Data = dbBook };
    }
}
