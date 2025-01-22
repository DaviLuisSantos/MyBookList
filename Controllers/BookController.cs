using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyBookList.Models;
using MyBookList.Services;
using System.ComponentModel.DataAnnotations;

namespace MyBookList.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly ITokenService _tokenService;
        public BookController(IBookService bookService, ITokenService tokenService)
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
        public async Task<IActionResult> CreateBook(BookCreateDto book)
        {
            Book newBook = new Book
            {
                Title = book.Title,
                Author = book.Author,
                Description = book.Description,
                Pages = book.Pages,
                Genre = book.Genre
            };
            var createdBook = await _bookService.CreateBook(newBook);
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

        public class BookCreateDto
        {
            [Required]
            public string Title { get; set; }
            [Required]
            public string Author { get; set; }

            public string? Description { get; set; }
            public int? Pages { get; set; }
            public string? Genre { get; set; }
        }

    }
}
