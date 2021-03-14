using Microsoft.EntityFrameworkCore;
using SignupWithMailConfirmation.Models;

namespace SignupWithMailConfirmation.Data
{
    public class DataContext : DbContext
    
    {
        public DataContext(DbContextOptions options) : base(options) 
        {
            
        }

        public DbSet<LoginInfo> LoginInfos { get; set; }

    }
}