using System.ComponentModel.DataAnnotations;
using Users_Minimal_Api.Models;

namespace Users_Minimal_Api.Dtos
{
    public class UserDto
    {
        [Required, MinLength(5)]
        public string Username { get; set; } = null!;

        public UserDto()
        {

        }

        public UserDto(User user)
        {
            Username= user.Username;
        }
    }
}
