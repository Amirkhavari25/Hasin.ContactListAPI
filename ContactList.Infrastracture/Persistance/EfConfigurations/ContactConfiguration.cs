using ContactList.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContactList.Infrastracture.Persistance.EfConfigurations
{
    public sealed class ContactConfiguration
    : IEntityTypeConfiguration<Contact>
    {
        public void Configure(EntityTypeBuilder<Contact> builder)
        {
            builder.ToTable("Contacts");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedNever();

            builder.Property(x => x.CreateDate)
                .IsRequired();

            builder.Property(x => x.UpdateDate);

            builder.Property(x => x.IsDeleted)
                .IsRequired();

            builder.Property(x => x.FirstName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.LastName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.PhoneNumber)
                .HasConversion(
                    phone => phone.Value,
                    value => new PhoneNumber(value))
                .HasMaxLength(11)
                .IsRequired();

            builder.Property(x => x.Tag)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.UserId)
                .IsRequired();
        }
    }
}
