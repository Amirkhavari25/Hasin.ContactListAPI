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
        public User User { get; private set; } = default!;

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
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException(
                    "First name is required.",
                    nameof(firstName));

            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException(
                    "Last name is required.",
                    nameof(lastName));

            ArgumentNullException.ThrowIfNull(phoneNumber);

            if (string.IsNullOrWhiteSpace(tag))
                throw new ArgumentException(
                    "Tag is required.",
                    nameof(tag));

            if (userId == Guid.Empty)
                throw new ArgumentException(
                    "User id cannot be empty.",
                    nameof(userId));

            FirstName = firstName.Trim();
            LastName = lastName.Trim();
            PhoneNumber = phoneNumber;
            Tag = tag.Trim();
            UserId = userId;
        }
    }
}
