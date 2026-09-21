using ContactList.Domain.ValueObjects;

namespace ContactList.Domain.Entities
{
    public class Contact : BaseEntity
    {
        public string FirstName { get; private set; } = default!;
        public string LastName { get; private set; } = default!;
        public PhoneNumber PhoneNumber { get; private set; } = default!;
        public string Tag { get; private set; } = default!;

        public Guid UserId { get; private set; }
        public User? User { get; private set; }

        protected Contact()
        {
        }
        public Contact(
            Guid id,
            string firstName,
            string lastName,
            PhoneNumber phoneNumber,
            string tag,
            Guid userId)
            : base(id)
        {
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            Tag = tag;
            UserId = userId;
        }
    }
}
