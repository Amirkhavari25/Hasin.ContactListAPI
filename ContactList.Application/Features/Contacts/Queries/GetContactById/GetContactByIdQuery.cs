using ContactList.Application.DTOs;
using MediatR;

namespace ContactList.Application.Features.Contacts.Queries.GetContactById
{
    public record GetContactByIdQuery(Guid contactId, string creatorId) : IRequest<CustomResult<ContactDTO>>;

}
