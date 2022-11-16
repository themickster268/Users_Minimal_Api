using Microsoft.EntityFrameworkCore;
using Users_Minimal_Api.Models;

namespace Users_Minimal_Api.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }
        
        public DbSet<User> Users => Set<User>();
    }
}
