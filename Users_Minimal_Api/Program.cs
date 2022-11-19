using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MiniValidation;
using Users_Minimal_Api.Data;
using Users_Minimal_Api.Dtos;
using Users_Minimal_Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>(options => options.UseInMemoryDatabase("UsersDB"));
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "UsersAPI", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "UsersAPI v1");
    });
}

var usersRoutes = app.MapGroup("/users");

usersRoutes.MapGet("/",  (IUsersService userService) => userService.GetUsers());

usersRoutes.MapPost("/", (UserDto newUser, IUsersService userService) =>
{
    return !MiniValidator.TryValidate(newUser, out var errors)
        ? Results.ValidationProblem(errors)
        : userService.CreateUser(newUser);
});

usersRoutes.MapPut("/{id}", (int id, UserDto newUser, IUsersService userService) => 
{
    return !MiniValidator.TryValidate(newUser, out var errors)
        ? Results.ValidationProblem(errors)
        : userService.UpdateUser(id, newUser);
});

usersRoutes.MapDelete("/{id}", (int id, IUsersService userService) => userService.DeleteUser(id));

app.Run();

public partial class Program { }