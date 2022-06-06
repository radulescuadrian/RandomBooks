namespace RandomBooks.Website.Logic.Services.AuthorService
{
    public interface IAuthorService
    {
        List<Author> Authors { get; set; }
        string Message { get; set; }
        int Page { get; set; }
        int PageCount { get; set; }

        event Action OnChange;

        Task<Author> AddAuthor(Author author, string image);
        Task<ServiceResponse<Author>> GetAuthor(int authorId);
        Task GetAuthors();
        Task<List<Author>> GetVisibleAuthorsList();
        Task<Author> UpdateAuthor(Author author, string image);
    }
}