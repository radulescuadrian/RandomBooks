using Microsoft.EntityFrameworkCore;
using RandomBooks.Shared.DatabaseModels;

namespace RandomBooks.Data.Context;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region Category Seed
        modelBuilder.Entity<Category>().HasData(
            new Category
            {
                Id = 1,
                Name = "Action and Adventure"
            },
            new Category
            {
                Id = 2,
                Name = "Classics"
            },
            new Category
            {
                Id = 3,
                Name = "Horror"
            },
            new Category
            {
                Id = 4,
                Name = "Fantasy"
            },
            new Category
            {
                Id = 5,
                Name = "Romance"
            }); 
        #endregion
    }

    public DbSet<Blob> Blobs { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<User_Card> UserCards { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Publisher> Publishers { get; set; }
    public DbSet<Language> Languages { get; set; }
    public DbSet<Status> Statuses { get; set; }
}
