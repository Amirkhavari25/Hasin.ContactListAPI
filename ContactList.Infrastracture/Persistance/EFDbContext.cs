using ContactList.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContactList.Infrastracture.Persistance
{
    public sealed class EFDbContext : DbContext
    {
        public EFDbContext(
            DbContextOptions<EFDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Contact> Contacts => Set<Contact>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(EFDbContext).Assembly);
        }
    }
}
