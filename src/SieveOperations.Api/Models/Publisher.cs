using Sieve.Attributes;

namespace SieveOperations.Api.Models;

public class Publisher
{
    public int Id { get; set; }

    [Sieve(CanFilter = true, CanSort = true, Name = "publisher.name")]
    public string Name { get; set; }

    [Sieve(CanFilter = true, CanSort = true, Name = "publisher.country")]
    public string Country { get; set; }
}