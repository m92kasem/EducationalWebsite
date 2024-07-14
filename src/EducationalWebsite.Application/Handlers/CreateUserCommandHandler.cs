using EducationalWebsite.Application.Commands.Users;
using EducationalWebsite.Application.DTOs;
using EducationalWebsite.Application.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationalWebsite.Application.Handlers
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, CreateUserDto>
    {
        private readonly IUserService _userService;

        public CreateUserCommandHandler(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<CreateUserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = request.CreateUserDto;
            await _userService.RegisterUserAsync(user);
            return user;
        }
    }
}
