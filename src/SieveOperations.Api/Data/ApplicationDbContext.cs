// Data/ApplicationDbContext.cs - DbContext for Entity Framework

using Microsoft.EntityFrameworkCore;
using SieveOperations.Api.Models;

namespace SieveOperations.Api.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Book> Books { get; set; }
    public DbSet<Publisher> Publishers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure the one-to-many relationship
        modelBuilder.Entity<Book>()
            .HasOne(b => b.Publisher)
            .WithMany()
            .HasForeignKey(b => b.PublisherId);

        // Seed data
        modelBuilder.Entity<Publisher>().HasData(
            new Publisher { Id = 1, Name = "Penguin Random House", Country = "USA" },
            new Publisher { Id = 2, Name = "HarperCollins", Country = "UK" },
            new Publisher { Id = 3, Name = "Simon & Schuster", Country = "USA" }
        );

        modelBuilder.Entity<Book>().HasData(
            new Book
            {
                Id = 1,
                Title = "The Great Gatsby",
                Author = "F. Scott Fitzgerald",
                PublishedDate = new DateTime(1925, 4, 10),
                Price = 12.99m,
                Genre = "Fiction",
                IsAvailable = true,
                PageCount = 180,
                ISBN = "978-0743273565",
                PublisherId = 1 
            },
            new Book
            {
                Id = 2,
                Title = "To Kill a Mockingbird",
                Author = "Harper Lee",
                PublishedDate = new DateTime(1960, 7, 11),
                Price = 14.99m,
                Genre = "Fiction",
                IsAvailable = true,
                PageCount = 281,
                ISBN = "978-0061120084",
                PublisherId = 2 
            },
            new Book
            {
                Id = 3,
                Title = "1984",
                Author = "George Orwell",
                PublishedDate = new DateTime(1949, 6, 8),
                Price = 11.99m,
                Genre = "Science Fiction",
                IsAvailable = false,
                PageCount = 328,
                ISBN = "978-0451524935",
                PublisherId = 3 
            },
            new Book
            {
                Id = 4,
                Title = "The Hobbit",
                Author = "J.R.R. Tolkien",
                PublishedDate = new DateTime(1937, 9, 21),
                Price = 15.99m,
                Genre = "Fantasy",
                IsAvailable = true,
                PageCount = 310,
                ISBN = "978-0547928227",
                PublisherId = 1 
            },
            new Book
            {
                Id = 5,
                Title = "Pride and Prejudice",
                Author = "Jane Austen",
                PublishedDate = new DateTime(1813, 1, 28),
                Price = 9.99m,
                Genre = "Romance",
                IsAvailable = true,
                PageCount = 432,
                ISBN = "978-0141439518",
                PublisherId = 2 
            }
        );
    }
}
