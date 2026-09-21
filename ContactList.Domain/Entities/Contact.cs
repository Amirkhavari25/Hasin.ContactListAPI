using ContactList.Domain.ValueObjects;

namespace ContactList.Domain.Entities
{
    public sealed class Contact : BaseEntity
    {
        public string FirstName { get; private set; } = default!;
        public string LastName { get; private set; } = default!;
        public PhoneNumber PhoneNumber { get; private set; } = default!;
        public string Tag { get; private set; } = default!;

        public Guid UserId { get; private set; }

        public Contact(
            Guid id,
            string firstName,
            string lastName,
            PhoneNumber phoneNumber,
            string tag,
            Guid userId)
            : base(id)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException(
                    "User id cannot be empty.",
                    nameof(userId));

            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            Tag = tag;
            UserId = userId;
        }
    }
}
