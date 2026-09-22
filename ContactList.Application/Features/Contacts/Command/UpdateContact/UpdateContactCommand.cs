using ContactList.Application.DTOs;
using MediatR;
using System.Text.Json.Serialization;

namespace ContactList.Application.Features.Contacts.Command.UpdateContact
{
    public class UpdateContactCommand : IRequest<CustomResult<string>>
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Tag { get; set; }
        [JsonIgnore]
        public string? creatorId { get; set; }
    }
}
