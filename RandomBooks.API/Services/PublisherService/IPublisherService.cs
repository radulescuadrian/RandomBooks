namespace RandomBooks.API.Services.PublisherService
{
    public interface IPublisherService
    {
        Task<ServiceResponse<List<Publisher>>> AddPublisher(Publisher publisher);
        Task<ServiceResponse<List<Publisher>>> GetPublishers();
        Task<ServiceResponse<List<Publisher>>> GetVisiblePublishers();
        Task<ServiceResponse<List<Publisher>>> UpdatePublisher(Publisher publisher);
    }
}