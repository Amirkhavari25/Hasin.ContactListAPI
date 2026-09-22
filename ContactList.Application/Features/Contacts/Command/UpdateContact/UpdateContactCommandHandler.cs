using ContactList.Application.Contracts.Persistance;
using ContactList.Application.DTOs;
using ContactList.Domain.ValueObjects;
using MediatR;

namespace ContactList.Application.Features.Contacts.Command.UpdateContact
{
    public class UpdateContactCommandHandler : IRequestHandler<UpdateContactCommand, CustomResult<string>>
    {
        private readonly IContactRepository _contactRepository;
        public UpdateContactCommandHandler(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }
        public async Task<CustomResult<string>> Handle(UpdateContactCommand request, CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(request.creatorId, out var userId))
            {
                return CustomResult<string>.FailureResult("Invalid user ID.");
            }
            var contact = await _contactRepository.GetByIdAsync(
             request.Id,
             userId,
             cancellationToken);

            if (contact is null)    
            {
                return CustomResult<string>.FailureResult(
                    "Contact not found.");
            }

            contact.Update(
                request.FirstName,
                request.LastName,
                new PhoneNumber(request.PhoneNumber),
                request.Tag);

            await _contactRepository.UpdateAsync(
                contact,
                cancellationToken);

            return CustomResult<string>.SuccessResult(
                "Contact updated successfully.");
        }


    }

}
