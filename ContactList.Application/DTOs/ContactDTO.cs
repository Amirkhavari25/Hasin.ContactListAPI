namespace ContactList.Application.DTOs
{
    public class ContactDTO
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Tag { get; set; }
    }
}
