using MyBookList.Models;
using MyBookList.Services;
using Carter;
using MyBookList.DTOs.Book;
using Microsoft.AspNetCore.Authorization;


namespace MyBookList.Controllers;

public class BookController : CarterModule
{
    public BookController()
    {
        this.RequireAuthorization();
    }
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/book/getAll", async (int id, IBookService service) =>
        {
            var book = await service.GetAll();
            if (book == null)
                return Results.NotFound();
            return Results.Ok(book);

        });

        app.MapPost("/api/book/getById", async (int id, IBookService service) =>
        {
            var book = await service.GetById(id);
            if (book == null)
                return Results.NotFound();
            return Results.Ok(book);
        });

        app.MapPost("/api/book/create", [Authorize] async (BookCreateDto book, IBookService service) =>
        {
            var createdBook = await service.Create(book);
            return Results.Ok(createdBook);
        });

        app.MapPut("/api/book/update", async (Book book, IBookService service) =>
        {
            var updatedBook = await service.Update(book);
            return Results.Ok(updatedBook);
        });

        app.MapDelete("/api/book/delete/{id:int}", async (IBookService service, int id) =>
        {
            var result = await service.Delete(id);
            if (!result)
                return Results.NotFound();
            return Results.NoContent();
        });
    }
  
}
