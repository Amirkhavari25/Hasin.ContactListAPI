using ContactList.Application.Contracts.Persistance;
using ContactList.Application.DTOs;
using ContactList.Domain.Entities;
using ContactList.Domain.ValueObjects;
using MediatR;

namespace ContactList.Application.Features.Contacts.Command.CreateContact
{
    public class CreateContactCommandHandler : IRequestHandler<CreateContactCommand, CustomResult<ContactDTO>>
    {
        private readonly IContactRepository _contactRepository;
        public CreateContactCommandHandler(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }
        public async Task<CustomResult<ContactDTO>> Handle(CreateContactCommand request, CancellationToken cancellationToken)
        {
            var contactId = Guid.NewGuid();
            var contact = new Contact(
                            contactId,
                            request.FirstName,
                            request.LastName,
                            new PhoneNumber(request.PhoneNumber),
                            request.Tag,
                            request.UserId);

            await _contactRepository.AddAsync(
                contact,
                cancellationToken);

            var contactDto = new ContactDTO
            {
                Id = contactId,
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                PhoneNumber = contact.PhoneNumber.Value,
                Tag = contact.Tag,
            };

            return CustomResult<ContactDTO>.SuccessResult(
                contactDto);

        }
    }
}
