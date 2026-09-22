using ContactList.Application.Contracts.Persistance;
using ContactList.Application.DTOs;
using MediatR;

namespace ContactList.Application.Features.Contacts.Queries.GettAllContacts
{
    public class GetAllContactsQueryHandler : IRequestHandler<GetAllContactsQuery, CustomResult<List<ContactDTO>>>
    {
        private readonly IContactRepository _contactRepository;
        public GetAllContactsQueryHandler(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public async Task<CustomResult<List<ContactDTO>>> Handle(GetAllContactsQuery request, CancellationToken ct)
        {
            if (!Guid.TryParse(request.creatorId, out var userId))
            {
                return CustomResult<List<ContactDTO>>.FailureResult("Invalid user ID.");
            }
            var contacts = await _contactRepository.GetAllByUserIdAsync(userId, ct);
            if (contacts is null || contacts.Count == 0)
            {
                return CustomResult<List<ContactDTO>>.FailureResult("No contact found");
            }

            List<ContactDTO> result = [..contacts.Select(x=> new ContactDTO
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    PhoneNumber = x.PhoneNumber.Value,
                    Tag = x.Tag,
                })];

            return CustomResult<List<ContactDTO>>.SuccessResult(result);
        }
    }
}
