using Microsoft.EntityFrameworkCore;
using Moq;
using Sieve.Models;
using Sieve.Services;
using SieveOperations.Api.Data;
using SieveOperations.Api.Data.Repositories;
using SieveOperations.Api.Models;

namespace SieveOperations.Tests.Repositories;

public class BookRepositoryTests
{
    private readonly DbContextOptions<ApplicationDbContext> _contextOptions;
    private readonly Mock<ISieveProcessor> _mockSieveProcessor;

    public BookRepositoryTests()
    {
        _contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _mockSieveProcessor = new Mock<ISieveProcessor>();
        
        // Seed the database
        using var context = new ApplicationDbContext(_contextOptions);
        SeedDatabase(context);
    }

    private void SeedDatabase(ApplicationDbContext context)
    {
        var publisher = new Publisher { Id = 1, Name = "Test Publisher", Country = "US" };
        context.Publishers.Add(publisher);
        
        context.Books.AddRange(
            new Book
            {
                Id = 1,
                Title = "Test Book 1",
                Author = "Author 1",
                Genre = "Fiction",
                ISBN = "1234567890",
                Price = 19.99m,
                PublisherId = 1,
                Publisher = publisher
            },
            new Book
            {
                Id = 2,
                Title = "Test Book 2",
                Author = "Author 2",
                Genre = "Non-Fiction",
                ISBN = "0987654321",
                Price = 29.99m,
                PublisherId = 1,
                Publisher = publisher
            }
        );
        
        context.SaveChanges();
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsBook_WhenIdExists()
    {
        // Arrange
        await using var context = new ApplicationDbContext(_contextOptions);
        var repository = new BookRepository(context, _mockSieveProcessor.Object);
        
        // Act
        var result = await repository.GetByIdAsync(1);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Test Book 1", result.Title);
        Assert.NotNull(result.Publisher);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenIdDoesNotExist()
    {
        // Arrange
        await using var context = new ApplicationDbContext(_contextOptions);
        var repository = new BookRepository(context, _mockSieveProcessor.Object);
        
        // Act
        var result = await repository.GetByIdAsync(999);
        
        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAdvancedAsync_AppliesPriceFilter_WhenPriceRangeProvided()
    {
        // Arrange
        await using var context = new ApplicationDbContext(_contextOptions);
        var repository = new BookRepository(context, _mockSieveProcessor.Object);
        
        var sieveModel = new SieveModel();
        var books = context.Books.ToList();
        
        _mockSieveProcessor
            .Setup(x => x.Apply(
                It.IsAny<SieveModel>(),
                It.IsAny<IQueryable<Book>>(),
                It.IsAny<object[]>(),
                It.IsAny<bool>(),
                It.IsAny<bool>(),
                It.IsAny<bool>()))
            .Returns<SieveModel, IQueryable<Book>, object, bool, bool, bool>(
                (model, query, _, _, _, _) => query);
        
        // Act
        var result = await repository.GetAdvancedAsync(sieveModel, 15, 25);
        
        // Assert
        Assert.Single(result);
        Assert.Equal(19.99m, result.First().Price);
    }
}