using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EducationalWebsite.Domain.Entities;

namespace EducationalWebsite.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(Guid Id);
        Task<IEnumerable<User>> GetAllAsync();
        Task AddAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteAsync(Guid Id);
        Task<User> GetUserByEmailAsync(string email);
        Task<User> AuthenticateAsync(string Email, string password);
    }
}