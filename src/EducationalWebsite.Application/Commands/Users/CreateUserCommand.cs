using EducationalWebsite.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationalWebsite.Application.Commands.Users
{
    public class CreateUserCommand : IRequest<CreateUserDto>
    {
        public CreateUserDto CreateUserDto { get; }

        public CreateUserCommand(CreateUserDto createUserDto)
        {
            CreateUserDto = createUserDto;
        }
    }
}
