using Microsoft.AspNetCore.Mvc;
using MyBookList.Models;
using MyBookList.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;

namespace MyBookList.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public UserController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsers();
            return Ok(users);
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateUser(UserCreateDto userDto)
        {
            var user = new User
            {
                Username = userDto.Username,
                Email = userDto.Email,
                PasswordHash = userDto.Password
            };

            var createdUser = await _userService.CreateUser(user);
            return CreatedAtAction(nameof(GetUser), new { id = createdUser.UserId }, createdUser);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateUser(int id, User user)
        {
            if (id != user.UserId)
            {
                return BadRequest();
            }

            var updatedUser = await _userService.UpdateUser(user);
            if (updatedUser == null)
                return NotFound();

            return Ok(updatedUser);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUser(id);
            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            // 1. Validate the user and password (can use UserService)
            User? user = await _userService.GetUserByUsername(loginRequest.Username);
            if (user == null)
            {
                return NotFound();
            }

            /*
            // Verify the password (use an appropriate hash comparison function)
            if (!VerifyPassword(loginRequest.Password, user.PasswordHash))
            {
                return Unauthorized();
            }
            */

            // 2. Generate the JWT token
            string? token = _tokenService.GenerateToken(user.UserUuid.ToString());

            // 3. Return the token to the client
            return Ok(new { Token = token });
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            // Implement your password verification logic here
            // For example, using BCrypt:
            return BCrypt.Net.BCrypt.Verify(password, storedHash);
        }

        public class LoginRequest
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        public class UserCreateDto
        {
            [Required]
            [MaxLength(100)]
            public string Username { get; set; }
            [Required]
            [EmailAddress]
            public string Email { get; set; }
            [Required]
            public string Password { get; set; }
        }
    }
}