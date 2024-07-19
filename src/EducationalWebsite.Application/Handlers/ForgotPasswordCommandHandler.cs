using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EducationalWebsite.Application.Commands.Auth;
using EducationalWebsite.Domain.Interfaces.Users;
using MediatR;

namespace EducationalWebsite.Application.Handlers
{
    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, Unit>
    {
        private readonly IAuthenticationService _authenticationService;

        public ForgotPasswordCommandHandler(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
        }


        public async Task<Unit> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            await _authenticationService.GeneratePasswordResetTokenAsync(request.Email).ConfigureAwait(false);
            return Unit.Value;
        }
    }
}