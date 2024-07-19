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

        public async Task<IdentityResult> RegisterUserAsync(ApplicationUser user, string password)
        {
            try
            {
                var createResult = await _userManager.CreateAsync(user, password).ConfigureAwait(false);
                if (createResult.Succeeded)
                {
                    _logger.LogInformation($"User with email {user.Email} registered successfully.");

                    var roleResult = await _userManager.AddToRoleAsync(user, "USER").ConfigureAwait(false);
                    if (roleResult.Succeeded)
                    {
                        _logger.LogInformation($"User with email {user.Email} added to role USER successfully.");
                    }
                    else
                    {
                        _logger.LogError($"An error occurred while adding user to role USER: {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
                        return roleResult;
                    }
                    
                }
                else
                {
                    _logger.LogError($"An error occurred while registering user with email {user.Email}: {string.Join(", ", createResult.Errors.Select(e => e.Description))}");
                    return createResult;
                }

                return createResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error registering user with email {Email}", user.Email);
                return IdentityResult.Failed(new IdentityError { Description = "An unexpected error occurred." });
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

        public async Task<bool> UserExistsAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email).ConfigureAwait(false) != null;
        }
    }
}
