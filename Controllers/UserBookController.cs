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

        app.MapPut("/api/userBook/update", async (UserBook userBook, IUserBookService service) =>
        {
            var updatedUserBook = await service.UpdateUserBook(userBook);
            return Results.Ok(updatedUserBook);
        });
        app.MapDelete("/api/userBook/delete/{id:int}", async (IUserBookService service, int id) =>
        {
            var result = await service.DeleteUserBook(id);
            if (!result)
                return Results.NotFound();
            return Results.NoContent();
        });

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
