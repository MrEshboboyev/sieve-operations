using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Sieve.Models;
using SieveOperations.Api.Controllers.V1;
using SieveOperations.Api.Models;
using SieveOperations.Api.Services;

namespace SieveOperations.Tests.Controllers;

public class BooksControllerTests
{
    private readonly Mock<IBookService> _mockService;
    private readonly Mock<ILogger<BooksController>> _mockLogger;
    private readonly BooksController _controller;
    private readonly List<Book> _books;

    public BooksControllerTests()
    {
        // Setup test data
        _books =
        [
            new Book
            {
                Id = 1,
                Title = "Test Book 1",
                Author = "Author 1",
                Genre = "Fiction",
                ISBN = "1234567890",
                Price = 19.99m,
                Publisher = new Publisher { Id = 1, Name = "Test Publisher", Country = "US" }
            },

            new Book
            {
                Id = 2,
                Title = "Test Book 2",
                Author = "Author 2",
                Genre = "Non-Fiction",
                ISBN = "0987654321",
                Price = 29.99m,
                Publisher = new Publisher { Id = 1, Name = "Test Publisher", Country = "US" }
            }
        ];

        // Setup mocks
        _mockService = new Mock<IBookService>();
        _mockLogger = new Mock<ILogger<BooksController>>();
        _controller = new BooksController(_mockService.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetBooks_ReturnsOkResult_WithBooks()
    {
        // Arrange
        var sieveModel = new SieveModel();
        _mockService.Setup(service => service.GetBooksAsync(sieveModel))
            .ReturnsAsync((_books, _books.Count));

        // Act
        var result = await _controller.GetBooks(sieveModel);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<ApiResponse<IEnumerable<Book>>>(okResult.Value);
        Assert.True(response.Success);
        Assert.Equal(_books, response.Data);
        Assert.Equal(_books.Count, response.Pagination.TotalCount);
    }

    [Fact]
    public async Task GetBook_ReturnsOkResult_WhenBookExists()
    {
        // Arrange
        var bookId = 1;
        var book = _books[0];
        _mockService.Setup(service => service.GetBookAsync(bookId))
            .ReturnsAsync(book);

        // Act
        var result = await _controller.GetBook(bookId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<ApiResponse<Book>>(okResult.Value);
        Assert.True(response.Success);
        Assert.Equal(book, response.Data);
    }

    [Fact]
    public async Task GetBook_ReturnsNotFound_WhenBookDoesNotExist()
    {
        // Arrange
        var nonExistentId = 999;
        _mockService.Setup(service => service.GetBookAsync(nonExistentId))
            .ReturnsAsync((Book)null!);

        // Act
        var result = await _controller.GetBook(nonExistentId);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        var response = Assert.IsType<ApiResponse<object>>(notFoundResult.Value);
        Assert.False(response.Success);
        Assert.Contains("not found", response.Message);
    }

    [Fact]
    public async Task GetBooksAdvanced_ReturnsBadRequest_WithInvalidPriceRange()
    {
        // Arrange
        var sieveModel = new SieveModel();
        var invalidPriceRange = "invalid-format";

        // Act
        var result = await _controller.GetBooksAdvanced(sieveModel, invalidPriceRange);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        var response = Assert.IsType<ApiResponse<object>>(badRequestResult.Value);
        Assert.False(response.Success);
        Assert.Contains("Invalid price range", response.Message);
    }

    [Fact]
    public async Task GetBooksAdvanced_ReturnsOkResult_WithValidPriceRange()
    {
        // Arrange
        var sieveModel = new SieveModel();
        var validPriceRange = "10.50-30.99";
        _mockService.Setup(service => service.GetBooksAdvancedAsync(sieveModel, validPriceRange))
            .ReturnsAsync(_books);

        // Act
        var result = await _controller.GetBooksAdvanced(sieveModel, validPriceRange);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<ApiResponse<IEnumerable<Book>>>(okResult.Value);
        Assert.True(response.Success);
        Assert.Equal(_books, response.Data);
    }
}