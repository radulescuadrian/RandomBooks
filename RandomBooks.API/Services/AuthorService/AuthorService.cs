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
        var author = await _ctx.Authors
            .Include(x => x.Image)
            .FirstOrDefaultAsync(x => x.Id == authorId);
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
            .Include(x => x.Image)
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

    public async Task<ServiceResponse<List<Author>>> GetVisibleAuthors()
    {
        var authors = await _ctx.Authors
            .Where(x => !x.Deleted)
            .ToListAsync();
        if (authors == null)
            return new ServiceResponse<List<Author>>
            {
                Success = false,
                Message = "Authors not found."
            };

        return new ServiceResponse<List<Author>> { Data = authors };
    }

    public async Task<ServiceResponse<Author>> AddAuthor(AuthorEdit edit)
    {
        edit.Author.Editing = edit.Author.New = false;
        if (!string.IsNullOrWhiteSpace(edit.NewImage))
            edit.Author.Image = new Blob
            {
                Data = edit.NewImage,
                Type = "picture",
                Description = "author"
            };

        _ctx.Authors.Add(edit.Author);
        await _ctx.SaveChangesAsync();

        return new ServiceResponse<Author> { Data = edit.Author };
    }

    public async Task<ServiceResponse<Author>> UpdateAuthor(AuthorEdit edit)
    {
        var dbAuthor = await _ctx.Authors
            .Include(p => p.Image)
            .FirstOrDefaultAsync(x => x.Id == edit.Author.Id);
        if (dbAuthor == null)
            return new ServiceResponse<Author>
            {
                Success = false,
                Message = "Author not found."
            };

        dbAuthor.Name = edit.Author.Name;
        dbAuthor.Description = edit.Author.Description;
        dbAuthor.Deleted = edit.Author.Deleted;
        
        if (!string.IsNullOrWhiteSpace(edit.NewImage))
            if (dbAuthor.Image is null)
                dbAuthor.Image = new Blob
                {
                    Data = edit.NewImage,
                    Type = "picture",
                    Description = "author"
                };
            else
            {
                dbAuthor.Image.Data = edit.NewImage;
                dbAuthor.Image.ModifiedAt = DateTime.Now;
            }

        await _ctx.SaveChangesAsync();

        return new ServiceResponse<Author> { Data = dbAuthor };
    }
}
