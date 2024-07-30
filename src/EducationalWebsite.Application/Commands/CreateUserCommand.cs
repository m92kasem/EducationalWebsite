using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EducationalWebsite.Application.Dtos;
using EducationalWebsite.Domain.ValueObjects;
using MediatR;

namespace EducationalWebsite.Application.Commands
{
    public class CreateUserCommand : IRequest<UserDto>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public Address Address { get; set; }
        public string MobileNumber { get; set; }
    }
}