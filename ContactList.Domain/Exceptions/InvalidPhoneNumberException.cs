namespace ContactList.Domain.Exceptions
{
    public sealed class InvalidPhoneNumberException : DomainException
    {
        public InvalidPhoneNumberException(string phoneNumber)
            : base($"The phone number '{phoneNumber}' is invalid.")
        {
        }
    }
}
