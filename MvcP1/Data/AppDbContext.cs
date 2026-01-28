using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MvcP1.Models;
using System.Text.Json;

namespace MvcP1.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<NoteModel> Notes => Set<NoteModel>();
        public DbSet<ContactsModel> Contacts => Set<ContactsModel>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var converter = new ValueConverter<List<string>, string>(
                v => JsonSerializer.Serialize(v ?? new List<string>(), (JsonSerializerOptions?)null),
                v => string.IsNullOrWhiteSpace(v)
                    ? new List<string>()
                    : JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions?)null) ?? new List<string>()
            );

            var comparer = new ValueComparer<List<string>>(
                (c1, c2) => (c1 ?? new()).SequenceEqual(c2 ?? new()),
                c => (c ?? new()).Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => (c ?? new()).ToList()
            );

            //
            modelBuilder.Entity<NoteModel>()
                .Property(x => x.Name)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<NoteModel>()
                .Property(x => x.Text)
                .HasMaxLength(1000)
                .IsRequired();

            modelBuilder.Entity<NoteModel>()
                .Property(e => e.Tags)
                .HasConversion(converter)
                .HasColumnType("nvarchar(max)")
                .Metadata.SetValueComparer(comparer);

            modelBuilder.Entity<NoteModel>()
                .Property(x => x.CreatedAt)
                .HasDefaultValueSql("SYSUTCDATETIME()");

            modelBuilder.Entity<NoteModel>()
                .Property(x => x.UpdatedAt)
                .HasDefaultValueSql("SYSUTCDATETIME()");

            //
            modelBuilder.Entity<ContactsModel>()
                .Property(x => x.Name)
                .HasMaxLength(30) // Степан Работа
                .IsRequired();

            modelBuilder.Entity<ContactsModel>()
               .Property(x => x.Phone)
               .HasMaxLength(13) // +380931234567
               .IsRequired();

            modelBuilder.Entity<ContactsModel>()
               .Property(x => x.PhoneAlt)
               .HasMaxLength(13); // +380931234567

            modelBuilder.Entity<ContactsModel>()
                .Property(x => x.Email)
                .HasMaxLength(50);

            modelBuilder.Entity<ContactsModel>()
               .Property(x => x.DescShort)
               .HasMaxLength(100);

            modelBuilder.Entity<ContactsModel>()
                 .Property(x => x.CreatedAt)
                 .HasDefaultValueSql("SYSUTCDATETIME()");

            modelBuilder.Entity<ContactsModel>()
                .Property(x => x.UpdatedAt)
                .HasDefaultValueSql("SYSUTCDATETIME()");

            //
            modelBuilder.Entity<NoteModel>()
                .ToTable("Notes", tb => tb.HasTrigger("TR_Notes_UpdatedAt"));

            modelBuilder.Entity<ContactsModel>()
                .ToTable("Contacts", tb => tb.HasTrigger("TR_Contacts_UpdatedAt"));

            //
            SeedData(modelBuilder);
        }

        private static void SeedData(ModelBuilder modelBuilder)
        {
            var seedTime = new DateTime(2026, 1, 28, 0, 0, 0, DateTimeKind.Utc);

            modelBuilder.Entity<ContactsModel>().HasData(
                new ContactsModel
                {
                    Id = 1,
                    CreatedAt = seedTime,
                    UpdatedAt = seedTime,
                    Name = "Степан Работа",
                    Phone = "+380931234567",
                    PhoneAlt = null,
                    Email = "stepan@example.com",
                    DescShort = "Основний контакт"
                },
                new ContactsModel
                {
                    Id = 2,
                    CreatedAt = seedTime,
                    UpdatedAt = seedTime,
                    Name = "Олег",
                    Phone = "+380501112233",
                    PhoneAlt = "+380671112233",
                    Email = null,
                    DescShort = "Другий номер"
                }
            );

            modelBuilder.Entity<NoteModel>().HasData(
                new NoteModel
                {
                    Id = 1,
                    CreatedAt = seedTime,
                    UpdatedAt = seedTime,
                    Name = "Перша нотатка",
                    Text = "Тестовий текст нотатки",
                    Tags = new List<string> { "work", "mssql", "mvc" }
                },
                new NoteModel
                {
                    Id = 2,
                    CreatedAt = seedTime,
                    UpdatedAt = seedTime,
                    Name = "Друга нотатка",
                    Text = "Ще одна нотатка",
                    Tags = new List<string> { "home", "idea" }
                }
            );
        }
    }
}
