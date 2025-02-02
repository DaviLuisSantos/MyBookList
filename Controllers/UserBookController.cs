using MyBookList.Models;
using MyBookList.Services;
using Carter;
using MyBookList.DTOs.UserBook;
using Microsoft.AspNetCore.Http;


namespace MyBookList.Controllers;

public class UserBookController : CarterModule
{
public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/userBook/getAll", async (IUserBookService service, HttpContext httpContext) =>
        {
            var token = httpContext.Request.Headers["key"].ToString();

            if (string.IsNullOrEmpty(token))
            {
                return Results.Unauthorized();
            }
            Guid userUuid = Guid.Parse(token);

            var uBook = await service.GetByUserUuid(userUuid);
            if (uBook == null)
                return Results.NotFound();
            return Results.Ok(uBook);

        });
        app.MapPost("/api/userBook/create", async (UserBookCreateDto userBook, IUserBookService service, HttpContext httpContext) =>
        {
            var token = httpContext.Request.Headers["key"].ToString();

            if (string.IsNullOrEmpty(token))
            {
                return Results.Unauthorized();
            }
            Guid userUuid = Guid.Parse(token);

            userBook.UserUuid = userUuid; 

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
