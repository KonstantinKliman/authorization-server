using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthorizationServer.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddDescriptionToApplicationsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("24eb7d79-e522-4a15-9579-b9c2a94e918a"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2025, 10, 9, 15, 56, 34, 231, DateTimeKind.Utc).AddTicks(657),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2025, 10, 8, 12, 17, 26, 487, DateTimeKind.Utc).AddTicks(4159));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Applications",
                type: "text",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Name", "PasswordHash" },
                values: new object[] { new Guid("3be62e69-764f-43ce-89ce-d5ad68cea9af"), new DateTime(2025, 10, 9, 15, 56, 34, 231, DateTimeKind.Utc).AddTicks(1689), "Admin", "AA1CE2A3A986AC52838A99ED0B587E36:9A46234CD9666671BB30B79C8C583B85D708A9E1D2992FA1B09EE25CD73BD2F1" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("3be62e69-764f-43ce-89ce-d5ad68cea9af"));

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Applications");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2025, 10, 8, 12, 17, 26, 487, DateTimeKind.Utc).AddTicks(4159),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2025, 10, 9, 15, 56, 34, 231, DateTimeKind.Utc).AddTicks(657));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Name", "PasswordHash" },
                values: new object[] { new Guid("24eb7d79-e522-4a15-9579-b9c2a94e918a"), new DateTime(2025, 10, 8, 12, 17, 26, 487, DateTimeKind.Utc).AddTicks(5042), "Admin", "AA1CE2A3A986AC52838A99ED0B587E36:9A46234CD9666671BB30B79C8C583B85D708A9E1D2992FA1B09EE25CD73BD2F1" });
        }
    }
}
