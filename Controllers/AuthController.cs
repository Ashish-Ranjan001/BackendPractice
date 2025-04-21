using BackendPractice.AuthModle;
using BackendPractice.Repository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BackendPractice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILoginRepository loginRepository;

        public AuthController(IConfiguration configuration, ILoginRepository loginRepository)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.loginRepository = loginRepository ?? throw new ArgumentNullException(nameof(loginRepository));
        }

        //[HttpPost("login")]
        //public async Task<IActionResult> Login([FromBody] LoginModel request)
        //{
        //    Console.WriteLine($"Login attempt: Email={request.Email}");

        //    if (!ModelState.IsValid)
        //    {
        //        Console.WriteLine("Invalid request body");
        //        return BadRequest("Invalid Request Body");
        //    }

        //    var isAuthenticated = await loginRepository.Login(request.Email, request.Password);
        //    if (isAuthenticated != null)
        //    {
        //        Console.WriteLine("User authenticated successfully!");

        //        var token = GenerateJwtToken(isAuthenticated);
        //        return Ok(new { Token = token });
        //    }

        //    Console.WriteLine("Authentication failed: Invalid user credentials.");
        //    return Unauthorized("Invalid user credentials.");
        //}
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel request)
        {
            Console.WriteLine($"Login attempt: Email={request.Email}");

            if (!ModelState.IsValid)
            {
                Console.WriteLine("Invalid request body");
                return BadRequest(new { error = "Invalid Request Body" });
            }

            var user = await loginRepository.Login(request.Email, request.Password);

            if (user == null)
            {
                Console.WriteLine("Authentication failed: Invalid user credentials.");
                return Unauthorized(new { error = "Invalid email or password." });
            }

            if (!user.IsActive)
            {
                Console.WriteLine("Blocked: Account is inactive.");
                return Unauthorized(new { error = "Your account is deactivated. Please contact support." });
            }

            var token = GenerateJwtToken(user);
            return Ok(new { message = "Login successful!", token });
        }

        private string GenerateJwtToken(RegisterDBModel user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration["Jwt:Key"] ?? throw new Exception("JWT Key missing in config")
            ));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var userRole = string.IsNullOrEmpty(user.Roles) ? "User" : user.Roles; // Assign default role if null

            var claims = new List<Claim>
    {
        new(ClaimTypes.NameIdentifier, user.FirstName ?? ""),
        new(ClaimTypes.Email, user.Email ?? ""),
        new(ClaimTypes.Role, userRole) // Ensure role is included
    };

            Console.WriteLine($"Generated Claims: {string.Join(", ", claims.Select(c => c.Type + "=" + c.Value))}");

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
       
     
    }
}
