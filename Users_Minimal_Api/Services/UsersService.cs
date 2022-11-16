using Users_Minimal_Api.Data;
using Users_Minimal_Api.Dtos;
using Users_Minimal_Api.Models;

namespace Users_Minimal_Api.Services
{
    public class UsersService : IUsersService
    {
        private readonly DataContext _context;

        public UsersService(DataContext context)
        {
            this._context = context;
        }

        public IResult CreateUser(UserDto newUser)
        {
            if (_context.Users.Any(u => u.Username == newUser.Username))
            {
                return Results.BadRequest("User with that username already exists.");
            }

            var user = new User { Username= newUser.Username };
            _context.Users.Add(user);
            _context.SaveChanges();

            return Results.Created("/users", new UserDto(user));
        }

        public IResult DeleteUser(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId.Equals(id));
            if (user is null)
            {
                return Results.NotFound();
            }

            _context.Users.Remove(user);
            _context.SaveChanges();

            return Results.NoContent();
        }

        public IResult GetUsers()
        {
            var users = _context.Users.OrderBy(u => u.UserId).ToList();
            return Results.Ok(users);
        }

        public IResult UpdateUser(int id, UserDto updatedUser)
        {
            if (_context.Users.Any(u => u.Username == updatedUser.Username))
            {
                return Results.BadRequest("User with that username already exists.");
            }

            var user = _context.Users.FirstOrDefault(u => u.UserId.Equals(id));
            if (user is null)
            {
                return Results.NotFound("User not found.");
            }

            _context.Users.Update(user);
            _context.SaveChanges();

            return Results.Ok(new UserDto(user));
        }
    }
}
