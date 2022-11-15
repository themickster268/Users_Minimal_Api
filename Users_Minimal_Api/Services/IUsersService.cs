using Users_Minimal_Api.Dtos;

namespace Users_Minimal_Api.Services
{
    public interface IUsersService
    {
        IResult GetUsers();
        IResult CreateUser(UserDto newUser);
        IResult UpdateUser(int id, UserDto updatedUser);
        IResult DeleteUser(int id);
    }
}
