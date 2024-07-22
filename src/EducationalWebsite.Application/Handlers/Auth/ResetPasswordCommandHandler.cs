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
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, IdentityResult>

    {
        private readonly IAuthenticationService _authenticationService;

        public ResetPasswordCommandHandler(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
        }

        public async Task<IdentityResult> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            await _authenticationService.ResetPasswordAsync(request.Email, request.Token, request.NewPassword).ConfigureAwait(false);
            return IdentityResult.Success;
        }
        
    }
}