using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthorizationServer.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddApplicationsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("be086da5-7afe-42b9-905b-f4f178506b7c"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2025, 10, 8, 11, 38, 49, 890, DateTimeKind.Utc).AddTicks(3681),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2025, 10, 1, 19, 30, 12, 106, DateTimeKind.Utc).AddTicks(699));

            migrationBuilder.CreateTable(
                name: "Applications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ClientId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ClientSecret = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    RedirectUrl = table.Column<List<string>>(type: "text[]", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserApplications",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ApplicationId = table.Column<Guid>(type: "uuid", nullable: false),
                    Roles = table.Column<List<string>>(type: "text[]", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserApplications", x => new { x.UserId, x.ApplicationId });
                    table.ForeignKey(
                        name: "FK_UserApplications_Applications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Applications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserApplications_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Name", "PasswordHash" },
                values: new object[] { new Guid("fb85c3a5-2172-4564-b3c3-581039e38e95"), new DateTime(2025, 10, 8, 11, 38, 49, 890, DateTimeKind.Utc).AddTicks(5251), "Admin", "AA1CE2A3A986AC52838A99ED0B587E36:9A46234CD9666671BB30B79C8C583B85D708A9E1D2992FA1B09EE25CD73BD2F1" });

            migrationBuilder.CreateIndex(
                name: "IX_Applications_ClientId",
                table: "Applications",
                column: "ClientId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserApplications_ApplicationId",
                table: "UserApplications",
                column: "ApplicationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserApplications");

            migrationBuilder.DropTable(
                name: "Applications");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("fb85c3a5-2172-4564-b3c3-581039e38e95"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2025, 10, 1, 19, 30, 12, 106, DateTimeKind.Utc).AddTicks(699),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2025, 10, 8, 11, 38, 49, 890, DateTimeKind.Utc).AddTicks(3681));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Name", "PasswordHash" },
                values: new object[] { new Guid("be086da5-7afe-42b9-905b-f4f178506b7c"), new DateTime(2025, 10, 1, 19, 30, 12, 106, DateTimeKind.Utc).AddTicks(1580), "Admin", "AA1CE2A3A986AC52838A99ED0B587E36:9A46234CD9666671BB30B79C8C583B85D708A9E1D2992FA1B09EE25CD73BD2F1" });
        }
    }
}
