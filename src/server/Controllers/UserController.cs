using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using invoicing.server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace invoicing.server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly Data.InvoicingDbContext _dbContext;
        public UserController(Data.InvoicingDbContext dbContext, UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }
        
        public class RegisterDto {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        [HttpPost("register")]       
        [AllowAnonymous] 
        public ActionResult<string> Register([FromBody]RegisterDto user)
        {
            return "Success";
        }
    }
}