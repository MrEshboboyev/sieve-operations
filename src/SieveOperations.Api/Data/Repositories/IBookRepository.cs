using SieveOperations.Api.Models;
using Sieve.Models;

namespace SieveOperations.Api.Data.Repositories;

public interface IBookRepository
{
    Task<IEnumerable<Book>> GetAllAsync(SieveModel sieveModel);
    Task<int> GetCountAsync(SieveModel sieveModel, bool applyFiltering = true);
    Task<Book?> GetByIdAsync(int id);
    Task<IEnumerable<Book>> GetAdvancedAsync(SieveModel sieveModel, decimal? minPrice = null, decimal? maxPrice = null);
}