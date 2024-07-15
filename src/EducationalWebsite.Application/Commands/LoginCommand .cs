using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EducationalWebsite.Application.Dtos;
using MediatR;

namespace EducationalWebsite.Application.Commands
{
    public class LoginCommand : IRequest<AuthResult>
{
    public string Email { get; set; }
    public string Password { get; set; }
}
}