using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookSwap.Migrations
{
    /// <inheritdoc />
    public partial class finallllllll : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                    { "a56ba8a4-e715-4c4c-82fb-85993940cdbd", "3", "Reader", "READER" },
                    { "a8e1684a-29de-4e3b-91e2-c7c0c0dc549a", "2", "Book Owner", "BOOK OWNER" },
                    { "c1e83e06-8e54-4a14-b59c-752dc38fd218", "1", "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a56ba8a4-e715-4c4c-82fb-85993940cdbd");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a8e1684a-29de-4e3b-91e2-c7c0c0dc549a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c1e83e06-8e54-4a14-b59c-752dc38fd218");

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
    }
}
