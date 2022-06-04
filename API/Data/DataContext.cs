using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base (options)
        {
            
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Post> Posts { get; set; }
        

        protected override void OnModelCreating(ModelBuilder builder) 
        {
            base.OnModelCreating(builder);

            builder.HasPostgresExtension("postgis");
        }
    }
}