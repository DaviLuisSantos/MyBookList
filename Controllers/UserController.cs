using MyBookList.Models;
using MyBookList.Services;
using System.ComponentModel.DataAnnotations;
using Carter;
using MyBookList.Data;


namespace MyBookList.Controllers;

public class UserController : CarterModule
{
    private readonly IUserService _userService;

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/user/getById", async (int id) =>
        {
            var user = await _userService.GetUserById(id);
            if (user == null)
                return Results.NotFound();
            return Results.Ok(user);

        });

        app.MapPost("/api/user/getAll", async (IUserService service) =>
        {
            var users = await service.GetAll();
            return users == null ? Results.Ok() : Results.Ok(users);
        });

        app.MapPost("/api/user/register", async (UserCreateDto userDto, IUserService service) =>
        {
            bool createdUser = await service.CreateUser(userDto.Username,userDto.Password,userDto.Email);
            if (!createdUser)
                return Results.BadRequest();
            else
                return Results.Ok();
        });

        app.MapPost("/api/user/update", async (int id, User user) =>
        {
            if (id != user.UserId)
            {
                return Results.BadRequest();
            }
            var updatedUser = await _userService.UpdateUser(user);
            if (updatedUser == null)
                return Results.NotFound();
            return Results.Ok(updatedUser);
        });

        app.MapPost("/api/user/delete", async (int id) =>
        {
            var result = await _userService.DeleteUser(id);
            if (!result)
                return Results.NotFound();
            return Results.NoContent();
        });

        app.MapPost("/api/user/login", async (LoginRequest loginRequest, IUserService service) =>
        {

            string? token = await service.Login(loginRequest.Username, loginRequest.Password);
            if (token == null)
            {
                return Results.NotFound();
            }
            else if (token == "0")
            {
                return Results.Unauthorized();
            }

            return Results.Ok(new { Token = token });

        });
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class UserCreateDto
    {
        [Required]
        [MaxLength(100)]
        public string Username { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
