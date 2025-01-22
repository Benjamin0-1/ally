using Microsoft.EntityFrameworkCore;
using Ally.Domain.Entities; // Import your domain entities here

namespace Ally.Infrastructure.Data
{
    // This class should inherit from DbContext
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Register your tables here by defining DbSet properties
        public DbSet<UserEntity> Users { get; set; } // Example table

        // Add more DbSet properties for other tables
        // public DbSet<OtherEntity> OtherEntities { get; set; }

        // Optional: Configure entity models using Fluent API (if needed)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Example of custom entity configuration (if needed)
            // modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
        }
    }
}

