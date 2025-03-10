using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using SieveOperations.Api.Models;

namespace SieveOperations.Api.Data.Repositories;

public class BookRepository(ApplicationDbContext context, ISieveProcessor sieveProcessor)
    : IBookRepository
{
    public async Task<IEnumerable<Book>> GetAllAsync(SieveModel sieveModel)
    {
        var query = context.Books
            .Include(b => b.Publisher)
            .AsQueryable();

        return await sieveProcessor
            .Apply(sieveModel, query)
            .ToListAsync();
    }

    public async Task<int> GetCountAsync(SieveModel sieveModel, bool applyFiltering = true)
    {
        var query = context.Books.AsQueryable();
        
        if (applyFiltering)
        {
            return await sieveProcessor
                .Apply(sieveModel, query, applyPagination: false, applySorting: false)
                .CountAsync();
        }
        
        return await query.CountAsync();
    }

    public async Task<Book?> GetByIdAsync(int id)
    {
        return await context.Books
            .Include(b => b.Publisher)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<IEnumerable<Book>> GetAdvancedAsync(SieveModel sieveModel, decimal? minPrice = null, decimal? maxPrice = null)
    {
        var query = context.Books
            .Include(b => b.Publisher)
            .AsQueryable();

        if (minPrice.HasValue && maxPrice.HasValue)
        {
            query = query.Where(b => b.Price >= minPrice && b.Price <= maxPrice);
        }

        return await sieveProcessor
            .Apply(sieveModel, query)
            .ToListAsync();
    }
} 