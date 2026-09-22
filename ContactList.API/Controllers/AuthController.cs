using ContactList.Application.DTOs;
using ContactList.Application.Features.Users.Commands.LoginUser;
using ContactList.Application.Features.Users.Commands.RegisterUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ContactList.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<CustomResult<UserDTO>> Register(RegisterUserCommand command)
        {
            if (!ModelState.IsValid)
            {
                return CustomResult<UserDTO>.FailureResult("Invalid data!");
            }

            return await _mediator.Send(command);
        }

        [HttpPost("login")]
        public async Task<CustomResult<string>> Login(LoginCommand command)
        {
            if (!ModelState.IsValid)
                return CustomResult<string>.FailureResult("Invalid data!");

            return await _mediator.Send(command);
        }
    }
}
