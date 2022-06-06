namespace RandomBooks.Website.Logic.Services.PublisherService
{
    public interface IPublisherService
    {
        List<Publisher> Publishers { get; set; }

        event Action OnChange;

        Task AddPublisher(Publisher publisher);
        Publisher CreateNewPublisher();
        Task GetPublishers();
        Task<List<Publisher>> GetVisiblePublishersList();
        Task UpdatePublisher(Publisher publisher);
    }
}