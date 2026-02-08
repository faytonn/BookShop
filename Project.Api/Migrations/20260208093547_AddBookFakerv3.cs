using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Project.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddBookFakerv3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("019c3c99-d3d4-70b5-bb69-6052de365d79"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("019c3c99-d3d9-7033-b12b-3b84472bff28"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("019c3c99-d3d9-7075-ab7a-b7b26555140b"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("019c3c99-d3d9-707f-9278-78802f9f06a9"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("019c3c99-d3d9-70cd-9214-4e7556bc4866"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("019c3c99-d3d9-71f6-8a7f-95c63ebd651a"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("019c3c99-d3d9-7211-b56f-fb78b5b4841c"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("019c3c99-d3d9-750c-abd3-878339d77a46"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("019c3c99-d3d9-7650-8969-fb03b2bcc806"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("019c3c99-d3d9-76a9-8be6-e89a3ced0298"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("019c3c99-d3d9-76ee-87df-b8728a707dbf"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("019c3c99-d3d9-775c-a3d7-55a7ec5f9ab7"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("019c3c99-d3d9-7822-9ad6-c6c2d0221d21"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("019c3c99-d3d9-78bf-a0b4-2874ef0b6f73"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("019c3c99-d3d9-7983-b0c8-0401f6d0edf6"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("019c3c99-d3d9-7b9e-94ac-97da1adfef62"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("019c3c99-d3d9-7c45-b26a-feb2831ccfa5"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("019c3c99-d3d9-7c58-9b95-ab1589b94afc"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("019c3c99-d3d9-7d50-a9cd-805a21f27f96"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("019c3c99-d3d9-7df2-91a1-2202cdc18dcb"));

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "CreatedAt", "Discount", "IsDeleted", "IsReleased", "Name", "Price", "ReleaseDate", "Stock" },
                values: new object[,]
                {
                    { new Guid("019c3c9b-7675-7855-a1bd-c4da7e20509d"), new DateTime(2026, 2, 8, 9, 35, 47, 316, DateTimeKind.Utc).AddTicks(7349), (byte)0, false, false, "Quia dolorem et quae maiores provident voluptatem similique", 16.50m, new DateTime(2025, 9, 15, 9, 35, 47, 322, DateTimeKind.Utc).AddTicks(2733), 89 },
                    { new Guid("019c3c9b-767b-72f0-971c-5a3bb9763d8b"), new DateTime(2026, 2, 8, 9, 35, 47, 323, DateTimeKind.Utc).AddTicks(997), (byte)0, false, false, "Molestiae voluptatem fuga soluta quidem", 35.85m, new DateTime(2025, 5, 30, 9, 35, 47, 323, DateTimeKind.Utc).AddTicks(1030), 81 },
                    { new Guid("019c3c9b-767b-74f3-bbcf-7fb0affdfbe8"), new DateTime(2026, 2, 8, 9, 35, 47, 323, DateTimeKind.Utc).AddTicks(1124), (byte)0, false, false, "Voluptas et natus magnam et pariatur dicta repellat", 30.94m, new DateTime(2025, 10, 22, 9, 35, 47, 323, DateTimeKind.Utc).AddTicks(1166), 18 },
                    { new Guid("019c3c9b-767b-7717-a3cd-a8b01c232f5b"), new DateTime(2026, 2, 8, 9, 35, 47, 323, DateTimeKind.Utc).AddTicks(966), (byte)0, false, false, "Quae quod et", 16.57m, new DateTime(2025, 3, 13, 9, 35, 47, 323, DateTimeKind.Utc).AddTicks(994), 45 },
                    { new Guid("019c3c9b-767b-7ac1-8d0c-96a4abbda4c2"), new DateTime(2026, 2, 8, 9, 35, 47, 323, DateTimeKind.Utc).AddTicks(1169), (byte)0, false, false, "Accusantium repellat aut distinctio nihil magni", 5.11m, new DateTime(2025, 4, 28, 9, 35, 47, 323, DateTimeKind.Utc).AddTicks(1201), 87 },
                    { new Guid("019c3c9b-767b-7b2f-a03f-21e2c50c9b8f"), new DateTime(2026, 2, 8, 9, 35, 47, 323, DateTimeKind.Utc).AddTicks(243), (byte)0, false, false, "Assumenda dolorum et asperiores aut rerum", 48.60m, new DateTime(2025, 4, 1, 9, 35, 47, 323, DateTimeKind.Utc).AddTicks(887), 56 },
                    { new Guid("019c3c9b-767b-7c0b-8a1e-bd1fa208959a"), new DateTime(2026, 2, 8, 9, 35, 47, 323, DateTimeKind.Utc).AddTicks(903), (byte)0, false, false, "Est consectetur recusandae ipsam omnis", 5.04m, new DateTime(2025, 8, 24, 9, 35, 47, 323, DateTimeKind.Utc).AddTicks(959), 25 },
                    { new Guid("019c3c9b-767b-7db7-8245-f82acb7c6a32"), new DateTime(2026, 2, 8, 9, 35, 47, 323, DateTimeKind.Utc).AddTicks(1033), (byte)0, false, false, "Quod molestiae et omnis id consequatur quasi", 11.91m, new DateTime(2025, 2, 18, 9, 35, 47, 323, DateTimeKind.Utc).AddTicks(1120), 59 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("019c3c9b-7675-7855-a1bd-c4da7e20509d"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("019c3c9b-767b-72f0-971c-5a3bb9763d8b"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("019c3c9b-767b-74f3-bbcf-7fb0affdfbe8"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("019c3c9b-767b-7717-a3cd-a8b01c232f5b"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("019c3c9b-767b-7ac1-8d0c-96a4abbda4c2"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("019c3c9b-767b-7b2f-a03f-21e2c50c9b8f"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("019c3c9b-767b-7c0b-8a1e-bd1fa208959a"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("019c3c9b-767b-7db7-8245-f82acb7c6a32"));

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "CreatedAt", "Discount", "IsDeleted", "IsReleased", "Name", "Price", "ReleaseDate", "Stock" },
                values: new object[,]
                {
                    { new Guid("019c3c99-d3d4-70b5-bb69-6052de365d79"), new DateTime(2026, 2, 8, 9, 34, 0, 147, DateTimeKind.Utc).AddTicks(9812), (byte)0, false, false, "Qui labore asperiores alias iure dignissimos quisquam ratione", 25.28m, new DateTime(2025, 6, 2, 9, 34, 0, 152, DateTimeKind.Utc).AddTicks(8746), 43 },
                    { new Guid("019c3c99-d3d9-7033-b12b-3b84472bff28"), new DateTime(2026, 2, 8, 9, 34, 0, 153, DateTimeKind.Utc).AddTicks(5853), (byte)0, false, false, "Et enim quasi", 6.17m, new DateTime(2025, 4, 12, 9, 34, 0, 153, DateTimeKind.Utc).AddTicks(5869), 33 },
                    { new Guid("019c3c99-d3d9-7075-ab7a-b7b26555140b"), new DateTime(2026, 2, 8, 9, 34, 0, 153, DateTimeKind.Utc).AddTicks(5657), (byte)0, false, false, "Ut laudantium autem sed qui rerum", 6.10m, new DateTime(2025, 6, 7, 9, 34, 0, 153, DateTimeKind.Utc).AddTicks(5721), 147 },
                    { new Guid("019c3c99-d3d9-707f-9278-78802f9f06a9"), new DateTime(2026, 2, 8, 9, 34, 0, 153, DateTimeKind.Utc).AddTicks(5788), (byte)0, false, false, "Dolorem qui facilis autem", 11.84m, new DateTime(2025, 6, 14, 9, 34, 0, 153, DateTimeKind.Utc).AddTicks(5811), 104 },
                    { new Guid("019c3c99-d3d9-70cd-9214-4e7556bc4866"), new DateTime(2026, 2, 8, 9, 34, 0, 153, DateTimeKind.Utc).AddTicks(5239), (byte)0, false, false, "Et voluptatibus eos ad autem iure", 11.29m, new DateTime(2025, 11, 28, 9, 34, 0, 153, DateTimeKind.Utc).AddTicks(5289), 5 },
                    { new Guid("019c3c99-d3d9-71f6-8a7f-95c63ebd651a"), new DateTime(2026, 2, 8, 9, 34, 0, 153, DateTimeKind.Utc).AddTicks(4637), (byte)0, false, false, "Voluptatem at labore at nesciunt libero quo soluta", 2.12m, new DateTime(2025, 12, 16, 9, 34, 0, 153, DateTimeKind.Utc).AddTicks(5105), 45 },
                    { new Guid("019c3c99-d3d9-7211-b56f-fb78b5b4841c"), new DateTime(2026, 2, 8, 9, 34, 0, 153, DateTimeKind.Utc).AddTicks(5127), (byte)0, false, false, "Molestiae quod minima et est dolor nostrum", 31.17m, new DateTime(2025, 7, 2, 9, 34, 0, 153, DateTimeKind.Utc).AddTicks(5231), 127 },
                    { new Guid("019c3c99-d3d9-750c-abd3-878339d77a46"), new DateTime(2026, 2, 8, 9, 34, 0, 153, DateTimeKind.Utc).AddTicks(5611), (byte)0, false, false, "Optio qui aperiam voluptatem et perspiciatis qui et", 20.23m, new DateTime(2025, 7, 17, 9, 34, 0, 153, DateTimeKind.Utc).AddTicks(5655), 93 },
                    { new Guid("019c3c99-d3d9-7650-8969-fb03b2bcc806"), new DateTime(2026, 2, 8, 9, 34, 0, 153, DateTimeKind.Utc).AddTicks(5814), (byte)0, false, false, "Ab eaque distinctio ut ea placeat voluptatum occaecati", 35.02m, new DateTime(2025, 8, 24, 9, 34, 0, 153, DateTimeKind.Utc).AddTicks(5850), 147 },
                    { new Guid("019c3c99-d3d9-76a9-8be6-e89a3ced0298"), new DateTime(2026, 2, 8, 9, 34, 0, 153, DateTimeKind.Utc).AddTicks(5872), (byte)0, false, false, "Quam quis ut minima consequuntur et", 41.26m, new DateTime(2025, 9, 11, 9, 34, 0, 153, DateTimeKind.Utc).AddTicks(5902), 85 },
                    { new Guid("019c3c99-d3d9-76ee-87df-b8728a707dbf"), new DateTime(2026, 2, 8, 9, 34, 0, 153, DateTimeKind.Utc).AddTicks(5506), (byte)0, false, false, "Ut cum et incidunt deserunt totam", 24.01m, new DateTime(2025, 10, 28, 9, 34, 0, 153, DateTimeKind.Utc).AddTicks(5543), 86 },
                    { new Guid("019c3c99-d3d9-775c-a3d7-55a7ec5f9ab7"), new DateTime(2026, 2, 8, 9, 34, 0, 153, DateTimeKind.Utc).AddTicks(5575), (byte)0, false, false, "Voluptate animi aut impedit cum totam vel", 31.09m, new DateTime(2026, 1, 26, 9, 34, 0, 153, DateTimeKind.Utc).AddTicks(5609), 60 },
                    { new Guid("019c3c99-d3d9-7822-9ad6-c6c2d0221d21"), new DateTime(2026, 2, 8, 9, 34, 0, 153, DateTimeKind.Utc).AddTicks(5757), (byte)0, false, false, "Eaque error sed inventore", 40.12m, new DateTime(2025, 12, 25, 9, 34, 0, 153, DateTimeKind.Utc).AddTicks(5785), 106 },
                    { new Guid("019c3c99-d3d9-78bf-a0b4-2874ef0b6f73"), new DateTime(2026, 2, 8, 9, 34, 0, 153, DateTimeKind.Utc).AddTicks(5724), (byte)0, false, false, "In qui omnis dolores vel est nemo", 24.75m, new DateTime(2025, 8, 21, 9, 34, 0, 153, DateTimeKind.Utc).AddTicks(5754), 109 },
                    { new Guid("019c3c99-d3d9-7983-b0c8-0401f6d0edf6"), new DateTime(2026, 2, 8, 9, 34, 0, 153, DateTimeKind.Utc).AddTicks(5326), (byte)0, false, false, "At ipsam esse quia eaque", 44.50m, new DateTime(2025, 4, 24, 9, 34, 0, 153, DateTimeKind.Utc).AddTicks(5361), 39 },
                    { new Guid("019c3c99-d3d9-7b9e-94ac-97da1adfef62"), new DateTime(2026, 2, 8, 9, 34, 0, 153, DateTimeKind.Utc).AddTicks(5440), (byte)0, false, false, "Est itaque quae non", 12.22m, new DateTime(2025, 9, 2, 9, 34, 0, 153, DateTimeKind.Utc).AddTicks(5467), 49 },
                    { new Guid("019c3c99-d3d9-7c45-b26a-feb2831ccfa5"), new DateTime(2026, 2, 8, 9, 34, 0, 153, DateTimeKind.Utc).AddTicks(5294), (byte)0, false, false, "Ut molestiae cupiditate", 28.79m, new DateTime(2025, 7, 26, 9, 34, 0, 153, DateTimeKind.Utc).AddTicks(5323), 98 },
                    { new Guid("019c3c99-d3d9-7c58-9b95-ab1589b94afc"), new DateTime(2026, 2, 8, 9, 34, 0, 153, DateTimeKind.Utc).AddTicks(5546), (byte)0, false, false, "Voluptates ex dolor asperiores sequi vero", 38.90m, new DateTime(2026, 1, 30, 9, 34, 0, 153, DateTimeKind.Utc).AddTicks(5573), 138 },
                    { new Guid("019c3c99-d3d9-7d50-a9cd-805a21f27f96"), new DateTime(2026, 2, 8, 9, 34, 0, 153, DateTimeKind.Utc).AddTicks(5469), (byte)0, false, false, "Est ex ut maxime laborum et", 19.18m, new DateTime(2026, 2, 5, 9, 34, 0, 153, DateTimeKind.Utc).AddTicks(5504), 108 },
                    { new Guid("019c3c99-d3d9-7df2-91a1-2202cdc18dcb"), new DateTime(2026, 2, 8, 9, 34, 0, 153, DateTimeKind.Utc).AddTicks(5414), (byte)0, false, false, "Est veniam nemo", 34.50m, new DateTime(2025, 9, 30, 9, 34, 0, 153, DateTimeKind.Utc).AddTicks(5437), 58 }
                });
        }
    }
}
