using AutoMapper;
using EducationalWebsite.Application.Commands.Users;
using EducationalWebsite.Application.DTOs;
using EducationalWebsite.Application.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EducationalWebsite.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;


        public UsersController(IMediator mediator, IUserService userService, ILogger<UsersController> logger)
        {
            _mediator = mediator;
            _userService = userService;
            _logger = logger;
        }
        

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CreateUserDto createUserDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var command = new CreateUserCommand(createUserDto);
            await _mediator.Send(command);
            return Ok();
        }
        [HttpPost("register2")]
        public async Task<IActionResult> Register2([FromBody] CreateUserDto createUserDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _userService.RegisterUserAsync(createUserDto);
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (loginDto == null)
            {
                _logger.LogError("User login attempt with null data.");
                return BadRequest("Login data is null.");
            }

            try
            {
                var userDto = await _userService.LoginUserAsync(loginDto.Email, loginDto.Password);
                if (userDto == null)
                {
                    return Unauthorized("Invalid credentials.");
                }

                return Ok(userDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while logging in the user.");
                return StatusCode(500, "Internal server error.");
            }
        }
    }
}
