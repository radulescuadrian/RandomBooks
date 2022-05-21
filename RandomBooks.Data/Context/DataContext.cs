﻿using Microsoft.EntityFrameworkCore;
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

    public DbSet<Author> Authors { get; set; }
}
