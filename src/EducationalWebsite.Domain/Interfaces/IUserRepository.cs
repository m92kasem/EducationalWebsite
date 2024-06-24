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
        Task UpdateAsync(Guid Id, User user);
        Task DeleteAsync(Guid Id);
        Task<User> GetByUsernameAsync(string username);
        Task<User> AuthenticateAsync(string Email, string password);
    }
}