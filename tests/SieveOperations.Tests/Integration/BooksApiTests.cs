using Microsoft.AspNetCore.Mvc.Testing;
using SieveOperations.Api.Models;
using System.Net;
using System.Net.Http.Json;

namespace SieveOperations.Tests.Integration;

public class BooksApiTests : IClassFixture<TestWebApplicationFactory>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public BooksApiTests(TestWebApplicationFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task GetBooks_ReturnsSuccessStatusCode()
    {
        // Act
        var response = await _client.GetAsync("/api/v1/Books");

        // Assert
        response.EnsureSuccessStatusCode();
        
        var content = await response.Content.ReadFromJsonAsync<ApiResponse<List<Book>>>();
        Assert.NotNull(content);
        Assert.True(content.Success);
        Assert.NotNull(content.Data);
        Assert.NotNull(content.Pagination);
    }

    [Fact]
    public async Task GetBook_WithValidId_ReturnsBook()
    {
        // Act
        var response = await _client.GetAsync("/api/v1/Books/1");

        // Assert
        response.EnsureSuccessStatusCode();
        
        var content = await response.Content.ReadFromJsonAsync<ApiResponse<Book>>();
        Assert.NotNull(content);
        Assert.True(content.Success);
        Assert.NotNull(content.Data);
        Assert.Equal(1, content.Data.Id);
    }

    [Fact]
    public async Task GetBook_WithInvalidId_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync("/api/v1/Books/999");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        
        var content = await response.Content.ReadFromJsonAsync<ApiResponse<object>>();
        Assert.NotNull(content);
        Assert.False(content.Success);
    }

    [Fact]
    public async Task GetBooksAdvanced_WithValidPriceRange_ReturnsFilteredBooks()
    {
        // Act
        var response = await _client.GetAsync("/api/v1/Books/advanced?priceRange=10-20");

        // Assert
        response.EnsureSuccessStatusCode();
        
        var content = await response.Content.ReadFromJsonAsync<ApiResponse<List<Book>>>();
        Assert.NotNull(content);
        Assert.True(content.Success);
        Assert.NotNull(content.Data);
        Assert.All(content.Data, book => Assert.InRange(book.Price, 10, 20));
    }

    [Fact]
    public async Task GetBooksAdvanced_WithInvalidPriceRange_ReturnsBadRequest()
    {
        // Act
        var response = await _client.GetAsync("/api/v1/Books/advanced?priceRange=invalid");

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        
        var content = await response.Content.ReadFromJsonAsync<ApiResponse<object>>();
        Assert.NotNull(content);
        Assert.False(content.Success);
        Assert.Contains("Invalid price range", content.Message);
    }
}