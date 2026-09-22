using ContactList.Application.DTOs;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ContactList.Application.Features.Contacts.Command.CreateContact
{
    public class CreateContactCommand : IRequest<CustomResult<ContactDTO>>
    {
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Tag { get; set; }
        [JsonIgnore]
        public Guid UserId { get; set; }
    }
}
