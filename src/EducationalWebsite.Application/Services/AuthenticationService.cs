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
    public class AuthenticationService : IAuthenticationService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly ILogger<AuthenticationService> _logger;
        private readonly IEmailSender _emailSender;

        public AuthenticationService(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IJwtTokenGenerator jwtTokenGenerator,
            ILogger<AuthenticationService> logger,
            IEmailSender emailSender)
        {
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _jwtTokenGenerator = jwtTokenGenerator ?? throw new ArgumentNullException(nameof(jwtTokenGenerator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
        }

        public async Task<(SignInResult signInResult, string token)> LoginUserAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                _logger.LogError($"User with email {email} not found.");
                return (SignInResult.Failed, string.Empty);
            }

            var result = await _signInManager.PasswordSignInAsync(user, password, false, false);
            if (!result.Succeeded)
            {
                _logger.LogError($"An error occurred while logging in user with email {email}.");
                return (result, string.Empty);
            }

            var token = _jwtTokenGenerator.GenerateToken(user);
            return (result, token);
        }

        public async Task LogoutUserAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> ResetPasswordAsync(string email, string token, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                _logger.LogError($"User with email {email} not found.");
                return IdentityResult.Failed(new IdentityError { Description = $"User with email {email} not found." });
            }

            try
            {
                var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
                if(result.Succeeded)
                {
                    _logger.LogInformation($"Password reset for user with email {email} successful.");
                }
                else
                {
                    _logger.LogError($"An error occurred while resetting password for user with email {email}.");
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred while resetting password for user with email {email}");
                throw;
            }
        }

        public async Task GeneratePasswordResetTokenAsync (string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                _logger.LogError($"User with email {email} not found.");
                return;
            }

            try
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                // Send email with token
                var callbackUrl = $"https://localhost:5001/reset-password?email={email}&token={token}";
                var message = $"Please reset your password by clicking <a href='{callbackUrl}'>here</a> or by copying the following link: {callbackUrl} into your browser. This link will expire in 24 hours.";
                await _emailSender.SendEmailAsync(email, "Reset Password", message);

                Console.WriteLine(token);

                _logger.LogInformation($"Password reset token for user with email {email} generated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred while generating password reset token for user with email {email}");
                throw;
            }
        }
    }
}