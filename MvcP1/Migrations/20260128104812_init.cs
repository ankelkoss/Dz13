using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MvcP1.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    PhoneAlt = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DescShort = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Notes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Text = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Tags = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notes", x => x.Id);
                });
// ----------------------------------------------------------------------------------------------------------------------------------
            migrationBuilder.Sql(@"
                CREATE OR ALTER TRIGGER TR_Notes_UpdatedAt
                ON dbo.Notes
                AFTER UPDATE
                AS
                BEGIN
                    SET NOCOUNT ON;

                    UPDATE n
                    SET UpdatedAt = SYSUTCDATETIME()
                    FROM dbo.Notes n
                    INNER JOIN inserted i ON n.Id = i.Id;
                END"
            );

            migrationBuilder.Sql(@"
                CREATE OR ALTER TRIGGER TR_Contacts_UpdatedAt
                ON dbo.Contacts
                AFTER UPDATE
                AS
                BEGIN
                    SET NOCOUNT ON;

                    UPDATE c
                    SET UpdatedAt = SYSUTCDATETIME()
                    FROM dbo.Contacts c
                    INNER JOIN inserted i ON c.Id = i.Id;
                END"
            );
// ----------------------------------------------------------------------------------------------------------------------------------
            migrationBuilder.InsertData(
                table: "Contacts",
                columns: new[] { "Id", "CreatedAt", "DescShort", "Email", "Name", "Phone", "PhoneAlt", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 28, 0, 0, 0, 0, DateTimeKind.Utc), "Основний контакт", "stepan@example.com", "Степан Работа", "+380931234567", null, new DateTime(2026, 1, 28, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, new DateTime(2026, 1, 28, 0, 0, 0, 0, DateTimeKind.Utc), "Другий номер", null, "Олег", "+380501112233", "+380671112233", new DateTime(2026, 1, 28, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "Notes",
                columns: new[] { "Id", "CreatedAt", "Name", "Tags", "Text", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 28, 0, 0, 0, 0, DateTimeKind.Utc), "Перша нотатка", "[\"work\",\"mssql\",\"mvc\"]", "Тестовий текст нотатки", new DateTime(2026, 1, 28, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, new DateTime(2026, 1, 28, 0, 0, 0, 0, DateTimeKind.Utc), "Друга нотатка", "[\"home\",\"idea\"]", "Ще одна нотатка", new DateTime(2026, 1, 28, 0, 0, 0, 0, DateTimeKind.Utc) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP TRIGGER IF EXISTS TR_Notes_UpdatedAt;");
            migrationBuilder.Sql(@"DROP TRIGGER IF EXISTS TR_Contacts_UpdateAt;");

            migrationBuilder.DropTable(name: "Contacts");

            migrationBuilder.DropTable(name: "Notes");
        }
    }
}
