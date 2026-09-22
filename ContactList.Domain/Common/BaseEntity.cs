namespace ContactList.Domain.Common
{
    public abstract class BaseEntity
    {
        public Guid Id { get; protected set; }
        public DateTime CreateDate { get; protected set; }
        public DateTime? UpdateDate { get; protected set; }
        public bool IsDeleted { get; protected set; }
        protected BaseEntity()
        {
        }

        protected BaseEntity(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException(
                    "Entity id cannot be empty.",
                    nameof(id));
            }

            Id = id;
            CreateDate = DateTime.UtcNow;
            IsDeleted = false;
        }

        public void Delete()
        {
            if (IsDeleted)
                return;

            IsDeleted = true;
            UpdateDate = DateTime.UtcNow;
        }

        public void Restore()
        {
            if (!IsDeleted)
                return;

            IsDeleted = false;
            UpdateDate = DateTime.UtcNow;
        }
    }
}
