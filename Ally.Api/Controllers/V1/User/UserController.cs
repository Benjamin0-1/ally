using Ally.Application.Core.Authentication.Command;
using Ally.Application.Core.Authentication.Query;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using Ally.Infrastructure.ActionFilters;
using Ally.Infrastructure.Attributes;

namespace Ally.Api.Controllers.V1
{
    [Route("/api/v1/user")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [Authorize]
        [ServiceFilter(typeof(RoleCheckFilter))]
        [RequireRole(2)]  // This checks if the user has role 20
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            return Ok("You are here.");
        }


        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] SignUpCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);

                if (!result)
                {
                    throw new ApplicationException("Error creating user");
                }
                
                return CreatedAtAction(nameof(SignUp), new { email = command.Email },
                    result);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error during user sign-up: {e.Message}");
                return StatusCode(500, $"Internal server error: {e}");
            }
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> GetUserProfile()
        {
            try
            {
                var query = new GetUserProfileQuery();
                var result = await _mediator.Send(query);

                if (result == null)
                {
                    return NotFound("User profile not found.");
                }

                return Ok(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, "An internal error occurred while processing the request.");
            }
        }
    }
}

