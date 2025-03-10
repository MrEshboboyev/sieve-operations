using SieveOperations.Api.Models;
using Sieve.Models;

namespace SieveOperations.Api.Services;

public interface IBookService
{
    Task<(IEnumerable<Book> Books, int TotalCount)> GetBooksAsync(SieveModel sieveModel);
    Task<Book?> GetBookAsync(int id);
    Task<IEnumerable<Book>> GetBooksAdvancedAsync(SieveModel sieveModel, string? priceRange);
} 