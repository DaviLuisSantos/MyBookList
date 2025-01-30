using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBookList.Services;
using MyBookList.Models;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MyBookList.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserBookController : ControllerBase
    {
        private readonly IUserBookService _userBookService;

        public UserBookController(IUserBookService userBookService)
        {
            _userBookService = userBookService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllUserBooks()
        {
            var userBooks = await _userBookService.GetAllUserBooks();
            return Ok(userBooks);
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize]
        public async Task<IActionResult> GetUserBookById(int id)
        {
            var userBook = await _userBookService.GetUserBookById(id);
            if (userBook == null)
                return NotFound();
            return Ok(userBook);
        }
        [HttpGet]
        [Route("user/{userId}")]
        [Authorize]
        public async Task<IActionResult> GetUserBooksByUserId(int userId)
        {
            var userBooks = await _userBookService.GetUserBooksByUserId(userId);
            return Ok(userBooks);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateUserBook(UserBookCreateDto userBook)
        {
            // Extract the UserUuid from the JWT token
            var userUuidClaim = User.FindFirst("UserUuid");
            if (userUuidClaim == null)
            {
                return Unauthorized();
            }

            Guid userUuid = Guid.Parse(userUuidClaim.Value);

            // Convert startDate from string to DateTime
            DateTime? startDate = null;
            if (!string.IsNullOrEmpty(userBook.startDate))
            {
                startDate = DateTime.ParseExact(userBook.startDate, "yyyy-MM-dd", null);
            }

            UserBook newuserBook = new UserBook
            {
                BookId = userBook.BookId,
                Status = userBook.Status,
                DateStarted = startDate,
                DateFinished = userBook.DateFinished
            };

            var createdUserBook = await _userBookService.CreateUserBook(newuserBook, userUuid);
            return CreatedAtAction(nameof(GetUserBookById), new { id = createdUserBook.UserBookId }, createdUserBook);
        }
        [HttpPut]
        [Route("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateUserBook(int id, UserBook userBook)
        {
            if (id != userBook.UserBookId)
                return BadRequest();
            var updatedUserBook = await _userBookService.UpdateUserBook(userBook);
            return Ok(updatedUserBook);
        }
        [HttpDelete]
        [Route("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteUserBook(int id)
        {
            var result = await _userBookService.DeleteUserBook(id);
            if (!result)
                return NotFound();
            return NoContent();
        }

        public class UserBookCreateDto
        {
            [Required]
            public int UserId { get; set; }
            [Required]
            public int BookId { get; set; }
            [Required]
            public string? Status { get; set; }
            public string? startDate { get; set; }
            public DateTime? DateFinished { get; set; }
        }
    }
}
