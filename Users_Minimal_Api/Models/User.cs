using System.ComponentModel.DataAnnotations;

namespace Users_Minimal_Api.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        public string Username { get; set; } = null!;
    }
}
