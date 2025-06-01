using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookSwap.Migrations
{
    /// <inheritdoc />
    public partial class newww : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "479b0eff-4060-4719-bb30-8c4a8e6217f8", "2", "Book Owner", "BOOK OWNER" },
                    { "b8ae8b8c-d042-4abd-84c6-36748214f5ad", "3", "Reader", "READER" },
                    { "ddd7dfdc-545b-4ecb-a0c7-210f0c73d4c5", "1", "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "479b0eff-4060-4719-bb30-8c4a8e6217f8");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b8ae8b8c-d042-4abd-84c6-36748214f5ad");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ddd7dfdc-545b-4ecb-a0c7-210f0c73d4c5");

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
    }
}
