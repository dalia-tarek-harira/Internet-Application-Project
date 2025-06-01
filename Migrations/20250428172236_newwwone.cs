using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookSwap.Migrations
{
    /// <inheritdoc />
    public partial class newwwone : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1eac8e07-d6e7-47e7-9c05-8155f7dbb550");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5205d724-7448-4046-8c68-03724ba63dbe");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "982e789a-930f-45aa-8474-f4944d2e5236");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "796505f5-6160-4fe2-b049-84213b1717cf", "1", "Admin", "ADMIN" },
                    { "c13c3eaa-50c9-4693-a37f-d131692f74c1", "2", "Book Owner", "BOOK OWNER" },
                    { "dee0b127-84d7-46e8-bcad-6f41d26aab77", "3", "Reader", "READER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "796505f5-6160-4fe2-b049-84213b1717cf");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c13c3eaa-50c9-4693-a37f-d131692f74c1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "dee0b127-84d7-46e8-bcad-6f41d26aab77");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1eac8e07-d6e7-47e7-9c05-8155f7dbb550", "1", "Admin", "ADMIN" },
                    { "5205d724-7448-4046-8c68-03724ba63dbe", "2", "Book Owner", "BOOK OWNER" },
                    { "982e789a-930f-45aa-8474-f4944d2e5236", "3", "Reader", "READER" }
                });
        }
    }
}
