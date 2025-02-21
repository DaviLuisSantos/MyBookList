using Carter;
using Microsoft.EntityFrameworkCore;
using MyBookList.Authentication;
using MyBookList.Data;
using MyBookList.Services;

var builder = WebApplication.CreateBuilder(args);

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddOpenApi();
builder.AddAuth();
// Adicione o contexto do banco de dados
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite("Data Source=database.db"));

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IUserBookService, UserBookService>();
builder.Services.AddScoped<IEmailSender, EmailSender>();
//builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddCarter();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

var app = builder.Build();

// Criar o banco de dados no inicio da aplicação
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("AllowAllOrigins");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// Map dos Controllers
app.MapCarter();

app.Run();