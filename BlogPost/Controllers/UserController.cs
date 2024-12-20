using BlogPost.Mappers;
using BlogPost.Models.Users;
using BlogPostBll.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BlogPost.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            bool result = await userService.LoginAsync(UserMapper.Map(loginModel));

            if (result)
            {
                string token = GenerateJwtToken(loginModel.Email);
                return Ok(new { Token = token });
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {
            if (!ModelState.IsValid)
            {
                return View(registerModel);
            }

            var result = await userService.RegisterAsync(UserMapper.Map(registerModel));

            if (result)
            {
                return RedirectToAction(nameof(Login));
            }
            else
            {
                ModelState.AddModelError("", "A user with this email already exists.");
                return View(registerModel);
            }
        }


        public IActionResult Register()
        {         
            return View(new RegisterModel());
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> IsRegistered()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            var userId = await userService.GetUserIdByEmailAsync(userEmail);
            return Ok(new { Message = "User is authenticated.", userId });
        }

        private string GenerateJwtToken(string email)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("W65Oo6yyinA7zpMqU_8W_kmdluxadu7MpWndz8ov6qE=")); // Replace with your secret key
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Email, email)
            };

            var token = new JwtSecurityToken(
                issuer: "BlogPost",
                audience: "BlogPost",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
