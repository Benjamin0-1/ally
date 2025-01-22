using Ally.Application.Core.Authentication.Command;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using MediatR;

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

                // Return a 201 Created status code since a new resource (user) is created
                return CreatedAtAction(nameof(SignUp), new { email = command.Email }, result); // Or use the appropriate response here
            }
            catch (Exception e)
            {
                // Log the exception for more detailed insight
                Console.WriteLine($"Error during user sign-up: {e.Message}");
                return StatusCode(500, $"Internal server error: {e}"); // Return a 500 error if something goes wrong
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

    }
}

