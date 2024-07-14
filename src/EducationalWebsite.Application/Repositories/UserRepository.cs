using EducationalWebsite.Domain.Entities;
using EducationalWebsite.Domain.Interfaces;
using EducationalWebsite.Infrastructure.MongoDB;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationalWebsite.Application.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _users;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(MongoDatabaseManager mongoDatabaseManager, ILogger<UserRepository> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            if (mongoDatabaseManager == null)
            {
                _logger.LogError("MongoDatabaseManager is null");
                throw new ArgumentNullException(nameof(mongoDatabaseManager));
            }

            _users = mongoDatabaseManager.Users;
        }

        public async Task AddAsync(User user)
        {
            await _users.InsertOneAsync(user);
        }

        public async Task<User> AuthenticateAsync(string email, string password)
        {
            var user = await _users.Find(u => u.Email.Equals(email) && u.Password.Equals(password)).FirstOrDefaultAsync();
            return user;
        }

        public async Task DeleteAsync(Guid Id)
        {
            await _users.DeleteOneAsync(u => u.Id == Id);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            var users = await _users.Find(u => true).ToListAsync();
            return users;
        }

        public async Task<User> GetByIdAsync(Guid Id)
        {
            var user = await _users.Find(u => u.Id == Id).FirstOrDefaultAsync();
            return user;
        }


        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _users.Find(u => u.Email.Equals(email)).FirstOrDefaultAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            await _users.ReplaceOneAsync(u => u.Id == user.Id, user);
        }
    }
}
