using MyBookList.Models;
using MyBookList.Services;
using Carter;
using MyBookList.DTOs.User;



namespace MyBookList.Controllers;

public class UserController : CarterModule
{
    private readonly IUserService _userService;

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/user/getById", async (int id) =>
        {
            var user = await _userService.GetById(id);
            if (user == null)
                return Results.NotFound();
            return Results.Ok(user);

        });

        app.MapPost("/api/user/getAll", async (IUserService service) =>
        {
            List<User>? users = await service.GetAll();
            return users == null ? Results.Ok() : Results.Ok(users);
        });

        app.MapPost("/api/user/register", async (UserCreateDto userDto, IUserService service) =>
        {
            var createdUser = await service.Create(userDto);
            if (createdUser==null)
                return Results.BadRequest();
            else
                return Results.Ok();
        });

        app.MapPost("/api/user/update", async (int id, User user) =>
        {
            if (id != user.Id)
            {
                return Results.BadRequest();
            }
            var updatedUser = await _userService.Update(user);
            if (updatedUser == null)
                return Results.NotFound();
            return Results.Ok(updatedUser);
        });

        app.MapPost("/api/user/delete", async (int id) =>
        {
            var result = await _userService.Delete(id);
            if (!result)
                return Results.NotFound();
            return Results.NoContent();
        });

        app.MapPost("/api/user/login", async (LoginDto loginRequest, IUserService service) =>
        {

            LoginReturn? loginReturn = await service.Login(loginRequest);
            if (loginReturn.token == null)
            {
                return Results.NotFound();
            }
            else if (loginReturn.token == "0")
            {
                return Results.Unauthorized();
            }

            return Results.Ok(new { loginReturn });

        });
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

}
