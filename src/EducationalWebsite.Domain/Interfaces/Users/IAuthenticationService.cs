using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace EducationalWebsite.Domain.Interfaces.Users
{
    public interface IAuthenticationService
    {
      Task<(SignInResult signInResult, string token)> LoginUserAsync(string email, string password);
      Task LogoutUserAsync();  
    }
}