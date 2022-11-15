using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;
using Users_Minimal_Api.Models;

namespace Users_Minimal_Api.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            if (!this.Users.Any())
            {
                this.Users.AddRange
                (
                    new User { Username = "joeblogs" },
                    new User { Username = "alice_brown" },
                    new User { Username = "brucewayne39" }
                );
                this.SaveChanges();
            }
        }
        
        public DbSet<User> Users => Set<User>();
    }
}
