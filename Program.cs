using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MyBookList.Authentication;
using MyBookList.Data;
using MyBookList.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();

builder.AddAuth();

// Adicione o contexto do banco de dados
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=wallet.db"));

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IUserBookService, UserBookService>();
//builder.Services.AddScoped<ITokenService, TokenService>();


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
    app.UseSwagger();
    }

// Habilita o CORS **ANTES** de qualquer outro middleware que possa precisar dele
app.UseCors("AllowAllOrigins");

app.UseHttpsRedirection();

// Middleware de Autenticação
app.UseAuthentication();

// Middleware de Autorização
app.UseAuthorization();

// Map dos Controllers
app.MapControllers();


app.Run();