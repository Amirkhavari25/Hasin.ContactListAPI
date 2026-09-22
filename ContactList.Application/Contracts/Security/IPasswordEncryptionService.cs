namespace ContactList.Application.Contracts.Security
{
    public interface IPasswordEncryptionService
    {
        Task<string> HashPassword(string password);
        Task<bool> VerifyPassword(string password, string hashedPassword);
    }
}
