using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using invoicing.server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace invoicing.server.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IEmailSender _emailSender;
        public UserController(UserManager<User> userManager,
            SignInManager<IdentityUser> signInManager,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }
        
        public class RegisterDto {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class RegisterResultDto {
            public bool Success { get; set; }
            public IEnumerable<IdentityError> Errors { get; set; }
        }

        [HttpPost("register")]       
        [AllowAnonymous] 
        public async Task<ActionResult<RegisterResultDto>> Register([FromBody]RegisterDto register)
        {
            var user = new User { UserName = register.Email, Email = register.Email };
            var result = await _userManager.CreateAsync(user, register.Password);

            if (result.Succeeded)
            {
                //_logger.LogInformation("User created a new account with password.");

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var callbackUrl = Url.Page(
                    "/user/confirm",
                    pageHandler: null,
                    values: new { userId = user.Id, code = code },
                    protocol: Request.Scheme);

                await _emailSender.SendEmailAsync(user.Email, "Confirm your email",
                    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                // await _signInManager.SignInAsync(user, isPersistent: false);
                // return LocalRedirect(returnUrl);
                return new RegisterResultDto() { Success = true };
            }
            return new RegisterResultDto() { Success = false, Errors = result.Errors };
        }

        [HttpGet("confirm")] 
        public async Task<ActionResult<bool>> ConfirmEmail() {
            return true;
        }
    }
}