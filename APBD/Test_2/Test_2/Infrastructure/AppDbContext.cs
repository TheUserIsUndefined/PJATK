using Microsoft.EntityFrameworkCore;
using Test_2.Core.Models;

namespace Test_2.Infrastructure;

public class AppDbContext : DbContext
{
    protected AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<PublishingHouse> PublishingHouses { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PublishingHouse>().HasData(
            new PublishingHouse{ IdPublishingHouse = 1, Name = "Meow", Country = "USA", City = "New York"},
            new PublishingHouse{ IdPublishingHouse = 2, Name = "Meow2", Country = "Russia", City = "Moscow"}
            );

        modelBuilder.Entity<Author>().HasData(
            new Author { IdAuthor = 1, FirstName = "No", LastName = "One" },
            new Author { IdAuthor = 2, FirstName = "Yes", LastName = "Two" }
        );

        modelBuilder.Entity<Book>().HasData(
            new Book { IdBook = 1, Name = "Yes", ReleaseDate = new DateTime(2013, 11, 27), IdPublishingHouse = 1 }
        );
    }
}