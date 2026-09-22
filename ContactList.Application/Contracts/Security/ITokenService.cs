using ContactList.Application.DTOs;
using ContactList.Domain.Entities;

namespace ContactList.Application.Contracts.Security
{
    public interface ITokenService
    {
        Task<CustomResult<string>> CreateToken(User user);
    }
}
