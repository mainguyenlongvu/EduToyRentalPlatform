using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ToyShop.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class seed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Users",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "FeedBack",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId1",
                table: "FeedBack",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Toy",
                columns: new[] { "Id", "CreatedBy", "CreatedTime", "DeletedBy", "DeletedTime", "LastUpdatedBy", "LastUpdatedTime", "Option", "ToyDescription", "ToyImg", "ToyName", "ToyPrice", "ToyQuantitySold", "ToyRemainingQuantity" },
                values: new object[,]
                {
                    { "225be041241943ee96eea4c54dee6b0f", "Admin", new DateTimeOffset(new DateTime(2024, 9, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)), null, null, "Admin", new DateTimeOffset(new DateTime(2024, 9, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)), "Interactive Learning", "A vibrant interactive toy set designed for toddlers to learn shapes, numbers, and colors.", "1.webp", "Educational Toy Set", 200000000, 8, 12 },
                    { "3a24ce8d358d4136aa517b7d614b5fcb", "Admin", new DateTimeOffset(new DateTime(2024, 9, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)), null, null, "Admin", new DateTimeOffset(new DateTime(2024, 9, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)), "Stackable Rings", "Classic colorful stacking rings toy for toddlers.", "stacking_rings.webp", "Stacking Rings", 150000000, 5, 20 },
                    { "6b16a0cca7ff4509b4eddb0862094612", "Admin", new DateTimeOffset(new DateTime(2024, 9, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)), null, null, "Admin", new DateTimeOffset(new DateTime(2024, 9, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)), "Puzzle", "A wooden puzzle with animal shapes and numbers.", "wooden_puzzle.webp", "Wooden Puzzle", 120000, 6, 15 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_FeedBack_UserId1",
                table: "FeedBack",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_FeedBack_Users_UserId1",
                table: "FeedBack",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeedBack_Users_UserId1",
                table: "FeedBack");

            migrationBuilder.DropIndex(
                name: "IX_FeedBack_UserId1",
                table: "FeedBack");

            migrationBuilder.DeleteData(
                table: "Toy",
                keyColumn: "Id",
                keyValue: "225be041241943ee96eea4c54dee6b0f");

            migrationBuilder.DeleteData(
                table: "Toy",
                keyColumn: "Id",
                keyValue: "3a24ce8d358d4136aa517b7d614b5fcb");

            migrationBuilder.DeleteData(
                table: "Toy",
                keyColumn: "Id",
                keyValue: "6b16a0cca7ff4509b4eddb0862094612");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "FeedBack");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "FeedBack");
        }
    }
}
