using BhagiRadhaSwayamKrushi.Data;
using BhagiRadhaSwayamKrushi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;

// Alias DTO namespace to avoid conflict with Identity
using DTO = BhagiRadhaSwayamKrushi.DTO;

namespace BhagiRadhaSwayamKrushi.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly AppDbContext _context;

        public AuthController(IConfiguration config, AppDbContext context)
        {
            _config = config;
            _context = context;
        }

        // ---------------- Registration ----------------
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] DTO.RegisterRequest request)
        {
            // Check if mobile already exists
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Mobile == request.Mobile);

            if (existingUser != null)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "Mobile number already registered"
                });
            }

            var user = new User
            {
                Name = request.Name,
                Mobile = request.Mobile,
                Password = request.Password,   // ⚠ plain text for now
                Address = request.Address,
                Role = "User",
                ActiveStatus = "Active"        // Default active
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Success = true,
                Message = "Registration successful"
            });
        }


        // ---------------- Login ----------------
        [HttpPost("login")]

        public async Task<IActionResult> Login([FromBody] DTO.LoginRequest request)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u =>
                    u.Mobile == request.Mobile &&
                    u.Password == request.Password);

            if (user == null)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "Invalid mobile number or password",
                    Token = string.Empty,
                    Expiration = DateTime.MinValue,
                    UserName = string.Empty,
                    Mobile=string.Empty,
                    Role = string.Empty,
                    Address = string.Empty,
                    ActiveStatus = string.Empty
                });
            }

            if (user.ActiveStatus != "Active")
            {
                return Ok(new
                {
                    Success = false,
                    Message = $"User is {user.ActiveStatus}. Please contact admin.",
                    Token = string.Empty,
                    Expiration = DateTime.MinValue,
                    UserName = user.Name,
                    Mobile=user.Mobile,
                    Role = user.Role,
                    Address = user.Address,
                    ActiveStatus = user.ActiveStatus
                });
            }

            // JWT creation
            var jwt = _config.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, user.Name),
        new Claim(ClaimTypes.Role, user.Role),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

            var token = new JwtSecurityToken(
                issuer: jwt["Issuer"],
                audience: jwt["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwt["ExpireMinutes"])),
                signingCredentials: creds
            );

            return Ok(new
            {
                Success = true,
                Message = "Login successful",
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo,
                UserName = user.Name,
                Mobile= user.Mobile,
                Role = user.Role,   
                Address = user.Address,
                ActiveStatus = user.ActiveStatus

            });
        }

    }
}
