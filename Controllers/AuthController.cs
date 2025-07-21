using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VendingMachine.DTOs;
using VendingMachine.Models;
using VendingMachine.Services;

namespace VendingMachine.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly ILogger<AuthController> _logger;
        private readonly IUserService _userService;

        public AuthController(ITokenService tokenService, ILogger<AuthController> logger, IUserService userService)
        {
            
            _tokenService = tokenService;
            _logger = logger;
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
        {

            _logger.LogInformation("Attempting to log in user: {Username}", request.Username);

            var user = await _userService.GetUserByUsernameAsync(request.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                _logger.LogWarning("Login failed for user {Username}: Invalid credentials.", request.Username);
                return Unauthorized("Invalid username or password.");
            }

            var token = _tokenService.GenerateToken(user);
            _logger.LogInformation("User {Username} logged in successfully and token generated.", user.UserName);
            return Ok(new { Token = token });
        }



        [HttpPost("logout")]
        [Authorize]
        public IActionResult Logout()
        {
            return Ok(new { message = "Logout successful" });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            _logger.LogInformation("Attempting to register user: {Username}", request.Username);

            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            {
                _logger.LogWarning("Registration failed: Username or password missing for {Username}", request.Username);
                return BadRequest("Username and password are required.");
            }

            var existingUser = await _userService.GetUserByUsernameAsync(request.Username);
            if (existingUser != null)
            {
                _logger.LogWarning("Registration failed: Username {Username} already exists.", request.Username);
                return Conflict("Username already exists.");
            }

            //Add check that request.Role is within valid range
            if (request.Role.HasValue && (request.Role < (int)UserRole.Buyer || request.Role > (int)UserRole.Seller))
            {
                _logger.LogWarning("Registration failed: Invalid role {Role} for user {Username}", request.Role, request.Username);
                return BadRequest("Invalid role specified.");
            }

            var newUser = new User
            {
                UserName = request.Username,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Role = (UserRole)request.Role // New registrations default to 'User' role
            };

            var addedUser = await _userService.AddUserAsync(newUser);

            _logger.LogInformation("User {Username} registered successfully with ID: {UserId}", addedUser.UserName, addedUser.Id);
            return Ok(new { Message = "User registered successfully.", UserId = addedUser.Id });
        }

        
        [HttpPut("update")]
        [Authorize]
        public async Task<IActionResult> UpdateUser([FromBody] UserDTO userDto)
        {
            var result = await _userService.UpdateUserAsync(userDto);
            if (result)
            {
                _logger.LogInformation("User with ID {UserId} updated successfully.", userDto.Id);
                return Ok(new { Message = "User updated successfully." });
            }
            else
            {
                _logger.LogWarning("Update failed for user with ID {UserId}: User not found.", userDto.Id);
                return NotFound("User not found.");
            }
        }

        //Add endpoint to delete user
        [HttpDelete("delete")]
        [Authorize]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            _logger.LogInformation("Attempting to delete user with ID: {UserId}", userId);
            var result = await _userService.DeleteUserAsync(userId);
            if (result)
            {
                _logger.LogInformation("User with ID {UserId} deleted successfully.", userId);
                return Ok(new { Message = "User deleted successfully." });
            }
            else
            {
                _logger.LogWarning("Delete failed for user with ID {UserId}: User not found.", userId);
                return NotFound("User not found.");
            }
        }
    }
}
