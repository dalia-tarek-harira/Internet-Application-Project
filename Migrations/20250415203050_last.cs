using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookSwap.Migrations
{
    /// <inheritdoc />
    public partial class last : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReaderId",
                table: "Likes");

            migrationBuilder.DropColumn(
                name: "ReaderId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "ReaderId",
                table: "BorrowRequests");

            migrationBuilder.DropColumn(
                name: "BookOwnerId",
                table: "BookPosts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReaderId",
                table: "Likes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ReaderId",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ReaderId",
                table: "BorrowRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BookOwnerId",
                table: "BookPosts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
