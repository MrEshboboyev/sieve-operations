using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using Sieve.Models;
using SieveOperations.Api.Data.Repositories;
using SieveOperations.Api.Models;
using SieveOperations.Api.Services;
using Xunit;

namespace SieveOperations.Tests.Services
{
    public class BookServiceTests
    {
        private readonly Mock<IBookRepository> _mockRepo;
        private readonly Mock<ILogger<BookService>> _mockLogger;
        private readonly BookService _service;
        private readonly List<Book> _books;

        public BookServiceTests()
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
            _mockRepo = new Mock<IBookRepository>();
            _mockLogger = new Mock<ILogger<BookService>>();
            _service = new BookService(_mockRepo.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetBooksAsync_ReturnsAllBooks_WhenNoFiltersApplied()
        {
            // Arrange
            var sieveModel = new SieveModel();
            _mockRepo.Setup(repo => repo.GetAllAsync(sieveModel))
                .ReturnsAsync(_books);
            _mockRepo.Setup(repo => repo.GetCountAsync(sieveModel, It.IsAny<bool>()))
                .ReturnsAsync(_books.Count);

            // Act
            var result = await _service.GetBooksAsync(sieveModel);

            // Assert
            Assert.Equal(_books, result.Books);
            Assert.Equal(_books.Count, result.TotalCount);
        }

        [Fact]
        public async Task GetBookAsync_ReturnsBook_WhenIdExists()
        {
            // Arrange
            var bookId = 1;
            var expectedBook = _books.First(b => b.Id == bookId);
            _mockRepo.Setup(repo => repo.GetByIdAsync(bookId))
                .ReturnsAsync(expectedBook);

            // Act
            var result = await _service.GetBookAsync(bookId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedBook, result);
        }

        [Fact]
        public async Task GetBookAsync_ReturnsNull_WhenIdDoesNotExist()
        {
            // Arrange
            var nonExistentId = 999;
            _mockRepo.Setup(repo => repo.GetByIdAsync(nonExistentId))
                .ReturnsAsync((Book)null);

            // Act
            var result = await _service.GetBookAsync(nonExistentId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetBooksAdvancedAsync_ParsesPriceRange_WhenProvided()
        {
            // Arrange
            var sieveModel = new SieveModel();
            var priceRange = "10-30";
            decimal minPrice = 10;
            decimal maxPrice = 30;
            
            _mockRepo.Setup(repo => repo.GetAdvancedAsync(sieveModel, minPrice, maxPrice))
                .ReturnsAsync(_books.Where(b => b.Price >= minPrice && b.Price <= maxPrice).ToList());

            // Act
            var result = await _service.GetBooksAdvancedAsync(sieveModel, priceRange);

            // Assert
            Assert.Equal(2, result.Count());
            _mockRepo.Verify(repo => repo.GetAdvancedAsync(sieveModel, minPrice, maxPrice), Times.Once);
        }

        [Fact]
        public async Task GetBooksAdvancedAsync_HandlesInvalidPriceRange()
        {
            // Arrange
            var sieveModel = new SieveModel();
            var invalidPriceRange = "invalid";
            
            _mockRepo.Setup(repo => repo.GetAdvancedAsync(sieveModel, null, null))
                .ReturnsAsync(_books);

            // Act
            var result = await _service.GetBooksAdvancedAsync(sieveModel, invalidPriceRange);

            // Assert
            Assert.Equal(2, result.Count());
            _mockRepo.Verify(repo => repo.GetAdvancedAsync(sieveModel, null, null), Times.Once);
        }
    }
}