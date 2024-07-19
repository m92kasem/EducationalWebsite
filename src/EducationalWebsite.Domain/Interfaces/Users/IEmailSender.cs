using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EducationalWebsite.Domain.Interfaces.Users
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}