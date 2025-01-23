using Ally.Infrastructure.Data; // Import the DbContext
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Ally.Application.Abstraction.Authentication;
using Ally.Application.Abstraction.JWT;
using Ally.Infrastructure.Repositories.Jwt;
using Ally.Infrastructure.User;

namespace Ally.Api
{
    public static class DependencyInjection
    {
        // Add DbContext configuration for PostgreSQL
        // public static IServiceCollection AddDbContext(
        //     this IServiceCollection services,
        //     IConfiguration configuration)
        // {
        //     var cs = configuration.GetConnectionString("DefaultConnection"); // Assuming connection string is in appsettings.json
        //     services.AddDbContext<ApplicationDbContext>(options => // <-- change to AllyDbContext
        //         options.UseNpgsql(cs, npgsqlOptions => npgsqlOptions
        //             .EnableRetryOnFailure(
        //                 maxRetryCount: 5,
        //                 maxRetryDelay: TimeSpan.FromSeconds(3), // Corrected TimeSpan usage
        //                 errorCodesToAdd: null // No specific error codes to add
        //             )
        //         )
        //     );
        //     return services;
        // }

        public static IServiceCollection AddDbContext(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // For SQLite, we will use a file-based database (local storage).
            var connectionString = "Data Source=Ally.db";  // SQLite connection string

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(connectionString)
            );

            return services;
        }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            // Register IAuthenticationRepository with its implementation UserCommandRepository
            services.AddScoped<IAuthenticationRepository, UserCommandRepository>();
            services.AddScoped<ICreateUserRepository, CreateUserRepositoryRepository>();
            services.AddScoped<IUserProfileQueryRepository, UserProfileQueryRepository>();
            services.AddScoped<ITokenRepository, TokenRepository>();
            services.AddScoped<IJwtRepository, JwtRepository>();
            return services;
        }

        public static IServiceCollection AddRedis(this IServiceCollection services)
        {
            // Example Redis setup (uncomment and configure it as needed)
            // services.AddStackExchangeRedisCache(options =>
            // {
            //     options.Configuration = "localhost:6379"; 
            //     options.InstanceName = "AllyCache"; 
            // });
            return services;
        }
    }
}