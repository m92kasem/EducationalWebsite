using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EducationalWebsite.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EducationalWebsite.Infrastructure.Identity
{
    public static class IdentityExtensions
    {
        public static void AddIdentityExtensions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                // Configure identity options here
                options.SignIn.RequireConfirmedEmail = true; // Requires email confirmation
                options.Password.RequireDigit = true;  // Requires a digit
                options.Password.RequiredLength = 8; // Minimum password length
                options.Password.RequireNonAlphanumeric = false; // Requires a non-alphanumeric character
                options.Password.RequireUppercase = true; // Requires an uppercase letter
                options.Password.RequireLowercase = false; // Requires a lowercase letter
                options.User.RequireUniqueEmail = true; // Requires unique email
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);  // Lockout for 5 minutes
                options.Lockout.MaxFailedAccessAttempts = 5; // Lockout after 5 failed access attempts
                options.Lockout.AllowedForNewUsers = true; // Lockout enabled for new users
            })
            .AddMongoDbStores<ApplicationUser, ApplicationRole, Guid>(
                configuration["MongoDbConnection:ConnectionString"], 
                configuration["MongoDbConnection:DatabaseName"])
            .AddDefaultTokenProviders();
        }
        
    }
}