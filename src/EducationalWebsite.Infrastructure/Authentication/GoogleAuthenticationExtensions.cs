using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EducationalWebsite.Domain.Entities;
using Google.Apis.Gmail.v1;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EducationalWebsite.Infrastructure.Authentication
{
    public static class GoogleAuthenticationExtensions
    {
        public static void AddGoogleAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication().AddGoogle(options =>
            {
                options.ClientId = configuration["GmailSettings:ClientId"] ?? throw new Exception("GmailSettings ClientId is missing");
                options.ClientSecret = configuration["GmailSettings:ClientSecret"] ?? throw new Exception("GmailSettings ClientSecret is missing");
                options.Scope.Add(GmailService.Scope.GmailSend);
                options.SaveTokens = true;
                options.Events.OnCreatingTicket = async ctx =>
                {
                    await HandleGoogleUserAsync(ctx);
                };
            });
        }

        private static async Task HandleGoogleUserAsync(OAuthCreatingTicketContext ctx)
        {
            var userEmail = ctx.Identity.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            if (userEmail == null) return;

            var userManager = ctx.HttpContext.RequestServices.GetRequiredService<UserManager<ApplicationUser>>();
            var user = await userManager.FindByEmailAsync(userEmail);
            if (user != null) return;

            var newUser = new ApplicationUser
            {
                UserName = userEmail,
                Email = userEmail,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(newUser);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(newUser, "USER");
            }
        }
    }
}