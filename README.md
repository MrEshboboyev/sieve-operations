![.NET](https://img.shields.io/badge/.NET-10.0-512BD4)
![C#](https://img.shields.io/badge/C%23-10.0-239120)
![EF Core](https://img.shields.io/badge/EF%20Core-10.0-purple)
![API](https://img.shields.io/badge/API-REST-orange)
![License](https://img.shields.io/badge/License-MIT-blue)

## 📚 A modern API demonstrating advanced filtering, sorting, and pagination capabilities

SieveOperations is a clean, maintainable .NET API that showcases best practices for implementing flexible data querying using the Sieve library. This project serves as both a reference implementation and a starting point for building powerful, filterable APIs.

## ✨ Features

- 🔍 **Advanced Filtering** - Filter books by any property with complex conditions
- 🔢 **Dynamic Sorting** - Sort results by multiple fields in ascending or descending order
- 📄 **Smart Pagination** - Paginate results with metadata for easy navigation
- 🔄 **API Versioning** - Future-proof your API with built-in versioning
- 📊 **Custom Filters** - Create domain-specific filters like "yearsOld" or "priceRange"
- 📝 **Comprehensive Documentation** - OpenAPI specifications with detailed endpoint descriptions
- 🧪 **Testing Support** - Structure for unit and integration tests
- 🔒 **Security Best Practices** - Proper error handling and input validation

## 🚀 Getting Started

### Prerequisites

- .NET 10.0 SDK or later
- Your favorite IDE (Visual Studio, VS Code, JetBrains Rider)

### Installation

1. Clone the repository
```bash
git clone https://github.com/yourusername/SieveOperations.git
```

2. Navigate to the project directory
```bash
cd SieveOperations
```

3. Restore dependencies and build
```bash
dotnet restore
dotnet build
```

4. Run the application
```bash
dotnet run --project src/SieveOperations.Api/SieveOperations.Api.csproj
```

5. Open your browser and navigate to:
```
https://localhost:7001/swagger
```

## 📖 Usage Examples

### Basic Filtering

```
GET /api/v1/books?filters=author==J.R.R.%20Tolkien
```

### Multi-field Sorting

```
GET /api/v1/books?sorts=price,title,-publishedDate
```

### Pagination

```
GET /api/v1/books?page=2&pageSize=10
```

### Combined Operations

```
GET /api/v1/books?filters=genre==Fantasy&sorts=-publishedDate&page=1&pageSize=5
```

### Custom Filters

```
GET /api/v1/books?filters=yearsOld>50
GET /api/v1/books?filters=priceRange==10.99-19.99
```

## 🏗️ Architecture

```
SieveOperations/
├── src/
│   └── SieveOperations.Api/
│       ├── Controllers/
│       ├── Models/
│       ├── Services/
│       ├── Data/
│       ├── Configurations/
│       └── HealthChecks/
└── tests/
    └── SieveOperations.Tests/
        ├── Controllers/
        ├── Services/
        └── Integration/
```

## 🧩 Key Components

- **Repository Pattern** - Clean separation between data access and business logic
- **Service Layer** - Encapsulated business rules and domain logic
- **Standardized API Responses** - Consistent response format with metadata
- **Custom Sieve Processor** - Extended filtering capabilities for domain-specific needs

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

1. Fork the repository
2. Create your feature branch: `git checkout -b feature/amazing-feature`
3. Commit your changes: `git commit -m 'Add some amazing feature'`
4. Push to the branch: `git push origin feature/amazing-feature`
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 👏 Acknowledgements

- [Sieve](https://github.com/Biarity/Sieve) - The filtering/sorting/pagination library that powers this project
- [Entity Framework Core](https://github.com/dotnet/efcore) - The ORM used for data access
- [NSwag](https://github.com/RicoSuter/NSwag) - OpenAPI toolchain
