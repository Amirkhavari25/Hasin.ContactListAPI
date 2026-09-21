using ContactList.Domain.ValueObjects;

namespace ContactList.Domain.Entities
{
    public class User : BaseEntity
    {
        public Email Email { get; private set; } = default!;

        public string Username { get; private set; } = default!;

        public PhoneNumber Mobile { get; private set; } = default!;

        public string PasswordHash { get; private set; } = default!;

        public ICollection<Contact> Contacts { get; private set; }
            = new List<Contact>();

        protected User()
        {
        }

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
    }
}
