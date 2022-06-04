namespace RandomBooks.API.Services.AuthorService
{
    public interface IAuthorService
    {
        Task<ServiceResponse<Author>> AddAuthor(Author author);
        Task<ServiceResponse<Author>> GetAuthor(int authorId);
        Task<ServiceResponse<AuthorListResult>> GetAuthors(int page);
        Task<ServiceResponse<Author>> UpdateAuthor(Author author);
    }
}