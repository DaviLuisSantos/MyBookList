using MyBookList.Models;
using MyBookList.Services;
using Carter;
using MyBookList.DTOs.Book;


namespace MyBookList.Controllers;

public class BookController : CarterModule
{
    private readonly IBookService _bookService;

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/userBook/getAll", async (int id, IBookService service) =>
        {
            var book = await _bookService.GetAll();
            if (book == null)
                return Results.NotFound();
            return Results.Ok(book);

        });

        app.MapPost("/api/userBook/getById", async (int id, IBookService service) =>
        {
            var book = await _bookService.GetById(id);
            if (book == null)
                return Results.NotFound();
            return Results.Ok(book);
        });

        app.MapPost("/api/userBook/create", async (BookCreateDto book, IBookService service) =>
        {
            var createdBook = await _bookService.Create(book);
            return Results.Ok(createdBook);
        });

        app.MapPut("/api/userBook/update", async (Book book, IBookService service) =>
        {
            var updatedBook = await _bookService.Update(book);
            return Results.Ok(updatedBook);
        });

        app.MapDelete("/api/userBook/delete/{id:int}", async (IBookService service, int id) =>
        {
            var result = await _bookService.Delete(id);
            if (!result)
                return Results.NotFound();
            return Results.NoContent();
        });
    }
  
}
