using ContactList.Application.Contracts.Persistance;
using ContactList.Application.Contracts.Security;
using ContactList.Application.DTOs;
using ContactList.Domain.Entities;
using ContactList.Domain.ValueObjects;
using MediatR;

namespace ContactList.Application.Features.Users.Commands.RegisterUser
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, CustomResult<UserDTO>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordEncryptionService _passwordEncryptionService;
        public RegisterUserCommandHandler(IUserRepository userRepository, IPasswordEncryptionService passwordEncryptionService)
        {
            _userRepository = userRepository;
            _passwordEncryptionService = passwordEncryptionService;
        }
        public async Task<CustomResult<UserDTO>> Handle(RegisterUserCommand request, CancellationToken ct)
        {
            var existingUser = await _userRepository.GetByEmailAsync(
            request.Email,
            ct);

            if (existingUser is not null)
            {
                return CustomResult<UserDTO>.FailureResult(
                    "User already exists. Try to login.");
            }

            var passwordHash =
                await _passwordEncryptionService.HashPassword(
                    request.Password);

            var user = new User(
                Guid.NewGuid(),
                new Email(request.Email),
                request.Username,
                new PhoneNumber(request.Mobile),
                passwordHash);

            await _userRepository.AddAsync(
                user,
                ct);

            var userDto = new UserDTO
            {
                Id = user.Id,
                Email = user.Email.Value,
                Username = user.Username,
                Mobile = user.Mobile.Value
            };

            return CustomResult<UserDTO>.SuccessResult(userDto);
        }
    }
}

