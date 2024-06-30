using System;
using System.Linq;
using System.Threading.Tasks;
using EducationalWebsite.Infrastructure.MongoDB;
using Microsoft.Extensions.Logging;
using Mongo2Go;
using MongoDB.Driver;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace EducationalWebsite.Tests.Helpers
{
    public class DbTest : IDisposable
    {
        private readonly ITestOutputHelper _output;
        private readonly Mock<ILogger<MongoDatabaseManager>> _mockLogger;
        private readonly MongoDbRunner _runner;
        private readonly MongoDbConnection _mongoDbConnection;

        public DbTest(ITestOutputHelper output)
        {
            _output = output;
            _mockLogger = new Mock<ILogger<MongoDatabaseManager>>();
            _runner = MongoDbRunner.Start();

            _mongoDbConnection = new MongoDbConnection
            {
                ConnectionString = _runner.ConnectionString,
                DatabaseName = "TestDb"
            };

            _output.WriteLine($"MongoDB connection string: {_runner.ConnectionString}");
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

            Assert.Throws<ArgumentNullException>(() => new MongoDatabaseManager(_mongoDbConnection, null));
            _output.WriteLine("ArgumentNullException thrown for null logger.");
        }

        [Fact]
        public async Task GetCollection_ValidType_ReturnsCollection()
        {
            // Arrange
            var manager = new MongoDatabaseManager(_mongoDbConnection,  _mockLogger.Object);

            // Act
            var collection = manager.GetCollection<TestDocument>();

            // Assert
            Assert.NotNull(collection);
            _output.WriteLine("Collection retrieved successfully.");

            // Additional test
            var document = new TestDocument { Name = "Test" };
            await collection.InsertOneAsync(document);

            var documents = await collection.Find(_ => true).ToListAsync();
            Assert.NotNull(documents);
            Assert.Single(documents);
            Assert.Equal("Test", documents.First().Name);
            _output.WriteLine("Document inserted and retrieved successfully.");
        }

        public void Dispose()
        {
            _runner?.Dispose();
        }

        public class TestDocument
        {
            public Guid Id { get; set; } = Guid.NewGuid();
            public string Name { get; set; }
        }
    }
}
