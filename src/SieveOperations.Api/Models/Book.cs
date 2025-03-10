using System.ComponentModel.DataAnnotations;
using Sieve.Attributes;

namespace SieveOperations.Api.Models;

public class Book
{
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    [Sieve(CanFilter = true, CanSort = true)]
    public required string Title { get; set; }

    [Required]
    [StringLength(100)]
    [Sieve(CanFilter = true, CanSort = true)]
    public required string Author { get; set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public DateTime PublishedDate { get; set; }

    [Range(0, 9999.99)]
    [Sieve(CanFilter = true, CanSort = true)]
    public decimal Price { get; set; }

    [StringLength(50)]
    [Sieve(CanFilter = true, CanSort = true)]
    public required string Genre { get; set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public bool IsAvailable { get; set; }

    [Range(1, 10000)]
    [Sieve(CanFilter = true, CanSort = true)]
    public int PageCount { get; set; }

    [StringLength(20)]
    [RegularExpression(@"^(?=(?:\D*\d){10}(?:(?:\D*\d){3})?$)[\d-]+$")]
    [Sieve(CanFilter = false, CanSort = false)]
    public required string ISBN { get; set; }

    // Foreign key for Publisher
    public int PublisherId { get; set; }
    
    public Publisher Publisher { get; set; }
}