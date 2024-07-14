using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace EducationalWebsite.Application.Services
{
    public interface IPasswordHasher
    {
        string HashPassword(SecureString password);
        bool VerifyHashedPassword(string hashedPassword, SecureString password);
    }
}
