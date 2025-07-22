using Events.Data;
using Events.Dtos.auth;
using Events.Models;
using Events.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Events.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(JwtToken jwtToken,
    ApplicationDbContext dbContext,
    RoleManager<IdentityRole> roleManager,
    SignInManager<Users> signInManager,
    AspNetUserManager<Users> userManager) : ControllerBase
    {
        private readonly JwtToken _jwt = jwtToken;
        private readonly ApplicationDbContext _dbCtx = dbContext;
        private readonly SignInManager<Users> _signInManger = signInManager;
        private readonly RoleManager<IdentityRole> _roleManger = roleManager;
        private readonly UserManager<Users> _userManger = userManager;

        [HttpPost("register")]
        public async Task<ActionResult> RegisterAccount([FromBody] RegisterDto register)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManger.Users.FirstOrDefaultAsync(u => u.Email == register.Email);
            if (user != null)
            {
                return BadRequest(new { message = $"User is email {register.Email} already exists" });
            }

            // create new user
            var newUser = new Users()
            {
                Email = register.Email,
                Name = register.Name,
                UserName = register.Email
            };

            var results = await _userManger.CreateAsync(newUser, register.Password);
            if (!results.Succeeded)
            {
                return BadRequest(new { message = "Invalid Credential entered, email/username or password!" });
            }

            return Ok(new { message = "User account successfully created." });

        }

        [HttpPost("login")]
        public async Task<ActionResult> LoginAccount([FromBody] LoginDto login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var user = await _userManger.FindByEmailAsync(login.Email);

            if (user == null)
            {
                return Unauthorized("Invalid login credentials!");
            }

            var results = await _signInManger.CheckPasswordSignInAsync(user, login.Password, false);

            if (!results.Succeeded)
            {
                return Unauthorized("Invalid login credentials");
            }

            // store session
            // HttpSession

            var token = _jwt.CreateJwtToken(user);
            var response = new Dictionary<string, string> { { "token", token }, { "message", "login successful" } };
            return Ok(response);
        }
    }
}
