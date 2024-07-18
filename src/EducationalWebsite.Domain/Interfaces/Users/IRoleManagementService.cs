using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EducationalWebsite.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace EducationalWebsite.Domain.Interfaces.Users
{
    public interface IRoleManagementService
    {
        Task<bool> RoleExistsAsync(string roleName);
        Task<IdentityResult> CreateRoleAsync(ApplicationRole role);
        Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string roleName);
        Task<IdentityResult> UpdateRoleAsync(ApplicationRole role);
        Task<IdentityResult> RemoveFromRoleAsync(ApplicationUser user, string roleName);
    }
}