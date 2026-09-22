namespace ContactList.Infrastracture.Persistance.QueryModels
{
    internal class ContactQueryModel
    {
        public Guid Id { get; init; }
        public string FirstName { get; init; } = default!;
        public string LastName { get; init; } = default!;
        public string PhoneNumber { get; init; } = default!;
        public string Tag { get; init; } = default!;
        public Guid UserId { get; init; }
        public DateTime CreateDate { get; init; }
        public DateTime? UpdateDate { get; init; }
        public bool IsDeleted { get; init; }
    }
}
