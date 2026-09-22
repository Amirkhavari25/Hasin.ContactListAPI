using ContactList.Application.DTOs;
using ContactList.Application.Features.Contacts.Command.CreateContact;
using ContactList.Application.Features.Contacts.Command.DeleteContact;
using ContactList.Application.Features.Contacts.Command.UpdateContact;
using ContactList.Application.Features.Contacts.Queries.GetContactById;
using ContactList.Application.Features.Contacts.Queries.GettAllContacts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ContactList.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ContactController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ContactController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        public async Task<CustomResult<ContactDTO>> CreateContact(CreateContactCommand command)
        {
            if (command == null || !ModelState.IsValid)
            {
                return CustomResult<ContactDTO>.FailureResult("Invalid Data");
            }
            var creatorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (creatorId == null)
            {
                return CustomResult<ContactDTO>.FailureResult("Invalid user credential"); ;
            }
            command.UserId = Guid.Parse(creatorId);
            return await _mediator.Send(command);
        }

        [HttpGet]
        public async Task<CustomResult<List<ContactDTO>>> GettAllContacts()
        {
            var creatorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (creatorId == null)
            {
                return CustomResult<List<ContactDTO>>.FailureResult("Invalid User request,Please try to login");
            }
            return await _mediator.Send(new GetAllContactsQuery(creatorId));
        }

        [HttpGet("{id}")]
        public async Task<CustomResult<ContactDTO>> GetById(Guid id)
        {
            var creatorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (creatorId == null)
            {
                return CustomResult<ContactDTO>.FailureResult("Invalid User request,Please try to login");
            }
            return await _mediator.Send(new GetContactByIdQuery(id, creatorId));

        }

        [HttpPut("{id}")]
        public async Task<CustomResult<string>> UpdateContact(Guid id, [FromBody] UpdateContactCommand command)
        {
            if (id != command.Id)
            {
                return CustomResult<string>.FailureResult("Id mismatch");
            }
            var creatorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (creatorId == null)
            {
                return CustomResult<string>.FailureResult("Invalid User request,Please try to login");
            }
            command.creatorId = creatorId;

            return await _mediator.Send(command);
        }

        [HttpDelete("{id}")]
        public async Task<CustomResult<string>> DeleteContact(Guid id)
        {
            var creatorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (creatorId == null)
            {
                return CustomResult<string>.FailureResult("Invalid User request,Please try to login");
            }
            return await _mediator.Send(new DeleteContactCommand(id, creatorId));
        }
    }
}
