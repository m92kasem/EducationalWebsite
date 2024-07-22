using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EducationalWebsite.Application.Dtos;
using EducationalWebsite.Domain.Entities;
using EducationalWebsite.Domain.Interfaces.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace EducationalWebsite.Application.Services
{
    public class UserManagementService : IUserManagementService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<UserManagementService> _logger;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public UserManagementService(
            IUserRepository userRepository,
            ILogger<UserManagementService> logger,
            UserManager<ApplicationUser> userManager,
            IJwtTokenGenerator jwtTokenGenerator)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _jwtTokenGenerator = jwtTokenGenerator ?? throw new ArgumentNullException(nameof(jwtTokenGenerator));
        }

        public async Task<IdentityResult> DeleteUserAsync(Guid userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId.ToString()).ConfigureAwait(false);
                if (user == null)
                {
                    _logger.LogWarning($"User with id {userId} not found.");
                    return IdentityResult.Failed(new IdentityError { Description = $"User with id {userId} not found." });
                }
                return await _userManager.DeleteAsync(user).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred while deleting user with id {userId}");
                return IdentityResult.Failed(new IdentityError { Description = $"An unexpected error occurred while deleting user with id {userId}" });
            }
        }

        public async Task<List<ApplicationUser>> GetAllUsersAsync()
        {
            try
            {
                return await _userRepository.GetAllUsersAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while retrieving all users");
                throw;
            }
        }

        public async Task<ApplicationUser> GetUserByEmailAsync(string email)
        {
            try
            {
                return await _userManager.FindByEmailAsync(email).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred while retrieving user with email {email}");
                throw;
            }
        }

        public async Task<ApplicationUser> GetUserByIdAsync(Guid userId)
        {
            try
            {
                return await _userManager.FindByIdAsync(userId.ToString()).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred while retrieving user with id {userId}");
                throw;
            }
        }

        public async Task<ApplicationUser> GetUserByUsernameAsync(string username)
        {
            try
            {
                return await _userManager.FindByNameAsync(username).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred while retrieving user with username {username}");
                throw;
            }
        }

        

        public async Task<IdentityResult> UpdateUserAsync(Guid userId, ApplicationUser user)
        {
            var userDB = await GetUserByIdAsync(userId).ConfigureAwait(false);
            if (userDB == null)
            {
                _logger.LogWarning($"User with id {userId} not found.");
                return IdentityResult.Failed(new IdentityError { Description = $"User with id {userId} not found." });
            }

            try
            {
                var result = await _userManager.UpdateAsync(userDB).ConfigureAwait(false);
                if (!result.Succeeded)
                {
                    _logger.LogError("User update failed: {Errors}", string.Join(", ", result.Errors));
                    return result;
                }

                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred while updating user with id {userId}");
                return IdentityResult.Failed(new IdentityError { Description = $"An unexpected error occurred while updating user with id {userId}: {ex.Message}" });
            }
        }

        
    }
}
