// Models/Book.cs - Our main entity model

using Sieve.Attributes;

namespace SieveOperations.Api.Models;

public class Book
{
    public int Id { get; set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public string Title { get; set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public string Author { get; set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public DateTime PublishedDate { get; set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public decimal Price { get; set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public string Genre { get; set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public bool IsAvailable { get; set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public int PageCount { get; set; }

    [Sieve(CanFilter = false, CanSort = false)]
    public string ISBN { get; set; }

    // Foreign key for Publisher
    public int PublisherId { get; set; }
    public Publisher Publisher { get; set; }
}