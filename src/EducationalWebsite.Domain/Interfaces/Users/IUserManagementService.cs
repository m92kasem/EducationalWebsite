using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EducationalWebsite.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace EducationalWebsite.Domain.Interfaces.Users
{
    public interface IUserManagementService
    {
        Task<IdentityResult> RegisterUserAsync(ApplicationUser user, string password);
        Task<ApplicationUser> GetUserByIdAsync(Guid userId);
        Task<ApplicationUser> GetUserByEmailAsync(string email);
        Task<List<ApplicationUser>> GetAllUsersAsync();
        Task<ApplicationUser> GetUserByUsernameAsync(string username);
        Task<IdentityResult> UpdateUserAsync(Guid userId, ApplicationUser user);
        Task<IdentityResult> DeleteUserAsync(Guid userId);
        Task<bool> UserExistsAsync(string email);
    }
}