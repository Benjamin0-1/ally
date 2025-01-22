using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using MediatR;
using Ally.Application.Core.Authentication.CommandHandler; // Example: Import command handler namespaces if required.

namespace Ally.Application.Configuration
{
    public static class DependencyInjection
    {
        // Add MediatR to the DI container and configure it to scan the current assembly.
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Add MediatR and specify the assembly to scan for handlers
            services.AddMediatR(cfg =>
            {
                // Register services from the current executing assembly, which should include handlers.
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });

            // If you have other dependencies (such as services or repositories) to register, you can do so here.
            // Example:
            // services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}

