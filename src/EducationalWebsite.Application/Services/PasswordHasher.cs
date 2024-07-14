using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace EducationalWebsite.Application.Services
{
    public class PasswordHasher : IPasswordHasher
    {
        public string HashPassword(SecureString password)
        {
            return BCrypt.Net.BCrypt.HashPassword(SecureStringToString(password));
        }

        public bool VerifyHashedPassword(string hashedPassword, SecureString password)
        {
            return BCrypt.Net.BCrypt.Verify(SecureStringToString(password), hashedPassword);
        }

        public string SecureStringToString(SecureString value)
        {
            IntPtr bstr = Marshal.SecureStringToBSTR(value);
            try
            {
                return Marshal.PtrToStringBSTR(bstr);
            }
            finally
            {
                Marshal.FreeBSTR(bstr);
            }
        }
    }
}
