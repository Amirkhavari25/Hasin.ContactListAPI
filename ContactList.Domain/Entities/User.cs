using ContactList.Domain.ValueObjects;

namespace ContactList.Domain.Entities
{
    public sealed class User : BaseEntity
    {
        public Email Email { get; private set; } = default!;
        public string Username { get; private set; } = default!;
        public PhoneNumber Mobile { get; private set; } = default!;
        public string PasswordHash { get; private set; } = default!;
        private readonly List<Contact> _contacts = [];
        public IReadOnlyCollection<Contact> Contacts =>
            _contacts.AsReadOnly();

        public User(
            Guid id,
            Email email,
            string username,
            PhoneNumber mobile,
            string passwordHash)
            : base(id)
        {
            Email = email;
            Username = username;
            Mobile = mobile;
            PasswordHash = passwordHash;
        }

        public void AddContact(Contact contact)
        {
            ArgumentNullException.ThrowIfNull(contact);

            if (contact.UserId != Id)
                throw new InvalidOperationException(
                    "Contact does not belong to this user.");

            if (_contacts.Any(x => x.Id == contact.Id))
                return;

            _contacts.Add(contact);
        }

        public void RemoveContact(Guid contactId)
        {
            var contact = _contacts.FirstOrDefault(x => x.Id == contactId);

            if (contact is null)
                return;

            _contacts.Remove(contact);
        }
    }
}
