using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookSwap.Migrations
{
    /// <inheritdoc />
    public partial class newone : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<string>(
                name: "bookownername",
                table: "BookPosts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "bookownername",
                table: "BookPosts");

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
    }
}
