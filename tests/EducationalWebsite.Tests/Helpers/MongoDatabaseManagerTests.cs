using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EducationalWebsite.Infrastructure.MongoDB;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Moq;
using Xunit.Abstractions;

namespace EducationalWebsite.Tests.Helpers
{
    public class MongoDatabaseManagerTests
    {
        private readonly Mock<ILogger<MongoDatabaseManager>> _mockLogger;
        private readonly ITestOutputHelper _output;
        private readonly MongoDbConnection _mongoDbConnection;

        public MongoDatabaseManagerTests(ITestOutputHelper output)
        {
            _mockLogger = new Mock<ILogger<MongoDatabaseManager>>();
            _output = output;
            _mongoDbConnection = new MongoDbConnection
            {
                ConnectionString = "mongodb://localhost:27017",
                DatabaseName = "TestDatabase"
            };

            _output.WriteLine($"MongoDB connection string: {_mongoDbConnection.ConnectionString}");
            _output.WriteLine($"MongoDB database name: {_mongoDbConnection.DatabaseName}");
        }

        [Fact]
        public void Constructor_ValidConnection_InitializesCorrectly()
        {
            // Act
            var manager = new MongoDatabaseManager(_mongoDbConnection, _mockLogger.Object);

            // Assert
            Assert.NotNull(manager);
            _output.WriteLine("MongoDatabaseManager initialized successfully.");
        }

        [Fact]
        public void Constructor_NullConnection_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new MongoDatabaseManager(null, _mockLogger.Object));
            _output.WriteLine("ArgumentNullException thrown for null connection.");
        }

        [Fact]
        public void Constructor_NullLogger_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new MongoDatabaseManager(_mongoDbConnection, null));
            _output.WriteLine("ArgumentNullException thrown for null logger.");
        }

        [Fact]
        public void GetCollection_ValidType_ReturnsCollection()
        {
            // Arrange
            var manager = new MongoDatabaseManager(_mongoDbConnection, _mockLogger.Object);

            // Act
            var collection = manager.GetCollection<TestDocument>();

            // Assert
            Assert.NotNull(collection);
            _output.WriteLine("Collection retrieved successfully.");
        }
        

    }

    public class TestDocument
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
    }

    
}