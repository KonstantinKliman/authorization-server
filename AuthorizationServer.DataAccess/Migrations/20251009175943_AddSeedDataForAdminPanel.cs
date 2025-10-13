using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthorizationServer.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddSeedDataForAdminPanel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("3be62e69-764f-43ce-89ce-d5ad68cea9af"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2025, 10, 9, 17, 59, 42, 430, DateTimeKind.Utc).AddTicks(2596),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2025, 10, 9, 15, 56, 34, 231, DateTimeKind.Utc).AddTicks(657));

            migrationBuilder.InsertData(
                table: "Applications",
                columns: new[] { "Id", "ClientId", "ClientSecret", "CreatedAt", "Description", "Name", "RedirectUrls" },
                values: new object[] { new Guid("94e7fd21-7b93-4633-9417-2cd54b0f315c"), "admin-panel", "AA1CE2A3A986AC52838A99ED0B587E36:9A46234CD9666671BB30B79C8C583B85D708A9E1D2992FA1B09EE25CD73BD2F1", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Admin Panel", new List<string> { "http://localhost:4200/admin/callback" } });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Name", "PasswordHash" },
                values: new object[] { new Guid("15c25363-9a63-423c-b4b2-2e1acd1107cc"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Admin", "AA1CE2A3A986AC52838A99ED0B587E36:9A46234CD9666671BB30B79C8C583B85D708A9E1D2992FA1B09EE25CD73BD2F1" });

            migrationBuilder.InsertData(
                table: "UserApplications",
                columns: new[] { "ApplicationId", "UserId", "Roles" },
                values: new object[] { new Guid("94e7fd21-7b93-4633-9417-2cd54b0f315c"), new Guid("15c25363-9a63-423c-b4b2-2e1acd1107cc"), new List<string> { "RootAdmin" } });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserApplications",
                keyColumns: new[] { "ApplicationId", "UserId" },
                keyValues: new object[] { new Guid("94e7fd21-7b93-4633-9417-2cd54b0f315c"), new Guid("15c25363-9a63-423c-b4b2-2e1acd1107cc") });

            migrationBuilder.DeleteData(
                table: "Applications",
                keyColumn: "Id",
                keyValue: new Guid("94e7fd21-7b93-4633-9417-2cd54b0f315c"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("15c25363-9a63-423c-b4b2-2e1acd1107cc"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2025, 10, 9, 15, 56, 34, 231, DateTimeKind.Utc).AddTicks(657),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2025, 10, 9, 17, 59, 42, 430, DateTimeKind.Utc).AddTicks(2596));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Name", "PasswordHash" },
                values: new object[] { new Guid("3be62e69-764f-43ce-89ce-d5ad68cea9af"), new DateTime(2025, 10, 9, 15, 56, 34, 231, DateTimeKind.Utc).AddTicks(1689), "Admin", "AA1CE2A3A986AC52838A99ED0B587E36:9A46234CD9666671BB30B79C8C583B85D708A9E1D2992FA1B09EE25CD73BD2F1" });
        }
    }
}
