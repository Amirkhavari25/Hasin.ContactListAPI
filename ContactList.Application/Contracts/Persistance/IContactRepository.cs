using ContactList.Domain.Entities;

namespace ContactList.Application.Contracts.Persistance
{
    public interface IContactRepository
    {
        Task<List<Contact>> GetAllByUserIdAsync(Guid userId, CancellationToken ct);
        Task<Contact?> GetByIdAsync(Guid id, Guid userId, CancellationToken ct);
        Task AddAsync(Contact contact, CancellationToken ct);
        Task UpdateAsync(Contact contact, CancellationToken ct);
        Task DeleteAsync(Guid id, Guid userId, CancellationToken ct);
    }
}
