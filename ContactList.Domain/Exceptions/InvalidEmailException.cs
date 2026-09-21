namespace ContactList.Domain.Exceptions
{
    public sealed class InvalidEmailException : DomainException
    {
        public InvalidEmailException(string email)
            : base($"The email address '{email}' is invalid.")
        {
        }
    }
}
