using Microsoft.AspNetCore.Mvc;
using Sieve.Models;
using SieveOperations.Api.Models;
using SieveOperations.Api.Services;
using System.Text.RegularExpressions;

namespace SieveOperations.Api.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public partial class BooksController(
    IBookService bookService,
    ILogger<BooksController> logger) : ControllerBase
{
    /// <summary>
    /// Gets a paginated list of books with optional filtering and sorting
    /// </summary>
    /// <param name="sieveModel">Sieve model containing filter, sort and pagination parameters</param>
    /// <returns>A list of books</returns>
    /// <response code="200">Returns the list of books</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<Book>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<IEnumerable<Book>>>> GetBooks([FromQuery] SieveModel sieveModel)
    {
        try
        {
            var (books, totalCount) = await bookService.GetBooksAsync(sieveModel);
            
            var pagination = new PaginationMetadata
            {
                TotalCount = totalCount,
                PageSize = sieveModel.PageSize ?? 10,
                CurrentPage = sieveModel.Page ?? 1
            };
            
            return Ok(ApiResponse<IEnumerable<Book>>.SuccessResponse(books, "Books retrieved successfully", pagination));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while getting books");
            return StatusCode(500, ApiResponse<object>.ErrorResponse("An unexpected error occurred"));
        }
    }

    /// <summary>
    /// Gets a specific book by id
    /// </summary>
    /// <param name="id">The book id</param>
    /// <returns>The requested book</returns>
    /// <response code="200">Returns the requested book</response>
    /// <response code="404">If the book doesn't exist</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<Book>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<Book>>> GetBook(int id)
    {
        try
        {
            var book = await bookService.GetBookAsync(id);

            if (book == null)
            {
                return NotFound(ApiResponse<object>.ErrorResponse($"Book with ID {id} not found"));
            }

            return Ok(ApiResponse<Book>.SuccessResponse(book, "Book retrieved successfully"));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while getting book with ID {Id}", id);
            return StatusCode(500, ApiResponse<object>.ErrorResponse("An unexpected error occurred"));
        }
    }

    /// <summary>
    /// Gets books with advanced filtering options
    /// </summary>
    /// <param name="sieveModel">Sieve model containing filter, sort and pagination parameters</param>
    /// <param name="priceRange">Optional price range filter in format min-max</param>
    /// <returns>A filtered list of books</returns>
    /// <response code="200">Returns the filtered list of books</response>
    /// <response code="400">If the price range format is invalid</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpGet("advanced")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<Book>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    // GET /api/v1/books/advanced?sorts=price&filters=price>10&page=1&pageSize=10&priceRange=10.99-50.99
    public async Task<ActionResult<ApiResponse<IEnumerable<Book>>>> GetBooksAdvanced(
        [FromQuery] SieveModel sieveModel,
        [FromQuery] string? priceRange = null)
    {
        try
        {
            if (priceRange != null && !PriceRangeFormatRegex().IsMatch(priceRange))
            {
                return BadRequest(ApiResponse<object>.ErrorResponse("Invalid price range format. Use min-max (e.g., 10.50-20.99)"));
            }
            
            var books = await bookService.GetBooksAdvancedAsync(sieveModel, priceRange);
            return Ok(ApiResponse<IEnumerable<Book>>.SuccessResponse(books, "Books retrieved successfully"));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while getting books with advanced filtering");
            return StatusCode(500, ApiResponse<object>.ErrorResponse("An unexpected error occurred"));
        }
    }

    [GeneratedRegex(@"^\d+(\.\d+)?-\d+(\.\d+)?$")]
    private static partial Regex PriceRangeFormatRegex();
} 