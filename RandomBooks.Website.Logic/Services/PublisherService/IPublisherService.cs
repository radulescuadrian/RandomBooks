﻿namespace RandomBooks.Website.Logic.Services.PublisherService
{
    public interface IPublisherService
    {
        List<Publisher> Publishers { get; set; }

        event Action OnChange;

        Task AddPublisher(Publisher publisher);
        Publisher CreateNewPublisher();
        Task DeletePublisher(int publisherId);
        Task GetPublishers();
        Task UpdatePublisher(Publisher publisher);
    }
}