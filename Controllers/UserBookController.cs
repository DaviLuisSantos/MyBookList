using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBookList.Services;
using MyBookList.Models;
using System.ComponentModel.DataAnnotations;

namespace MyBookList.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserBookController : ControllerBase
    {
        private readonly IUserBookService _userBookService;
        private readonly ITokenService _tokenService;

        public UserBookController(IUserBookService userBookService, ITokenService tokenService)
        {
            _userBookService = userBookService;
            _tokenService = tokenService;
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
        public async Task<IActionResult> CreateUserBook(UserBook userBook)
        {
            var createdUserBook = await _userBookService.CreateUserBook(userBook);
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
            public int Rating { get; set; }
            [Required]
            public string? Status { get; set; }
            public DateTime? DateStarted { get; set; }
            public DateTime? DateFinished { get; set; }
        }
    }
}
