using Microsoft.EntityFrameworkCore;
using RandomBooks.Data.Models;

namespace RandomBooks.Data.Context;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

    }

    public DbSet<Blob> Blobs { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<User_Card> UserCards { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Publisher> Publishers { get; set; }
    public DbSet<Status> Statuses { get; set; }
}
