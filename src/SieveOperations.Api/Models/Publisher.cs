using System.ComponentModel.DataAnnotations;
using Sieve.Attributes;

namespace SieveOperations.Api.Models;

public class Publisher
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    [Sieve(CanFilter = true, CanSort = true, Name = "publisher.name")]
    public required string Name { get; set; }

    [Required]
    [StringLength(50)]
    [Sieve(CanFilter = true, CanSort = true, Name = "publisher.country")]
    public required string Country { get; set; }
}