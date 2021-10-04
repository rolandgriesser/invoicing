using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using invoicing.server.Data;
using invoicing.server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace invoicing.server.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly InvoicingDbContext _dbContext;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;
        public UserController(UserManager<User> userManager,
            InvoicingDbContext dbContext,
            SignInManager<User> signInManager,
            IConfiguration configuration,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _signInManager = signInManager;
            _configuration = configuration;
            _emailSender = emailSender;
        }
        
        public class RegisterDto {
            //public Guid? OrganizationId { get; set; }
            public string OrganizationName { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class RegisterResultDto {
            public bool Success { get; set; }
            public string Token { get; set; }
            public IEnumerable<IdentityError> Errors { get; set; }
        }

        [HttpPost("register")]       
        [AllowAnonymous] 
        public async Task<ActionResult<RegisterResultDto>> Register([FromBody]RegisterDto register)
        {
            Organization organization = null;
            if(!string.IsNullOrEmpty(User.Identity.Name))
            {
                var existingUser = await _userManager.FindByNameAsync(User.Identity.Name);
                organization = _dbContext.Organizations.FirstOrDefault(i => i.Id == existingUser.OrganizationId);
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

                // var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                // var callbackUrl = Url.Page(
                //     "/user/confirm",
                //     pageHandler: null,
                //     values: new { userId = user.Id, code = code },
                //     protocol: Request.Scheme);

                // await _emailSender.SendEmailAsync(user.Email, "Confirm your email",
                //     $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                await _signInManager.SignInAsync(user, isPersistent: false);
                return new RegisterResultDto() { Success = true, Token = GetToken(user) };
            }
            return new RegisterResultDto() { Success = false, Errors = result.Errors };
        }

        // [HttpGet("confirm")] 
        // [AllowAnonymous]
        // public async Task<ActionResult<bool>> ConfirmEmail(string userId, string code) {
        //     return true;
        // }

        public class UserDto {
            public string Email { get; set; }
            public string Password { get; set; }
        }
        
        [HttpPost("token")]
        [AllowAnonymous] 
        public async Task<ActionResult> Authorize([FromBody]UserDto userDto) 
        {
            var result = await _signInManager.PasswordSignInAsync(userDto.Email, userDto.Password, false, false);
            if(result.Succeeded) {
                var user = await _userManager.FindByEmailAsync(userDto.Email);

                return Ok(new { token = GetToken(user) });
            }
            return Unauthorized();
        }

        [HttpPost]
        [Route("refreshtoken")]
        public async Task<IActionResult> RefreshToken()
        {
            if(string.IsNullOrEmpty(User.Identity.Name)) return Unauthorized();
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            return Ok(GetToken(user));
        }

        private String GetToken(User user)
        {
            var utcNow = DateTime.UtcNow;

            var claims = new Claim[]
            {
                        new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                        new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, utcNow.ToString())
            };

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<String>("Tokens:Key")));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            var jwt = new JwtSecurityToken(
                signingCredentials: signingCredentials,
                claims: claims,
                notBefore: utcNow,
                expires: utcNow.AddSeconds(_configuration.GetValue<int>("Tokens:Lifetime")),
                audience: _configuration.GetValue<String>("Tokens:Audience"),
                issuer: _configuration.GetValue<String>("Tokens:Issuer")
                );

            return new JwtSecurityTokenHandler().WriteToken(jwt);

        }
    }
}