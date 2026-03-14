using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MovieChallenge.API.DTOs.Auth;
using MovieChallenge.API.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MovieChallenge.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto data)
        {
            if (data == null) return BadRequest("Neplatná data");

            if (data.Password != data.PasswordConfirmation) return BadRequest("Hesla se neshodují");

            var user = new AppUser {
                
                Email = data.Email,
                UserName = data.Email,
                Name = data.Name,
            };

            var result = await _userManager.CreateAsync(user, data.Password);
            
            if (result.Succeeded)
            {
                return Ok("Registrace proběhla úspěšně");
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto data)
        {
            var user = await _userManager.FindByEmailAsync(data.Email);
            if (user != null)
            {
                var correctPassword = await _userManager.CheckPasswordAsync(user, data.Password);

                if (correctPassword)
                    return Ok(GenerateJwtToken(user));
                else
                    return Unauthorized("Nesprávné heslo");
            }

            return Unauthorized("Uživatel nenalezen");
        }

        private string GenerateJwtToken(AppUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Name, user.Name),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(
                    Convert.ToInt32(_configuration["Jwt:ExpirationInDays"])),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}