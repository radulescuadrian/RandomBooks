namespace RandomBooks.API.Services.AuthorService
{
    public interface IAuthorService
    {
        Task<ServiceResponse<Author>> AddAuthor(AuthorEdit edit);
        Task<ServiceResponse<Author>> GetAuthor(int authorId);
        Task<ServiceResponse<AuthorListResult>> GetAuthors(int page);
        Task<ServiceResponse<List<Author>>> GetVisibleAuthors();
        Task<ServiceResponse<Author>> UpdateAuthor(AuthorEdit edit);
    }
}