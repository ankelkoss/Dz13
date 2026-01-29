using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MvcP1.Models;

namespace MvcP1.Data.EntityConfigurations
{
    public class ContactsConfiguration : IEntityTypeConfiguration<Contacts>
    {
        public void Configure(EntityTypeBuilder<Contacts> builder)
        {
            builder.Property(x => x.Name)
                .HasMaxLength(30) // Степан Работа
                .IsRequired();

            builder.Property(x => x.Phone)
               .HasMaxLength(13) // +380931234567
               .IsRequired();

            builder.Property(x => x.PhoneAlt)
               .HasMaxLength(13); // +380931234567

            builder.Property(x => x.Email)
                .HasMaxLength(50);

            builder.Property(x => x.DescShort)
               .HasMaxLength(100);

            builder.Property(x => x.CreatedAt)
                 .HasDefaultValueSql("SYSUTCDATETIME()");

            builder.Property(x => x.UpdatedAt)
                .HasDefaultValueSql("SYSUTCDATETIME()");

            builder.ToTable("Contacts", tb => tb
                .HasTrigger("TR_Contacts_UpdatedAt"));
        }
    }
}
