using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookSwap.Migrations
{
    /// <inheritdoc />
    public partial class addroles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "6fc90bc5-2299-45fc-83c6-b2ada331c510", "1", "Admin", "ADMIN" },
                    { "82126c48-4b26-40fd-943e-c25de5ce78da", "2", "Book Owner", "BOOK OWNER" },
                    { "d43566d9-a8d1-436c-881f-8fe6d2532d3a", "3", "Reader", "READER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6fc90bc5-2299-45fc-83c6-b2ada331c510");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "82126c48-4b26-40fd-943e-c25de5ce78da");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d43566d9-a8d1-436c-881f-8fe6d2532d3a");
        }
    }
}
