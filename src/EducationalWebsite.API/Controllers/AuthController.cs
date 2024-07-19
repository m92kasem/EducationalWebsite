using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EducationalWebsite.Application.Commands;
using EducationalWebsite.Application.Commands.Auth;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EducationalWebsite.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IMediator mediator, ILogger<AuthController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(CreateUserCommand command)
        {
            var user = await _mediator.Send(command);
            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginCommand command)
        {
            var authResult = await _mediator.Send(command);
            Console.WriteLine(authResult);
            return Ok(authResult);
        }
        
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordCommand command)
        {
            try
            {
                await _mediator.Send(command);
                return Ok(new { status = "success", message = "Password reset link sent to your email." });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while processing forgot password request.");
                return StatusCode(500, new { status = "error", message = "An error occurred while processing forgot password request." });
            }
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                if (result.Succeeded)
                {
                    return Ok(new { status = "success", message = "Password reset successful." });
                }
                return BadRequest(new { status = "error", message = "Password reset failed", errors = result.Errors });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while processing reset password request.");
                return StatusCode(500, new { status = "error", message = "An error occurred while processing reset password request." });
            }
        }
    }
}