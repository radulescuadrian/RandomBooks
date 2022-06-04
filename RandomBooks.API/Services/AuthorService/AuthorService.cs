namespace RandomBooks.API.Services.AuthorService;

public class AuthorService : IAuthorService
{
    private readonly DataContext _ctx;

    public AuthorService(DataContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<ServiceResponse<Author>> GetAuthor(int authorId)
    {
        var author = await _ctx.Authors.FirstOrDefaultAsync(x => x.Id == authorId);
        if (author == null)
            return new ServiceResponse<Author>
            {
                Success = false,
                Message = "This author could not be found."
            };
        
        return new ServiceResponse<Author> { Data = author };
    }

    public async Task<ServiceResponse<AuthorListResult>> GetAuthors(int page)
    {
        var results = 5;
        var authors = await _ctx.Authors
            .OrderBy(x => x.Name)
            .ToListAsync();
        var pageCount = Math.Ceiling(authors.Count / (double)results);

        return new ServiceResponse<AuthorListResult>
        {
            Data = new AuthorListResult
            {
                Authors = authors
                    .Skip((page - 1) * results)
                    .Take(results)
                    .ToList(),
                Page = page,
                Pages = (int)pageCount
            }
        };
    }

    public async Task<ServiceResponse<Author>> AddAuthor(Author author)
    {
        author.Editing = author.New = false;
        _ctx.Authors.Add(author);
        await _ctx.SaveChangesAsync();

        return new ServiceResponse<Author> { Data = author };
    }

    public async Task<ServiceResponse<Author>> UpdateAuthor(Author author)
    {
        var dbAuthor = await _ctx.Authors.FindAsync(author.Id);
        if (dbAuthor == null)
            return new ServiceResponse<Author>
            {
                Success = false,
                Message = "Author not found."
            };

        dbAuthor.Name = author.Name;
        dbAuthor.Description = author.Description;
        dbAuthor.Image = author.Image;
        dbAuthor.Deleted = author.Deleted;

        await _ctx.SaveChangesAsync();

        return new ServiceResponse<Author> { Data = author };
    }
}
