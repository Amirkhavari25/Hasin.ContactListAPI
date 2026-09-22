using ContactList.Application.DTOs;
using MediatR;

namespace ContactList.Application.Features.Contacts.Command.DeleteContact
{
    public record DeleteContactCommand(Guid contactId, string creatorId) : IRequest<CustomResult<string>>;

}
