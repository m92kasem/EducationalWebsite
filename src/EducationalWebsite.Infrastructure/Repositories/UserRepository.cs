using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EducationalWebsite.Domain.Entities;
using EducationalWebsite.Domain.Interfaces.Users;
using EducationalWebsite.Infrastructure.MongoDB;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace EducationalWebsite.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<ApplicationUser> _users;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(MongoDatabaseManager mongoDatabaseManager, ILogger<UserRepository> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            if (mongoDatabaseManager == null)
            {
                _logger.LogError("MongoDatabaseManager is null");
                throw new ArgumentNullException(nameof(mongoDatabaseManager));
            }

            _users = mongoDatabaseManager._database.GetCollection<ApplicationUser>("applicationUsers");
        }

        public async Task CreateUserAsync(ApplicationUser user)
        {
            await _users.InsertOneAsync(user);
        }

        public async Task DeleteUserAsync(Guid userId)
        {
            await _users.DeleteOneAsync(u => u.Id == userId);
        }

        public async Task<List<ApplicationUser>> GetAllUsersAsync()
        {
            var users = await _users.Find(u => true).ToListAsync();
            return users;
        }

        public async Task<ApplicationUser> GetByUsernameAsync(string username)
        {
            var user = await _users.Find(u => u.UserName == username).FirstOrDefaultAsync();
            return user;
        }

        public async Task<ApplicationUser> GetUserByEmailAsync(string email)
        {
            var user = await _users.Find(u => u.Email == email).FirstOrDefaultAsync();
            return user;
        }

        public async Task<ApplicationUser> GetUserByIdAsync(Guid Id)
        {
            var user = await _users.Find(u => u.Id == Id).FirstOrDefaultAsync();
            return user;
        }

        public async Task UpdateUserAsync(Guid userId, ApplicationUser user)
        {
            await _users.FindOneAndReplaceAsync(u => u.Id == userId, user);

        }
    }
}