namespace RandomBooks.Website.Logic.Services.AuthorService
{
    public interface IAuthorService
    {
        List<Author> Authors { get; set; }
        string Message { get; set; }
        int Page { get; set; }
        int PageCount { get; set; }

        event Action OnChange;

        Task<Author> AddAuthor(Author author);
        Author CreateNewAuthor();
        Task<ServiceResponse<Author>> GetAuthor(int authorId);
        Task GetAuthors();
        Task<Author> UpdateAuthor(Author author);
    }
}