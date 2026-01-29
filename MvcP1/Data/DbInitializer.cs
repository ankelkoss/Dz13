using Microsoft.EntityFrameworkCore;
using MvcP1.Models;

namespace MvcP1.Data
{
    public class DbInitializer
    {
        private readonly ModelBuilder _modelBuilder;

        public DbInitializer(ModelBuilder modelBuilder)
        {
            _modelBuilder = modelBuilder;
        }

        public void Seed()
        {
            var seedTime = new DateTime(2026, 1, 28, 0, 0, 0, DateTimeKind.Utc);

            _modelBuilder.Entity<Contacts>().HasData(
                new Contacts
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
                new Contacts
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

            _modelBuilder.Entity<Note>().HasData(
                new Note
                {
                    Id = 1,
                    CreatedAt = seedTime,
                    UpdatedAt = seedTime,
                    Name = "Перша нотатка",
                    Text = "Тестовий текст нотатки",
                    Tags = new List<string> { "work", "mssql", "mvc" }
                },
                new Note
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
