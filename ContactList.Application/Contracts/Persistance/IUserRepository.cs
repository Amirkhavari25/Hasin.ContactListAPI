using ContactList.Domain.Entities;

namespace ContactList.Application.Contracts.Persistance
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email, CancellationToken ct = default);
        Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<User?> GetByUsernameAsync(string username, CancellationToken ct = default);
        Task AddAsync(User user, CancellationToken ct = default);
    }
}
