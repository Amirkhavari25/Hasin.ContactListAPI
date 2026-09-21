using ContactList.Domain.Exceptions;
using System.Text.RegularExpressions;

namespace ContactList.Domain.ValueObjects
{
    public sealed class Email
    {
        public string Value { get; private set; } = default!;

        private Email()
        {
        }

        public Email(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new InvalidEmailException(value);

            value = value.Trim();

            if (value.Length > 100)
                throw new InvalidEmailException(value);

            if (!Regex.IsMatch(
                    value,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.CultureInvariant))
            {
                throw new InvalidEmailException(value);
            }

            Value = value.ToLowerInvariant();
        }

        public override string ToString()
        {
            return Value;
        }

        public override bool Equals(object? obj)
        {
            return obj is Email other &&
                   Value == other.Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}
