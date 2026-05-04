using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WorkRequestTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "WorkRequests",
                columns: new[] { "Id", "ClientName", "CreatedAt", "Description", "DueDate", "Priority", "Status", "Title", "UpdatedAt" },
                values: new object[,]
                {
                    { "a1b2c3d4-0001-0000-0000-000000000001", "Acme Corp", new DateTimeOffset(new DateTime(2026, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Complete overhaul of the main landing page with new branding guidelines and responsive layout.", new DateTimeOffset(new DateTime(2026, 8, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 2, 1, "Redesign Landing Page", new DateTimeOffset(new DateTime(2026, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { "a1b2c3d4-0002-0000-0000-000000000002", "GlobalTech Solutions", new DateTimeOffset(new DateTime(2026, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Payment processing times out after 30 seconds on high-traffic days. Investigate and fix.", new DateTimeOffset(new DateTime(2026, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 2, 2, "Fix Payment Gateway Timeout", new DateTimeOffset(new DateTime(2026, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { "a1b2c3d4-0003-0000-0000-000000000003", "Acme Corp", new DateTimeOffset(new DateTime(2026, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Users need the ability to export their order history to CSV format from the dashboard.", new DateTimeOffset(new DateTime(2026, 7, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, 0, "Add Export to CSV Feature", new DateTimeOffset(new DateTime(2026, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { "a1b2c3d4-0004-0000-0000-000000000004", "Northwind Traders", new DateTimeOffset(new DateTime(2026, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Legal team provided updated privacy policy text. Replace existing content and update footer links.", new DateTimeOffset(new DateTime(2026, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 0, 3, "Update Privacy Policy Page", new DateTimeOffset(new DateTime(2026, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { "a1b2c3d4-0005-0000-0000-000000000005", "Summit Financial", new DateTimeOffset(new DateTime(2026, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Implement push notification support for iOS and Android. Include transaction alerts and marketing opt-in.", new DateTimeOffset(new DateTime(2026, 6, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, 0, "Mobile App Push Notifications", new DateTimeOffset(new DateTime(2026, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) }
                });

            migrationBuilder.InsertData(
                table: "Notes",
                columns: new[] { "Id", "Content", "CreatedAt", "WorkRequestId" },
                values: new object[,]
                {
                    { "b1b2c3d4-0001-0000-0000-000000000001", "Initial mockups approved by the client. Moving to development phase.", new DateTimeOffset(new DateTime(2026, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "a1b2c3d4-0001-0000-0000-000000000001" },
                    { "b1b2c3d4-0002-0000-0000-000000000002", "Header and hero section completed. Footer still in progress.", new DateTimeOffset(new DateTime(2026, 5, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "a1b2c3d4-0001-0000-0000-000000000001" },
                    { "b1b2c3d4-0003-0000-0000-000000000003", "Confirmed the timeout is on the payment provider's side. Awaiting their API fix.", new DateTimeOffset(new DateTime(2026, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "a1b2c3d4-0002-0000-0000-000000000002" },
                    { "b1b2c3d4-0004-0000-0000-000000000004", "Provider says fix will be deployed next week. Keeping status as Blocked.", new DateTimeOffset(new DateTime(2026, 5, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "a1b2c3d4-0002-0000-0000-000000000002" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Notes",
                keyColumn: "Id",
                keyValue: "b1b2c3d4-0001-0000-0000-000000000001");

            migrationBuilder.DeleteData(
                table: "Notes",
                keyColumn: "Id",
                keyValue: "b1b2c3d4-0002-0000-0000-000000000002");

            migrationBuilder.DeleteData(
                table: "Notes",
                keyColumn: "Id",
                keyValue: "b1b2c3d4-0003-0000-0000-000000000003");

            migrationBuilder.DeleteData(
                table: "Notes",
                keyColumn: "Id",
                keyValue: "b1b2c3d4-0004-0000-0000-000000000004");

            migrationBuilder.DeleteData(
                table: "WorkRequests",
                keyColumn: "Id",
                keyValue: "a1b2c3d4-0003-0000-0000-000000000003");

            migrationBuilder.DeleteData(
                table: "WorkRequests",
                keyColumn: "Id",
                keyValue: "a1b2c3d4-0004-0000-0000-000000000004");

            migrationBuilder.DeleteData(
                table: "WorkRequests",
                keyColumn: "Id",
                keyValue: "a1b2c3d4-0005-0000-0000-000000000005");

            migrationBuilder.DeleteData(
                table: "WorkRequests",
                keyColumn: "Id",
                keyValue: "a1b2c3d4-0001-0000-0000-000000000001");

            migrationBuilder.DeleteData(
                table: "WorkRequests",
                keyColumn: "Id",
                keyValue: "a1b2c3d4-0002-0000-0000-000000000002");
        }
    }
}
