using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using SieveOperations.Api.Data;
using SieveOperations.Api.Models;

namespace SieveOperations.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BooksController(
    ApplicationDbContext context,
    ISieveProcessor sieveProcessor) : ControllerBase
{
    // GET: api/Books
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Book>>> GetBooks(
        [FromQuery] SieveModel sieveModel)
    {
        var query = context.Books
            .Include(b => b.Publisher)
            .AsQueryable();

        // Apply Sieve filtering, sorting, and pagination
        var result = await sieveProcessor
            .Apply(sieveModel, query)
            .ToListAsync();

        // Get total count for pagination metadata
        var totalCount = await sieveProcessor
            .Apply(sieveModel, query, applyPagination: false, applySorting: false)
            .CountAsync();

        // Set pagination headers
        Response.Headers.Add("X-Total-Count", totalCount.ToString());

        return result;
    }

    // GET: api/Books/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Book>> GetBook(int id)
    {
        var book = await context.Books
            .Include(b => b.Publisher)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (book == null)
        {
            return NotFound();
        }

        return book;
    }

    // Example of custom filtering with Sieve
    [HttpGet("advanced")]
    public async Task<ActionResult<IEnumerable<Book>>> GetBooksAdvanced(
        [FromQuery] SieveModel sieveModel)
    {
        var query = context.Books
            .Include(b => b.Publisher)
            .AsQueryable();

        // Apply custom filtering logic before Sieve
        if (Request.Query.ContainsKey("priceRange"))
        {
            var range = Request.Query["priceRange"].ToString().Split('-');
            if (range.Length == 2)
            {
                if (decimal.TryParse(range[0], out var min) &&
                    decimal.TryParse(range[1], out var max))
                {
                    query = query.Where(b => b.Price >= min && b.Price <= max);
                }
            }
        }

        // Apply Sieve filtering, sorting, and pagination
        var result = await sieveProcessor
            .Apply(sieveModel, query)
            .ToListAsync();

        return result;
    }
}