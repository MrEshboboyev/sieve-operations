using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using SieveOperations.Api.Models;

namespace SieveOperations.Api.Data.Repositories;

public class BookRepository : IBookRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ISieveProcessor _sieveProcessor;

    public BookRepository(ApplicationDbContext context, ISieveProcessor sieveProcessor)
    {
        _context = context;
        _sieveProcessor = sieveProcessor;
    }

    public async Task<IEnumerable<Book>> GetAllAsync(SieveModel sieveModel)
    {
        var query = _context.Books
            .Include(b => b.Publisher)
            .AsQueryable();

        return await _sieveProcessor
            .Apply(sieveModel, query)
            .ToListAsync();
    }

    public async Task<int> GetCountAsync(SieveModel sieveModel, bool applyFiltering = true)
    {
        var query = _context.Books.AsQueryable();
        
        if (applyFiltering)
        {
            return await _sieveProcessor
                .Apply(sieveModel, query, applyPagination: false, applySorting: false)
                .CountAsync();
        }
        
        return await query.CountAsync();
    }

    public async Task<Book?> GetByIdAsync(int id)
    {
        return await _context.Books
            .Include(b => b.Publisher)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<IEnumerable<Book>> GetAdvancedAsync(SieveModel sieveModel, decimal? minPrice = null, decimal? maxPrice = null)
    {
        var query = _context.Books
            .Include(b => b.Publisher)
            .AsQueryable();

        if (minPrice.HasValue && maxPrice.HasValue)
        {
            query = query.Where(b => b.Price >= minPrice && b.Price <= maxPrice);
        }

        return await _sieveProcessor
            .Apply(sieveModel, query)
            .ToListAsync();
    }
} 