using EducationalWebsite.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EducationalWebsite.Application.Services
{
    public interface IUserService
    {
        Task RegisterUserAsync(CreateUserDto createUserDto);
        Task<UserDto> LoginUserAsync(string email, string password);
        Task<UserDto?> GetUserByEmailAsync(LoginDto loginDto);
    }
}