using ContactList.Application.Contracts.Persistance;
using ContactList.Application.Contracts.Security;
using ContactList.Application.DTOs;
using MediatR;

namespace ContactList.Application.Features.Users.Commands.LoginUser
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, CustomResult<string>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordEncryptionService _passwordEncryptionService;
        private readonly ITokenService _tokenService;
        public LoginCommandHandler(IUserRepository userRepository, IPasswordEncryptionService passwordEncryptionService, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _passwordEncryptionService = passwordEncryptionService;
            _tokenService = tokenService;
        }
        public async Task<CustomResult<string>> Handle(LoginCommand request, CancellationToken ct)
        {
            var user = await _userRepository.GetByEmailAsync(request.email, ct);
            if (user == null)
            {
                return CustomResult<string>.FailureResult("Wrong credential!", 400);
            }
            //check password
            if (!await _passwordEncryptionService.VerifyPassword(request.password, user.PasswordHash))
            {
                return CustomResult<string>.FailureResult("Wrong credential!", 400);
            }
            var tokenResult = await _tokenService.CreateToken(user);
            if (!tokenResult.IsSuccess) return CustomResult<string>.FailureResult(tokenResult.ErrorMessage ?? "Something went wrong with server");

            return CustomResult<string>.SuccessResult(tokenResult.Data!);


        }
    }
}
