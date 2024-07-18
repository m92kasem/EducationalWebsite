using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EducationalWebsite.Application.Commands;
using EducationalWebsite.Application.Dtos;
using EducationalWebsite.Domain.Entities;
using EducationalWebsite.Domain.Interfaces.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace EducationalWebsite.Application.Handlers
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, (SignInResult, string)>
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ILogger<LoginCommandHandler> _logger;

        public LoginCommandHandler(IAuthenticationService authenticationService, ILogger<LoginCommandHandler> logger)
        {
            _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<(SignInResult, string)> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var (result, token) = await _authenticationService.LoginUserAsync(request.Email, request.Password);

            if (!result.Succeeded)
            {
                _logger.LogError($"An error occurred while logging in user with email {request.Email}.");
                return (result, string.Empty);
            }

            return (result, token);
            
        }
    }

}