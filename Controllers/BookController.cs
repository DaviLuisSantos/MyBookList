using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyBookList.Models;
using MyBookList.Services;

namespace MyBookList.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly BookService _bookService;
        private readonly ITokenService _tokenService;
        public BookController(BookService bookService, ITokenService tokenService)
        {
            _bookService = bookService;
            _tokenService = tokenService;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllBooks()
        {
            var books = await _bookService.GetAllBooks();
            return Ok(books);
        }
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetBookById(int id)
        {
            var book = await _bookService.GetBookById(id);
            if (book == null)
                return NotFound();
            return Ok(book);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateBook(Book book)
        {
            var createdBook = await _bookService.CreateBook(book);
            return CreatedAtAction(nameof(GetBookById), new { id = createdBook.BookId }, createdBook);
        }
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateBook(int id, Book book)
        {
            if (id != book.BookId)
                return BadRequest();
            var updatedBook = await _bookService.UpdateBook(book);
            return Ok(updatedBook);
        }
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var result = await _bookService.DeleteBook(id);
            if (!result)
                return NotFound();
            return NoContent();
        }

    }
}
