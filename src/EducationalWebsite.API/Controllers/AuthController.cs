using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EducationalWebsite.Application.Commands;
using EducationalWebsite.Application.Commands.Auth;
using EducationalWebsite.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EducationalWebsite.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AuthController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthController(IMediator mediator, ILogger<AuthController> logger, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _mediator = mediator;
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;

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

        [HttpGet("login-google")]
        public IActionResult LoginGoogle()
        {
            var properties = new AuthenticationProperties { RedirectUri = Url.Action("GoogleResponse") };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

         [HttpGet("google-response")]
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
            if (!result.Succeeded)
                return BadRequest(); // Handle the error as you wish

            var email = result.Principal.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true
                };

                var identityResult = await _userManager.CreateAsync(user);
                if (identityResult.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "USER");
                }
                else
                {
                    return BadRequest(identityResult.Errors);
                }
            }

            await _signInManager.SignInAsync(user, isPersistent: false);
            return Redirect("~/"); // Redirect to your front-end application
        }

    }
}