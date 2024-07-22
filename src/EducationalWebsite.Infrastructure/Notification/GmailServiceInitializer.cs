using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Gmail.v1;
using Google.Apis.Services;
using Microsoft.Extensions.Configuration;

namespace EducationalWebsite.Infrastructure.Notification
{
    public class GmailServiceInitializer
    {
        private readonly IConfiguration _configuration;

        public GmailServiceInitializer(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }


        public async Task<GmailService> InitializeAsync()
        {
            var clientId = _configuration["GmailSettings:ClientId"];
            var clientSecret = _configuration["GmailSettings:ClientSecret"];
            var refreshToken = _configuration["GmailSettings:RefreshToken"];
            var applicationName = _configuration["GmailSettings:ApplicationName"];

            var secrets = new ClientSecrets
            {
                ClientId = clientId,
                ClientSecret = clientSecret
            };

            var token = new TokenResponse { RefreshToken = refreshToken };

            var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = secrets
            });

            var credential = new UserCredential(flow, "user", token);
            if (credential.Token.IsStale)
            {
                await credential.RefreshTokenAsync(default).ConfigureAwait(false);
            }
            return await Task.FromResult(new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = applicationName,
            }));
        }
    }
}