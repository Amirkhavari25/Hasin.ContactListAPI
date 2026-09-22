using ContactList.Application.DTOs;
using MediatR;

namespace ContactList.Application.Features.Contacts.Queries.GettAllContacts
{
    public record GetAllContactsQuery(string creatorId) : IRequest<CustomResult<List<ContactDTO>>>;

}
