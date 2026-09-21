namespace ContactList.Domain.Common
{
    public abstract class BaseEntity
    {
        public Guid Id { get; protected set; }

        public DateTime CreateDate { get; protected set; }

        public DateTime? UpdateDate { get; protected set; }

        public bool IsDelete { get; protected set; }

        protected BaseEntity()
        {
        }

        protected BaseEntity(Guid id)
        {
            Id = id;
            CreateDate = DateTime.UtcNow;
            IsDelete = false;
        }

        public void Delete()
        {
            IsDelete = true;
            UpdateDate = DateTime.UtcNow;
        }
    }
}
