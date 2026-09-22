using ContactList.Application.DTOs;
using MediatR;

namespace ContactList.Application.Features.Users.Commands.LoginUser
{
    public record LoginCommand(string email, string password) : IRequest<CustomResult<string>>;

}
