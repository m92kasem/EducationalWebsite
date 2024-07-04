using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EducationalWebsite.Domain.Entities;
using EducationalWebsite.Domain.Interfaces;
using EducationalWebsite.Infrastructure.MongoDB;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace EducationalWebsite.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _user;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(MongoDatabaseManager mongoDatabaseManager, ILogger<UserRepository> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            
            if (mongoDatabaseManager == null)
            {
                _logger.LogError("MongoDatabaseManager is null");
                throw new ArgumentNullException(nameof(mongoDatabaseManager));
            }

            _user = mongoDatabaseManager.Users;
        }
        
        public async Task<User> AddAsync(User user)
        {
            try
            {
                await _user.InsertOneAsync(user);
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding user.");
                throw;
            }
        }

        public async Task<User> AuthenticateAsync(string email, string password)
        {
            try
            {
                var user = await _user.Find(u => u.Email.Value == email).FirstOrDefaultAsync();
                if (user != null && user.Password.Equals(password))
                {
                    return user;
                }
                return null!;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while authenticating user.");
                throw;
            }
        }

        public async Task DeleteAsync(Guid Id)
        {
            try
            {
                await _user.DeleteOneAsync(u => u.Id == Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting user.");
                throw;
            }
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            try
            {
                var users = await _user.Find(u => true).ToListAsync();
                return users;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all users.");
                throw;
            }
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            try
            {
                var user = await _user.Find(u => u.Email.Value == email).FirstOrDefaultAsync();
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching user by email.");
                throw;
            }
        }

        public async Task<User> GetByIdAsync(Guid Id)
        {
            try
            {
                var user = await _user.Find(u => u.Id == Id).FirstOrDefaultAsync();
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching user by Id.");
                throw;
            }
        }

        public async Task UpdateAsync(User user)
        {
            try
            {
                await _user.ReplaceOneAsync(u => u.Id == user.Id, user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating user.");
                throw;
        }
        }
    }
}