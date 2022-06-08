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
        modelBuilder.Entity<BookVariants>()
            .HasKey(x => new { x.BookId, x.BookTypeId });

        modelBuilder.Entity<BookAuthors>()
            .HasKey(x => new { x.BookId, x.AuthorId });
        
        modelBuilder.Entity<BookLanguages>()
            .HasKey(x => new { x.BookId, x.LanguageId });

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

        #region BookTypes Seed
        modelBuilder.Entity<BookType>().HasData(
            new BookType
            {
                Id = 1,
                Name = "Paperback"
            },
            new BookType
            {
                Id = 2,
                Name = "E-Book"
            },
            new BookType
            {
                Id = 3,
                Name = "Audiobook"
            }); 
        #endregion
    }

    public DbSet<Blob> Blobs { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<CustomerCard> CustomerCards { get; set; }
    public DbSet<CustomerDetails> CustomerDetails { get; set; }
    public DbSet<EmployeeDetails> EmployeeDetails { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Status> Statuses { get; set; }

    public DbSet<Book> Books { get; set; }
    public DbSet<BookType> BookTypes { get; set; }
    public DbSet<BookVariants> BookVariants { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<BookAuthors> BookAuthors { get; set; }
    public DbSet<Language> Languages { get; set; }
    public DbSet<BookLanguages> BookLanguages { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Publisher> Publishers { get; set; }
}
