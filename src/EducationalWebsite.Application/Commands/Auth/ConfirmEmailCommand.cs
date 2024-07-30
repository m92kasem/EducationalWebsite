using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace EducationalWebsite.Application.Commands.Auth
{
    public class ConfirmEmailCommand : IRequest<IdentityResult>
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }
}