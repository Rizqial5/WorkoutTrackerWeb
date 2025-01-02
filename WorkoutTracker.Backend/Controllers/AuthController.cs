using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WorkoutTracker.Backend.Models;

namespace WorkoutTracker.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Description("Test")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;



        public AuthController(IConfiguration configuration, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _configuration = configuration;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost("Login")]
        [EndpointDescription("User Login")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Login([FromBody] LoginRequest login)
        {
            // Cari user berdasarkan username (atau email, jika menggunakan email untuk login)
            var user = await _userManager.FindByNameAsync(login.Username!);
            if (user == null)
            {
                return Unauthorized("Invalid credentials");
            }

            // Cek apakah password yang dimasukkan valid
            var result = await _signInManager.PasswordSignInAsync(user, login.Password!, false, false);
            if (result.Succeeded)
            {
                // Generate token jika login berhasil
                var token = GenerateJwtToken(user);
                return Ok(new { Token = token });
            }

            return Unauthorized("Invalid credentials");
        }

        [HttpPost("Register")]
        [EndpointDescription("Register new account for user")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest register)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser
                {
                    UserName = register.Username,
                    Email = register.Email
                };

                var result = await _userManager.CreateAsync(user, register.Password!);
                if (result.Succeeded)
                {
                    // Jika registrasi berhasil, login dan buat token JWT
                    return Ok("Registered Successfully");
                }

                return BadRequest(result.Errors);
            }

            return BadRequest("Invalid data.");
        }


        private string GenerateJwtToken(IdentityUser user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
