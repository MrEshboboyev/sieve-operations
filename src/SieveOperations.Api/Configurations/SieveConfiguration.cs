// Configuration/SieveConfiguration.cs - Custom Sieve configuration

using Microsoft.Extensions.Options;
using Sieve.Models;
using Sieve.Services;
using SieveOperations.Api.Models;

namespace SieveOperations.Api.Configurations;

public class ApplicationSieveProcessor(IOptions<SieveOptions> options) : SieveProcessor(options)
{
    protected override SievePropertyMapper MapProperties(SievePropertyMapper mapper)
    {
        // Configure mappings for the Book entity
        mapper.Property<Book>(b => b.Title)
            .CanFilter()
            .CanSort();

        mapper.Property<Book>(b => b.Author)
            .CanFilter()
            .CanSort();

        mapper.Property<Book>(b => b.PublishedDate)
            .CanFilter()
            .CanSort();

        mapper.Property<Book>(b => b.Price)
            .CanFilter()
            .CanSort();

        mapper.Property<Book>(b => b.Genre)
            .CanFilter()
            .CanSort();

        mapper.Property<Book>(b => b.IsAvailable)
            .CanFilter()
            .CanSort();

        mapper.Property<Book>(b => b.PageCount)
            .CanFilter()
            .CanSort();

        // Configure mappings for nested properties
        mapper.Property<Book>(b => b.Publisher.Name)
            .CanFilter()
            .CanSort()
            .HasName("publisher.name");

        mapper.Property<Book>(b => b.Publisher.Country)
            .CanFilter()
            .CanSort()
            .HasName("publisher.country");

        return mapper;
    }
}