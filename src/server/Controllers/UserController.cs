using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using invoicing.server.Data;
using invoicing.server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace invoicing.server.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly InvoicingDbContext _dbContext;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IEmailSender _emailSender;
        public UserController(UserManager<User> userManager,
            InvoicingDbContext dbContext,
            SignInManager<IdentityUser> signInManager,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }
        
        public class RegisterDto {
            public Guid? OrganizationId { get; set; }
            public string OrganizationName { get; set; }
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
            Organization organization = null;
            if(register.OrganizationId.HasValue) 
            {
                //todo get organization of current logged in user
                // organization = _dbContext.Organizations.FirstOrDefault(i => i.Id == regi);
            }
            else 
            {
                if(string.IsNullOrEmpty(register.OrganizationName)) {
                    return new RegisterResultDto() {
                        Success = false,
                        Errors = new IdentityError[] {
                            new IdentityError() { Code = "OrganizationNameEmpty", Description = "OrganizationName can't be empty."}
                        }
                    };
                }
                organization = new Organization() {
                    Name = register.OrganizationName
                };
                _dbContext.Organizations.Add(organization);
                _dbContext.SaveChanges();
            }

            if(organization == null) {
                return new RegisterResultDto() {
                    Success = false,
                    Errors = new IdentityError[] {
                        new IdentityError() { Code = "OrganizationNull", Description = "The organization of the user couldn't be fetched or created."}
                    }
                };
            }

            var user = new User { UserName = register.Email, Email = register.Email, Organization = organization };
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
        public async Task<ActionResult<bool>> ConfirmEmail(string user) {
            return true;
        }
    }
}