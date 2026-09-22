namespace ContactList.Infrastracture.Persistance.QueryModels
{
    public class UserQueryModel
    {
        public Guid Id { get; init; }
        public string Email { get; init; } = default!;
        public string Username { get; init; } = default!;
        public string Mobile { get; init; } = default!;
        public string PasswordHash { get; init; } = default!;
        public DateTime CreateDate { get; init; }
        public DateTime? UpdateDate { get; init; }
        public bool IsDeleted { get; init; }
    }
}
