using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project.Api.Migrations
{
    /// <inheritdoc />
    public partial class ChangeValueType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Key",
                table: "Metrics",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "Value",
                table: "Metrics",
                type: "jsonb",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Metrics_Key",
                table: "Metrics",
                column: "Key");

            migrationBuilder.CreateIndex(
                name: "IX_Metrics_MeasuredAt",
                table: "Metrics",
                column: "MeasuredAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Metrics_Key",
                table: "Metrics");

            migrationBuilder.DropIndex(
                name: "IX_Metrics_MeasuredAt",
                table: "Metrics");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "Metrics");

            migrationBuilder.AlterColumn<string>(
                name: "Key",
                table: "Metrics",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128);
        }
    }
}
