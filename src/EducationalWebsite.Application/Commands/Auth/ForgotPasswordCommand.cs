using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace EducationalWebsite.Application.Commands.Auth
{
    public class ForgotPasswordCommand : IRequest<Unit>
    {
        public string Email { get; set; }
    }
}