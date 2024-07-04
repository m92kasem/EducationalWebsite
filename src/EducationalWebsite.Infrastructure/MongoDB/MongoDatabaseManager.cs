using System;
using EducationalWebsite.Domain.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Events;

namespace EducationalWebsite.Infrastructure.MongoDB
{
    public class MongoDatabaseManager
    {
        private readonly IMongoClient _client;
        private readonly IMongoDatabase _database;
        private readonly ILogger<MongoDatabaseManager> _logger;

        public MongoDatabaseManager(IOptions<MongoDbConnection> connection, ILogger<MongoDatabaseManager> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            if (connection?.Value == null)
            {
                _logger.LogError("MongoDbConnection is null");
                throw new ArgumentNullException(nameof(connection));
            }

            ValidateConfiguration(connection.Value.ConnectionString!, connection.Value.DatabaseName!);

            // Create a MongoClientSettings object from the connection string
            var clientSettings = MongoClientSettings.FromConnectionString(connection.Value.ConnectionString);
            // Configure logging for MongoDB events
            ConfigureLogging(clientSettings);

            // Enable TLS 1.3
            clientSettings.SslSettings = new SslSettings { EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls13 };


            try
            {
                _client = new MongoClient(clientSettings);
                _database = _client.GetDatabase(connection.Value.DatabaseName);
                _logger.LogInformation("MongoDB connection established.");
            }
            catch (MongoConfigurationException ex)
            {
                _logger.LogError(ex, "Error establishing MongoDB connection - client.");
                throw;
            }
            catch (MongoConnectionException ex)
            {
                _logger.LogError(ex, "Error establishing MongoDB connection - Failed to connect MDB.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while initializing MongoDB client.");
                throw;
            }
        }

        // GetCollection method to retrieve a collection from the database
        public IMongoCollection<T> GetCollection<T>()
        {
            var collectionName = typeof(T).Name;
            if (string.IsNullOrEmpty(collectionName))
            {
                _logger.LogError("Collection name is null or empty.");
                throw new ArgumentException("Collection name is null or empty.", nameof(collectionName));
            }

            try
            {
                var collection = _database.GetCollection<T>(collectionName);
                _logger.LogInformation($"Collection {collectionName} retrieved successfully.");
                return collection;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred while retrieving collection {collectionName}.");
                throw;
            }
        }

        public IMongoCollection<User> Users => _database.GetCollection<User>("Users");

        // ValidateConfiguration method to validate the connection string and database name
        private void ValidateConfiguration(string connectionString, string databaseName)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                _logger.LogError("MongoDB connection string must be provided.");
                throw new ArgumentException("MongoDB connection string must be provided.", nameof(connectionString));
            }

            if (string.IsNullOrEmpty(databaseName))
            {
                _logger.LogError("MongoDB database name must be provided.");
                throw new ArgumentException("MongoDB database name must be provided.", nameof(databaseName));
            }

            if (!Uri.TryCreate(connectionString, UriKind.Absolute, out _))
            {
                _logger.LogError("Invalid MongoDB connection string format.");
                throw new ArgumentException("Invalid MongoDB connection string format.", nameof(connectionString));
            }
        }

        // ConfigureLogging method to configure logging for MongoDB events
        private void ConfigureLogging(MongoClientSettings clientSettings)
        {
            clientSettings.ClusterConfigurator = cb =>
            {
                cb.Subscribe<CommandStartedEvent>(e =>
                    _logger.LogInformation($"Command {e.CommandName} - {e.Command.ToJson()} started."));
                cb.Subscribe<CommandSucceededEvent>(e =>
                    _logger.LogInformation($"Command {e.CommandName} - {e.Reply.ToJson()} succeeded."));
                cb.Subscribe<CommandFailedEvent>(e =>
                    _logger.LogError(e.Failure, $"Command {e.CommandName} failed."));
            };
        }
        
    }
}