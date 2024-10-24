using EasyAccounts.DbContexts;
using EasyAccounts.Models;
using EasyAccounts.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using System.Data;
using EasyAccounts.Dtos;

namespace EasyAccounts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly EasyAccDbContext _context;
        private readonly AuthenticationService _authenticationService;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public AuthController(EasyAccDbContext context, AuthenticationService authenticationService,IConfiguration configuration, ILogger<AuthController> logger)
        {
            _context = context;
            _authenticationService = authenticationService;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequest)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == loginRequest.Username);

                if (user == null || !VerifyPassword(loginRequest.Password, user.PasswordHash, user.Salt))
                {
                    return Unauthorized("Invalid username or password");
                }

                var token = _authenticationService.GenerateJwtToken(user);

                return Ok(new { Token = token });

            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequest)
        {
            try
            {
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == registerRequest.Username);

                if (existingUser !=null)
                {
                    return BadRequest("Username Already taken!");
                }

                var newUser = new User
                {
                    Username = registerRequest.Username,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerRequest.Password),
                    Salt = BCrypt.Net.BCrypt.GenerateSalt(),
                };
                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();


                foreach (var roleId in registerRequest.Roles)
                {
                    var roleEntity = await _context.Roles.Where(r => registerRequest.Roles.Contains(r.Id)).ToListAsync();
                    if (roleEntity != null)
                    {
                        // Create a new UserRole link for each role
                        _context.UserRoles.Add(new UserRole { UserId = newUser.UserId, RoleId = roleId });
                    }
                }
                await _context.SaveChangesAsync();
                return Ok("User has registered Succesfully");
            }
            catch (DbUpdateException ex)
            {
                // Log the error or inspect the inner exception
                _logger.LogError(ex, "An error occurred while saving the entity changes: {Message}", ex.InnerException?.Message);

                // Return an error response
                return StatusCode(500, $"Internal server error: {ex.InnerException?.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request");

                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("roles")]
        public async Task<IActionResult> GetRoles()
        {
            try
            {
                var roles = await _context.Roles.Select(r => new { r.Id, r.RoleType }).ToListAsync();
                return Ok(roles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching roles");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var users = await _context.Users.Select(u => new { u.UserId, u.Username, u.UserRoles }).ToListAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching users");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        private bool VerifyPassword(string enteredPassword, string storedPasswordHash, string salt)
        {
            //return BCrypt.Net.BCrypt.Verify(enteredPassword + salt, storedPasswordHash);
            enteredPassword = enteredPassword?.Trim();

            var combinedPassword = enteredPassword + salt;

            enteredPassword = enteredPassword?.Trim();

            //_logger.LogInformation($"Entered Password: {enteredPassword}");
            //_logger.LogInformation($"Stored Password Hash: {storedPasswordHash}");

            // Compare enteredPassword with the stored password hash
            var isPasswordCorrect = BCrypt.Net.BCrypt.Verify(enteredPassword, storedPasswordHash);

            //_logger.LogInformation($"Password Verification Result: {isPasswordCorrect}");

            return isPasswordCorrect;
        }
    }
}
