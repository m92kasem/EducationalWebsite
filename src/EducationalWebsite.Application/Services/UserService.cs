using AutoMapper;
using EducationalWebsite.Application.DTOs;
using EducationalWebsite.Domain.Entities;
using EducationalWebsite.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace EducationalWebsite.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher _passwordHasher;

        public UserService(IUserRepository userRepository, IMapper mapper, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
        }

        public async Task RegisterUserAsync(CreateUserDto createUserDto)
        {
            var user = _mapper.Map<User>(createUserDto);
            //user.Password = ConvertToSecureString(_passwordHasher.HashPassword(ConvertToSecureString(createUserDto.Password)));
            Console.WriteLine("Password: " + createUserDto.Password);
            await _userRepository.AddAsync(user);
        }

        public async Task<UserDto> LoginUserAsync(string email, string password)
        {
            var hashedPassword = _passwordHasher.HashPassword(ConvertToSecureString(password));
            Console.WriteLine("Hashed Password: " + hashedPassword);
            var user = await _userRepository.AuthenticateAsync(email, hashedPassword);
            if (user != null)
            {
                return _mapper.Map<UserDto>(user);
            }
            
            return null;
        }

        private SecureString ConvertToSecureString(string password)
        {
            var secureString = new SecureString();
            foreach (char c in password)
            {
                secureString.AppendChar(c);
            }
            secureString.MakeReadOnly();
            return secureString;
        }

        public Task<UserDto?> GetUserByEmailAsync(LoginDto loginDto)
        {
            throw new NotImplementedException();
        }
    }
}
