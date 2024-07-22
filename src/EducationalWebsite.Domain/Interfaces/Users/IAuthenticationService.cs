using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EducationalWebsite.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace EducationalWebsite.Domain.Interfaces.Users
{
    public interface IAuthenticationService
    {
      Task<IdentityResult> RegisterUserAsync(ApplicationUser user, string password);
      Task<(SignInResult signInResult, string token)> LoginUserAsync(string email, string password);
      Task LogoutUserAsync();  
      Task<IdentityResult> ResetPasswordAsync(string email, string token, string newPassword);
      Task GeneratePasswordResetTokenAsync(string email);
      Task<bool> UserExistsAsync(string email);
    }
}