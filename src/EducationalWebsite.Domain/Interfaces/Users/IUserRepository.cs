using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EducationalWebsite.Domain.Entities;

namespace EducationalWebsite.Domain.Interfaces.Users
{
    public interface IUserRepository
    {
        Task<ApplicationUser> GetUserByIdAsync(Guid Id);
        Task<List<ApplicationUser>> GetAllUsersAsync();
        Task CreateUserAsync(ApplicationUser user);
        Task UpdateUserAsync(Guid Id, ApplicationUser user);
        Task DeleteUserAsync(Guid Id);
        Task<ApplicationUser> GetByUsernameAsync(string username);
        Task<ApplicationUser> GetUserByEmailAsync(string email);
    }
}