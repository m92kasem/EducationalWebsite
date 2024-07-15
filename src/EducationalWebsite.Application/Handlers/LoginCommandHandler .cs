using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EducationalWebsite.Application.Commands;
using EducationalWebsite.Application.Dtos;
using EducationalWebsite.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace EducationalWebsite.Application.Handlers
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResult>
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public LoginCommandHandler(SignInManager<User> signInManager, UserManager<User> userManager, IJwtTokenGenerator jwtTokenGenerator)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<AuthResult> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new Exception("Invalid login attempt.");
            }

            var result = await _signInManager.PasswordSignInAsync(user, request.Password, false, false);
            if (!result.Succeeded)
            {
                throw new Exception("Invalid login attempt.");
            }

            var token = _jwtTokenGenerator.GenerateToken(user);
            return new AuthResult { Token = token };
        }
    }

}