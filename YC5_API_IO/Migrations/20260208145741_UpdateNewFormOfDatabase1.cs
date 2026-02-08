using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YC5_API_IO.Migrations
{
    /// <inheritdoc />
    public partial class UpdateNewFormOfDatabase1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: "a18be9c0-aa65-4af8-bd17-002120485633",
                column: "CreatedAt",
                value: new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: "c2a1e1b2-3e4f-5a6b-7c8d-9e0f1a2b3c4d",
                column: "CreatedAt",
                value: new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: "e5f6g7h8-i9j0-k1l2-m3n4-o5p6q7r8s9t0",
                column: "CreatedAt",
                value: new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: "a18be9c0-aa65-4af8-bd17-002120485633",
                column: "CreatedAt",
                value: new DateTime(2026, 2, 8, 14, 55, 8, 273, DateTimeKind.Utc).AddTicks(7877));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: "c2a1e1b2-3e4f-5a6b-7c8d-9e0f1a2b3c4d",
                column: "CreatedAt",
                value: new DateTime(2026, 2, 8, 14, 55, 8, 273, DateTimeKind.Utc).AddTicks(9057));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: "e5f6g7h8-i9j0-k1l2-m3n4-o5p6q7r8s9t0",
                column: "CreatedAt",
                value: new DateTime(2026, 2, 8, 14, 55, 8, 273, DateTimeKind.Utc).AddTicks(9062));
        }
    }
}
