using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MiniValidation;
using Users_Minimal_Api.Data;
using Users_Minimal_Api.Dtos;
using Users_Minimal_Api.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<DataContext>(options => options.UseInMemoryDatabase("UsersDB"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "UserAPI", Version = "v1" });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "UserAPI v1");
    });
}

var usersRoutes = app.MapGroup("/users");

usersRoutes.MapGet("/",  GetUsers);

usersRoutes.MapPost("/", CreateUser);


usersRoutes.MapPut("/{id}", UpdateUser);

usersRoutes.MapDelete("/{id}", DeleteUser);

app.Run();

static async Task<IResult> GetUsers(DataContext context)
{
    return Results.Ok(await context.Users.OrderBy(u => u.UserId).ToListAsync());
}

static async Task<IResult> CreateUser(UserDto newUser, DataContext context)
{
    if (!MiniValidator.TryValidate(newUser, out var errors))
    {
        return Results.ValidationProblem(errors);
    }

    if (await context.Users.AnyAsync(u => u.Username == newUser.Username))
    {
        return Results.BadRequest("A user with the same username already exists.");
    }

    var user = new User { Username = newUser.Username };

    await context.Users.AddAsync(user);
    await context.SaveChangesAsync();

    return Results.Created("/users", new UserDto(user));
}

static async Task<IResult> UpdateUser(int id, UserDto updatedUser, DataContext context)
{
    if (!MiniValidator.TryValidate(updatedUser, out var errors))
    {
        return Results.ValidationProblem(errors);
    }

    if (await context.Users.AnyAsync(u => u.Username == updatedUser.Username))
    {
        return Results.BadRequest("A user with the same username already exists.");
    }
    
    var user = await context.Users.FirstOrDefaultAsync(u => u.UserId == id);
    if (user is null)
    {
        return Results.NotFound("User not found.");
    }

    user.Username = updatedUser.Username;

    context.Users.Update(user);
    await context.SaveChangesAsync();

    return Results.Ok(new UserDto(user));
}

static async Task<IResult> DeleteUser(int id, DataContext context)
{
    var user = await context.Users.FirstOrDefaultAsync(u => u.UserId == id);
    if (user is null)
    {
        return Results.NotFound("User not found.");
    }
    context.Users.Remove(user);
    await context.SaveChangesAsync();

    return Results.NoContent();
}

public partial class Program { }