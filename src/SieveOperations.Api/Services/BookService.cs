using SieveOperations.Api.Data.Repositories;
using SieveOperations.Api.Models;
using Sieve.Models;

namespace SieveOperations.Api.Services;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;
    private readonly ILogger<BookService> _logger;

    public BookService(IBookRepository bookRepository, ILogger<BookService> logger)
    {
        _bookRepository = bookRepository;
        _logger = logger;
    }

    public async Task<(IEnumerable<Book> Books, int TotalCount)> GetBooksAsync(SieveModel sieveModel)
    {
        _logger.LogInformation("Getting books with filters: {Filters}", sieveModel.Filters);
        
        var books = await _bookRepository.GetAllAsync(sieveModel);
        var totalCount = await _bookRepository.GetCountAsync(sieveModel);
        
        return (books, totalCount);
    }

    public async Task<Book?> GetBookAsync(int id)
    {
        _logger.LogInformation("Getting book with ID: {Id}", id);
        return await _bookRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Book>> GetBooksAdvancedAsync(SieveModel sieveModel, string? priceRange)
    {
        _logger.LogInformation("Getting books with advanced filters: {Filters}, PriceRange: {PriceRange}", 
            sieveModel.Filters, priceRange);
            
        decimal? minPrice = null;
        decimal? maxPrice = null;
        
        if (!string.IsNullOrEmpty(priceRange))
        {
            var range = priceRange.Split('-');
            if (range.Length == 2)
            {
                if (decimal.TryParse(range[0], out var min) &&
                    decimal.TryParse(range[1], out var max))
                {
                    minPrice = min;
                    maxPrice = max;
                }
            }
        }
        
        return await _bookRepository.GetAdvancedAsync(sieveModel, minPrice, maxPrice);
    }
} 