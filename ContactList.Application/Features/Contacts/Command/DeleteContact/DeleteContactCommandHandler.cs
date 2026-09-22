using ContactList.Application.Contracts.Persistance;
using ContactList.Application.DTOs;
using MediatR;


namespace ContactList.Application.Features.Contacts.Command.DeleteContact
{
    public class DeleteContactCommandHandler : IRequestHandler<DeleteContactCommand, CustomResult<string>>
    {
        private readonly IContactRepository _contactRepository;
        public DeleteContactCommandHandler(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }
        public async Task<CustomResult<string>> Handle(DeleteContactCommand request, CancellationToken ct)
        {

            if (!Guid.TryParse(request.creatorId, out var userId))
            {
                return CustomResult<string>.FailureResult("Invalid user ID.");
            }
            var contact = await _contactRepository.GetByIdAsync(request.contactId, userId, ct);
            if (contact == null)
            {
                return CustomResult<string>.FailureResult("Contact not found");
            }
            await _contactRepository.DeleteAsync(request.contactId, userId, ct);
            return CustomResult<string>.SuccessResult("Contact deleted successful");

        }
    }
}
