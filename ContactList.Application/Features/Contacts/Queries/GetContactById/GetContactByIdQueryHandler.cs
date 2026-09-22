using ContactList.Application.Contracts.Persistance;
using ContactList.Application.DTOs;
using MediatR;

namespace ContactList.Application.Features.Contacts.Queries.GetContactById
{
    public class GetContactByIdQueryHandler : IRequestHandler<GetContactByIdQuery, CustomResult<ContactDTO>>
    {
        private readonly IContactRepository _contactRepository;
        public GetContactByIdQueryHandler(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }
        public async Task<CustomResult<ContactDTO>> Handle(GetContactByIdQuery request, CancellationToken ct)
        {

            if (!Guid.TryParse(request.creatorId, out var userId))
            {
                return CustomResult<ContactDTO>.FailureResult("Invalid user ID.");
            }
            var contact = await _contactRepository.GetByIdAsync(request.contactId, userId, ct);
            if (contact is null)
            {
                return CustomResult<ContactDTO>.FailureResult("Contact not found");
            }
            ContactDTO result = new()
            {
                Id = contact.Id,
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                PhoneNumber = contact.PhoneNumber.Value,
                Tag = contact.Tag,
            };
            return CustomResult<ContactDTO>.SuccessResult(result);

        }
    }
}
