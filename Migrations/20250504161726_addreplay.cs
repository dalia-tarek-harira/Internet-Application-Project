using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookSwap.Migrations
{
    /// <inheritdoc />
    public partial class addreplay : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<string>(
                name: "ReplyText",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReplyUserId",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1b454498-7ab8-4470-9e72-c65ae1f238b1", "1", "Admin", "ADMIN" },
                    { "81b341a3-be93-4b13-8274-e57a7a4ecdbc", "2", "Book Owner", "BOOK OWNER" },
                    { "9fd69096-3bf5-46f9-8259-e44d99ad69da", "3", "Reader", "READER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1b454498-7ab8-4470-9e72-c65ae1f238b1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "81b341a3-be93-4b13-8274-e57a7a4ecdbc");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9fd69096-3bf5-46f9-8259-e44d99ad69da");

            migrationBuilder.DropColumn(
                name: "ReplyText",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "ReplyUserId",
                table: "Comments");

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
    }
}
