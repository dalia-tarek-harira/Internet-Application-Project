using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookSwap.Migrations
{
    /// <inheritdoc />
    public partial class last_isa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                    { "11690987-759b-4fd1-9931-9708691a3161", "2", "Book Owner", "BOOK OWNER" },
                    { "2d4e2cce-134a-4e2b-b9f1-f1f27b868a4b", "3", "Reader", "READER" },
                    { "f90a7595-2dfb-4478-b57e-9a546edaa652", "1", "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "11690987-759b-4fd1-9931-9708691a3161");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2d4e2cce-134a-4e2b-b9f1-f1f27b868a4b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f90a7595-2dfb-4478-b57e-9a546edaa652");

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
    }
}
