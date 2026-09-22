using ContactList.Application.DTOs;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace ContactList.Application.Features.Users.Commands.RegisterUser
{
    public class RegisterUserCommand : IRequest<CustomResult<UserDTO>>
    {
        [Required, MaxLength(100), EmailAddress]
        public string Email { get; set; }
        [Required, MaxLength(100)]
        public string Username { get; set; }
        [Required, MaxLength(100), Phone]
        public string Mobile { get; set; }
        [Required, MinLength(6)]
        public string Password { get; set; }
    }
}
