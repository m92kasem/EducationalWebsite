using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EducationalWebsite.Application.Dtos;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace EducationalWebsite.Application.Commands
{
    public class LoginCommand : IRequest<(SignInResult, string)>
{
    public string Email { get; set; }
    public string Password { get; set; }
}
}