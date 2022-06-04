namespace RandomBooks.API.Services.PublisherService;

public class PublisherService : IPublisherService
{
    private readonly DataContext _ctx;

    public PublisherService(DataContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<ServiceResponse<List<Publisher>>> GetPublishers()
    {
        var publishers = await _ctx.Publishers
            .OrderBy(x => x.Name)
            .ToListAsync();
        return new ServiceResponse<List<Publisher>> { Data = publishers };
    }

    public async Task<ServiceResponse<List<Publisher>>> AddPublisher(Publisher publisher)
    {
        publisher.Editing = publisher.New = false;
        _ctx.Publishers.Add(publisher);
        await _ctx.SaveChangesAsync();

        return await GetPublishers();
    }

    public async Task<ServiceResponse<List<Publisher>>> UpdatePublisher(Publisher publisher)
    {
        var dbpublisher = await _ctx.Publishers.FindAsync(publisher.Id);
        if (dbpublisher == null)
            return new ServiceResponse<List<Publisher>>
            {
                Success = false,
                Message = "Publisher not found."
            };

        dbpublisher.Name = publisher.Name;
        dbpublisher.Deleted = publisher.Deleted;

        await _ctx.SaveChangesAsync();

        return await GetPublishers();
    }

    public async Task<ServiceResponse<List<Publisher>>> DeletePublisher(int publisherId)
    {
        var publisher = await _ctx.Publishers.FindAsync(publisherId);
        if (publisher == null)
            return new ServiceResponse<List<Publisher>>
            {
                Success = false,
                Message = "Publisher not found."
            };

        publisher.Editing = publisher.New = false;
        publisher.Deleted = true;
        await _ctx.SaveChangesAsync();

        return await GetPublishers();
    }
}
