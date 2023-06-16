using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using BusinessLogic.Context;
using BusinessLogic.Models;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly TasksDbContext _context;

        public AuthController(IConfiguration configuration, TasksDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpPost("token")]
        public IActionResult GenerateToken([FromBody] LoginModel user)
        {
            if (IsValidUser(user))
            {
                var token = GenerateJwtToken(user.Username);
                return Ok(new { Token = token });
            }

            return Unauthorized();
        }
        

        private bool IsValidUser(LoginModel user)
        {
            var existingUser = _context.Users.FirstOrDefault(u => u.Username == user.Username && u.Password == user.Password);
            return existingUser != null;
        }

        private string GenerateJwtToken(string username)
        {
            var existingUser = _context.Users.FirstOrDefault(u => u.Username == username);

            if (existingUser == null)
            {
                // Utilizador não encontrado
                throw new ArgumentException("Invalid user");
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim("userId", existingUser.Userid.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["JwtSettings:TokenExpirationTimeInMinutes"]));

            var token = new JwtSecurityToken(
                _configuration["JwtSettings:Issuer"],
                _configuration["JwtSettings:Audience"],
                claims,
                expires: expires,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }
}
