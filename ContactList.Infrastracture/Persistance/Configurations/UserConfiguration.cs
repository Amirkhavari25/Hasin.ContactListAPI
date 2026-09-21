using ContactList.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContactList.Infrastracture.Persistance.Configurations
{
    public sealed class UserConfiguration
    : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedNever();

            ConfigureProperties(builder);

            ConfigureIndexes(builder);

            ConfigureRelationships(builder);
        }

        private static void ConfigureProperties(
            EntityTypeBuilder<User> builder)
        {
            builder.Property(x => x.Email)
                .HasConversion(
                    email => email.Value,
                    value => new Email(value))
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.Username)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.Mobile)
                .HasConversion(
                    phone => phone.Value,
                    value => new PhoneNumber(value))
                .HasMaxLength(11)
                .IsRequired();

            builder.Property(x => x.PasswordHash)
                .IsRequired();

            builder.Property(x => x.IsDelete)
                .IsRequired();

            builder.Property(x => x.CreateDate)
                .IsRequired();

            builder.Property(x => x.UpdateDate);
        }

        private static void ConfigureIndexes(
            EntityTypeBuilder<User> builder)
        {
            builder.HasIndex(x => x.Email)
                .IsUnique();

            builder.HasIndex(x => x.Username)
                .IsUnique();

            builder.HasIndex(x => x.Mobile)
                .IsUnique();
        }

        private static void ConfigureRelationships(
            EntityTypeBuilder<User> builder)
        {
            builder.HasMany(x => x.Contacts)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId)       
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
