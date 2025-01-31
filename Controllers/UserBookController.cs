using MyBookList.Models;
using MyBookList.Services;
using System.ComponentModel.DataAnnotations;
using Carter;
using MyBookList.Data;


namespace MyBookList.Controllers;

public class UserBookController : CarterModule
{
public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/userBook/getAll", async (int id, IUserBookService service) =>
        {
            var book = await service.GetUserBooksByUserId(id);
            if (book == null)
                return Results.NotFound();
            return Results.Ok(book);

        });
        app.MapPost("/api/userBook/create", async (UserBookCreateDto userBook, IUserBookService service, HttpContext httpContext) =>
        {
            var token = httpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                return Results.Unauthorized();
            }
            Guid userUuid = Guid.Parse(token);

            DateTime startDate;

            startDate = DateTime.ParseExact(userBook.StartDate, "yyyy-MM-dd", null);
            DateTime endDate = DateTime.ParseExact(userBook.FinishDate, "yyyy-MM-dd", null);

            UserBook newUserBook = new UserBook
            {
                BookId = userBook.BookId,
                Status = userBook.Status,
                DateStarted = startDate,
                DateFinished = endDate

            };

            var createdUserBook = await service.CreateUserBook(newUserBook, userUuid);
            return Results.Ok(createdUserBook);
        });

    }    
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
            public string? StartDate { get; set; }
            public string? FinishDate { get; set; }
        }
    }
}
