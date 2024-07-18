using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EducationalWebsite.Application.Commands;
using EducationalWebsite.Application.Dtos;
using EducationalWebsite.Domain.Entities;
using EducationalWebsite.Domain.Interfaces.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace EducationalWebsite.Application.Handlers
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
    {
        private readonly IUserManagementService _userService;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateUserCommandHandler> _logger;

        public CreateUserCommandHandler(IUserManagementService userService, IMapper mapper, ILogger<CreateUserCommandHandler> logger)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        

        public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var userExists = await _userService.UserExistsAsync(request.Email);
            if (userExists)
            {
                _logger.LogWarning($"User with email {request.Email} already exists.");
                IdentityResult.Failed(new IdentityError { Description = $"User with email {request.Email} already exists." });
            }
            try
            {
                var user = _mapper.Map<ApplicationUser>(request);
                var result = await _userService.RegisterUserAsync(user, request.Password);
 
                return _mapper.Map<UserDto>(user);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                throw new Exception(ex.Message, ex);
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred while registering user with email {request.Email}");
                throw new Exception($"An error occurred while registering user with email {request.Email}: {ex.Message}", ex);
            }
        }
    }
}