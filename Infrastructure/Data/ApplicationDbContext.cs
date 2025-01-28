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
        public DbSet<UserEntity> Users { get; set; } // User table
        public DbSet<RoleEntity> Roles { get; set; } // Role table
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<ProductVariantEntity> ProductVariants { get; set; }
        public DbSet<CartEntity> Carts { get; set; }
        public DbSet<CartProductVariantEntity> CartProductVariants { get; set; } // <-- actual many-to-many table

        // Add more DbSet properties for other tables if needed
        // public DbSet<OtherEntity> OtherEntities { get; set; }

        // Configure entity models using Fluent API
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Apply base configurations first

            // Enforce uniqueness for the Email field in the UserEntity table
            modelBuilder.Entity<UserEntity>()
                .HasIndex(u => u.Email)   // Specify the Email field
                .IsUnique();              // Ensure uniqueness for Email in the database

            modelBuilder.Entity<RoleEntity>()
                .HasIndex(r => r.Name)
                .IsUnique();
        }
    }
}

