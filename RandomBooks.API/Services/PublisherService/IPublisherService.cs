namespace RandomBooks.API.Services.PublisherService
{
    public interface IPublisherService
    {
        Task<ServiceResponse<List<Publisher>>> AddPublisher(Publisher publisher);
        Task<ServiceResponse<List<Publisher>>> DeletePublisher(int publisherId);
        Task<ServiceResponse<List<Publisher>>> GetPublishers();
        Task<ServiceResponse<List<Publisher>>> UpdatePublisher(Publisher publisher);
    }
}