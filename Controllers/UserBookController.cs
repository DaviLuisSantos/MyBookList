using MyBookList.Models;
using MyBookList.Services;
using Carter;
using MyBookList.DTOs.UserBook;


namespace MyBookList.Controllers;

public class UserBookController : CarterModule
{
public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/userBook/getAll", async (int id, IUserBookService service) =>
        {
            var book = await service.GetByUserId(id);
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

            var createdUserBook = await service.Create(userBook);
            return Results.Ok(createdUserBook);
        });

        app.MapPut("/api/userBook/update", async (UserBook userBook, IUserBookService service) =>
        {
            var updatedUserBook = await service.Update(userBook);
            return Results.Ok(updatedUserBook);
        });
        app.MapDelete("/api/userBook/delete/{id:int}", async (IUserBookService service, int id) =>
        {
            var result = await service.Delete(id);
            if (!result)
                return Results.NotFound();
            return Results.NoContent();
        });

    }    

}
