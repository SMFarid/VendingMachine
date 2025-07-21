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
        //private readonly UserManager<User> _userManager;
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

            //var user = await _userManager.FindByNameAsync(request.Username);
            //if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
            //{
            //    return Unauthorized(new { error = "Invalid credentials" });
            //}

            //var token = _tokenService.GenerateToken(user);

            //return Ok(new LoginResponse
            //{
            //    Token = token,
            //    User = new UserDto
            //    {
            //        Id = user.Id,
            //        Username = user.UserName ?? "",
            //        Role = user.Role.ToString(),
            //        Balance = user.Role == UserRole.Buyer ? user.Balance : null,
            //        CreatedAt = user.CreatedAt,
            //        UpdatedAt = user.UpdatedAt
            //    },
            //    ExpiresIn = 3600
            //});
        }



        [HttpPost("logout")]
        [Authorize]
        public IActionResult Logout()
        {
            return Ok(new { message = "Logout successful" });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] LoginRequest request)
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

            var newUser = new User
            {
                UserName = request.Username,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Role = (UserRole)request.Role // New registrations default to 'User' role
            };

            var addedUser = await _userService.AddUserAsync(newUser);

            _logger.LogInformation("User {Username} registered successfully with ID: {UserId}", addedUser.UserName); //, addedUser.Id
            return Ok(new { Message = "User registered successfully."  });//, UserId = addedUser.Id
        }
    }
}
