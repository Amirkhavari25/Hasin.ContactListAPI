using ContactList.Domain.Exceptions;
using System.Text.RegularExpressions;

namespace ContactList.Domain.ValueObjects
{
    public sealed class PhoneNumber
    {
        public string Value { get; private set; } = default!;

        private PhoneNumber()
        {
        }

        public PhoneNumber(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new InvalidPhoneNumberException(value);

            value = Normalize(value);

            if (!IsValid(value))
                throw new InvalidPhoneNumberException(value);

            Value = value;
        }

        private static string Normalize(string value)
        {
            value = value.Trim();

            value = value
                .Replace('۰', '0')
                .Replace('۱', '1')
                .Replace('۲', '2')
                .Replace('۳', '3')
                .Replace('۴', '4')
                .Replace('۵', '5')
                .Replace('۶', '6')
                .Replace('۷', '7')
                .Replace('۸', '8')
                .Replace('۹', '9');

            value = value
                .Replace(" ", "")
                .Replace("-", "")
                .Replace("(", "")
                .Replace(")", "");

            if (value.StartsWith("+98"))
                value = "0" + value[3..];

            else if (value.StartsWith("0098"))
                value = "0" + value[4..];

            return value;
        }

        private static bool IsValid(string value)
        {
            return Regex.IsMatch(
                value,
                @"^09\d{9}$",
                RegexOptions.CultureInvariant);
        }

        public override string ToString()
        {
            return Value;
        }

        public override bool Equals(object? obj)
        {
            return obj is PhoneNumber other &&
                   Value == other.Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}
