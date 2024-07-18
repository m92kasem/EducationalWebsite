using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EducationalWebsite.Domain.Entities;
using EducationalWebsite.Domain.Interfaces.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace EducationalWebsite.Application.Services
{
    public class RoleManagementService : IRoleManagementService
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RoleManagementService> _logger;

        public RoleManagementService(
            RoleManager<ApplicationRole> roleManager, 
            UserManager<ApplicationUser> userManager, 
            ILogger<RoleManagementService> logger)
        {
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> RoleExistsAsync(string roleName)
        {
            return await _roleManager.RoleExistsAsync(roleName);
        }

        public async Task<IdentityResult> CreateRoleAsync(ApplicationRole role)
        {
            var result = await _roleManager.CreateAsync(role);
            if (!result.Succeeded)
            {
                _logger.LogError($"An error occurred while creating role {role.Name}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
            return result;
        }

        public async Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string roleName)
        {
            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (!result.Succeeded)
            {
                _logger.LogError($"An error occurred while adding user {user.UserName} to role {roleName}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
            return result;
        }

        public async Task<IdentityResult> UpdateRoleAsync(ApplicationRole role)
        {
            var existingRole = await _roleManager.FindByIdAsync(role.Id.ToString());
            if (existingRole == null)
            {
                _logger.LogError($"Role with ID {role.Id} not found.");
                return IdentityResult.Failed(new IdentityError { Description = $"Role with ID {role.Id} not found." });
            }

            existingRole.Name = role.Name;
            var result = await _roleManager.UpdateAsync(existingRole);
            if (!result.Succeeded)
            {
                _logger.LogError($"An error occurred while updating role {role.Name}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
            return result;
        }

        public async Task<IdentityResult> RemoveFromRoleAsync(ApplicationUser user, string roleName)
        {
            var result = await _userManager.RemoveFromRoleAsync(user, roleName);
            if (!result.Succeeded)
            {
                _logger.LogError($"An error occurred while removing user {user.UserName} from role {roleName}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
            return result;
        }
    }
}