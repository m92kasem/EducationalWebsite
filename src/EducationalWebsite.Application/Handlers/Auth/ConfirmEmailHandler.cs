using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EducationalWebsite.Application.Commands.Auth;
using EducationalWebsite.Domain.Interfaces.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace EducationalWebsite.Application.Handlers.Auth
{
    public class ConfirmEmailHandler : IRequestHandler<ConfirmEmailCommand, IdentityResult>
    {
        private readonly IAuthenticationService _authenticationService;

        public ConfirmEmailHandler(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
        }

        public Task<IdentityResult> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            return _authenticationService.ConfirmEmailAsync(request.Email, request.Token);
        }

    }
}