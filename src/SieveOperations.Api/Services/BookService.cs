using SieveOperations.Api.Data.Repositories;
using SieveOperations.Api.Models;
using Sieve.Models;

namespace SieveOperations.Api.Services;

public class BookService(
    IBookRepository bookRepository, 
    ILogger<BookService> logger) : IBookService
{
    public async Task<(IEnumerable<Book> Books, int TotalCount)> GetBooksAsync(SieveModel sieveModel)
    {
        logger.LogInformation("Getting books with filters: {Filters}", sieveModel.Filters);
        
        var books = await bookRepository.GetAllAsync(sieveModel);
        var totalCount = await bookRepository.GetCountAsync(sieveModel);
        
        return (books, totalCount);
    }

    public async Task<Book?> GetBookAsync(int id)
    {
        logger.LogInformation("Getting book with ID: {Id}", id);
        return await bookRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Book>> GetBooksAdvancedAsync(SieveModel sieveModel, string? priceRange)
    {
        logger.LogInformation("Getting books with advanced filters: {Filters}, PriceRange: {PriceRange}", 
            sieveModel.Filters, priceRange);
            
        decimal? minPrice = null;
        decimal? maxPrice = null;

        if (string.IsNullOrEmpty(priceRange))
            return await bookRepository.GetAdvancedAsync(sieveModel, minPrice, maxPrice);
        var range = priceRange.Split('-');
        if (range.Length != 2) return await bookRepository.GetAdvancedAsync(sieveModel, minPrice, maxPrice);
        if (!decimal.TryParse(range[0], out var min) ||
            !decimal.TryParse(range[1], out var max))
            return await bookRepository.GetAdvancedAsync(sieveModel, minPrice, maxPrice);
        minPrice = min;
        maxPrice = max;

        return await bookRepository.GetAdvancedAsync(sieveModel, minPrice, maxPrice);
    }
} 