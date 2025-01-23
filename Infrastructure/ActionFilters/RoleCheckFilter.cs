using Ally.Application.Abstraction.JWT;
using Ally.Infrastructure.Attributes;
using Ally.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Ally.Infrastructure.ActionFilters
{
    
    public class RoleCheckFilter : IAsyncActionFilter
    {
        private readonly IJwtRepository _jwtRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _context;

        public RoleCheckFilter(IJwtRepository jwtRepository,
            IHttpContextAccessor httpContextAccessor,
            ApplicationDbContext context)
        {
            _jwtRepository = jwtRepository;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }


        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            try
            {
                var actionDescriptor = context.ActionDescriptor;

                var requiredRoleAttribute = actionDescriptor
                    .EndpointMetadata
                    .OfType<RequireRoleAttribute>()
                    .FirstOrDefault();

                if (requiredRoleAttribute != null)
                {
                
                    int userId = await _jwtRepository.GetUserIdFromJwt();

                    Console.WriteLine($"User id from RoleCheckFilter: {userId}");
                   
                    bool hasRole = await UserHasRole(userId, requiredRoleAttribute.RoleId);

                    if (!hasRole)
                    {
                        context.Result = new Microsoft.AspNetCore.Mvc.ForbidResult(); // 403 Forbidden
                        return;
                    }
                }

     
                await next();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                context.Result =
                    new Microsoft.AspNetCore.Mvc.StatusCodeResult(500); 
            }
        }

       /**
        * The method below will later come from an interface instead.
        */
        private async Task<bool> UserHasRole(int userId, int roleId)
        {
           
            var userRoleId = await _context.Users
                                            .Where(u => u.Id == userId)
                                            .Select(u => u.RoleId)
                                            .FirstOrDefaultAsync();
            Console.WriteLine($"User Role : {userRoleId}");
            return userRoleId == roleId;
        }
    }
}
