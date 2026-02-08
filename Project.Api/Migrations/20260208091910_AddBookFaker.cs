using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Project.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddBookFaker : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "Books");

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "CreatedAt", "Discount", "IsDeleted", "IsReleased", "Name", "Price", "ReleaseDate", "Stock" },
                values: new object[,]
                {
                    { new Guid("019c3c8c-3d68-7e8d-b317-46a1f30cdbe3"), new DateTime(2026, 2, 8, 9, 19, 9, 671, DateTimeKind.Utc).AddTicks(2701), (byte)0, false, false, "Voluptatem atque odit", 10.91m, new DateTime(2025, 2, 25, 9, 19, 9, 676, DateTimeKind.Utc).AddTicks(3918), 102 },
                    { new Guid("019c3c8c-3d6c-71d3-b2b0-be4f433edb79"), new DateTime(2026, 2, 8, 9, 19, 9, 676, DateTimeKind.Utc).AddTicks(9859), (byte)0, false, false, "Ut tempora repudiandae", 3.42m, new DateTime(2025, 8, 1, 9, 19, 9, 676, DateTimeKind.Utc).AddTicks(9892), 38 },
                    { new Guid("019c3c8c-3d6c-7516-bb0e-e08fdaad6797"), new DateTime(2026, 2, 8, 9, 19, 9, 676, DateTimeKind.Utc).AddTicks(9896), (byte)0, false, false, "Maiores voluptas sint omnis fuga ut at", 6.45m, new DateTime(2025, 6, 3, 9, 19, 9, 676, DateTimeKind.Utc).AddTicks(9943), 17 },
                    { new Guid("019c3c8c-3d6c-76d8-a099-ff6ef931f180"), new DateTime(2026, 2, 8, 9, 19, 9, 676, DateTimeKind.Utc).AddTicks(9691), (byte)0, false, false, "In ab est animi", 28.60m, new DateTime(2025, 8, 4, 9, 19, 9, 676, DateTimeKind.Utc).AddTicks(9745), 44 },
                    { new Guid("019c3c8c-3d6c-791a-9d70-138551143e6e"), new DateTime(2026, 2, 8, 9, 19, 9, 676, DateTimeKind.Utc).AddTicks(9390), (byte)0, false, false, "Iusto deserunt ea quam", 16.77m, new DateTime(2025, 3, 26, 9, 19, 9, 676, DateTimeKind.Utc).AddTicks(9676), 98 },
                    { new Guid("019c3c8c-3d6c-7ca0-9f7e-ddff83b24da0"), new DateTime(2026, 2, 8, 9, 19, 9, 676, DateTimeKind.Utc).AddTicks(9752), (byte)0, false, false, "Voluptas aut eligendi et sint molestiae exercitationem", 10.30m, new DateTime(2025, 9, 18, 9, 19, 9, 676, DateTimeKind.Utc).AddTicks(9856), 123 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("019c3c8c-3d68-7e8d-b317-46a1f30cdbe3"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("019c3c8c-3d6c-71d3-b2b0-be4f433edb79"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("019c3c8c-3d6c-7516-bb0e-e08fdaad6797"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("019c3c8c-3d6c-76d8-a099-ff6ef931f180"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("019c3c8c-3d6c-791a-9d70-138551143e6e"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("019c3c8c-3d6c-7ca0-9f7e-ddff83b24da0"));

            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "Books",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
