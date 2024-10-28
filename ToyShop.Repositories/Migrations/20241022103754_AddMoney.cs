using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ToyShop.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class AddMoney : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Toy",
                keyColumn: "Id",
                keyValue: "87b34c9830334f83b78c28b6497982ad");

            migrationBuilder.DeleteData(
                table: "Toy",
                keyColumn: "Id",
                keyValue: "bd01c39be0554c0eb0aa7d7c0eeed582");

            migrationBuilder.DeleteData(
                table: "Toy",
                keyColumn: "Id",
                keyValue: "ffefbd4170054f91891918f442e7fad5");

            migrationBuilder.AddColumn<int>(
                name: "Money",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "Toy",
                columns: new[] { "Id", "CreatedBy", "CreatedTime", "DeletedBy", "DeletedTime", "LastUpdatedBy", "LastUpdatedTime", "Option", "ToyDescription", "ToyImg", "ToyName", "ToyPrice", "ToyQuantitySold", "ToyRemainingQuantity" },
                values: new object[,]
                {
                    { "a7946ba90f514af58c7e9a0be3d0437a", "Admin", new DateTimeOffset(new DateTime(2024, 9, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)), null, null, "Admin", new DateTimeOffset(new DateTime(2024, 9, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)), "Interactive Learning", "A vibrant interactive toy set designed for toddlers to learn shapes, numbers, and colors.", "1.webp", "Educational Toy Set", 200000000, 8, 12 },
                    { "db45c2f51a1d46bcbbb18ed68459dbea", "Admin", new DateTimeOffset(new DateTime(2024, 9, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)), null, null, "Admin", new DateTimeOffset(new DateTime(2024, 9, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)), "Puzzle", "A wooden puzzle with animal shapes and numbers.", "wooden_puzzle.webp", "Wooden Puzzle", 120000, 6, 15 },
                    { "e98713ac01064ffa870839bb1418172c", "Admin", new DateTimeOffset(new DateTime(2024, 9, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)), null, null, "Admin", new DateTimeOffset(new DateTime(2024, 9, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)), "Stackable Rings", "Classic colorful stacking rings toy for toddlers.", "stacking_rings.webp", "Stacking Rings", 150000000, 5, 20 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Toy",
                keyColumn: "Id",
                keyValue: "a7946ba90f514af58c7e9a0be3d0437a");

            migrationBuilder.DeleteData(
                table: "Toy",
                keyColumn: "Id",
                keyValue: "db45c2f51a1d46bcbbb18ed68459dbea");

            migrationBuilder.DeleteData(
                table: "Toy",
                keyColumn: "Id",
                keyValue: "e98713ac01064ffa870839bb1418172c");

            migrationBuilder.DropColumn(
                name: "Money",
                table: "Users");

            migrationBuilder.InsertData(
                table: "Toy",
                columns: new[] { "Id", "CreatedBy", "CreatedTime", "DeletedBy", "DeletedTime", "LastUpdatedBy", "LastUpdatedTime", "Option", "ToyDescription", "ToyImg", "ToyName", "ToyPrice", "ToyQuantitySold", "ToyRemainingQuantity" },
                values: new object[,]
                {
                    { "87b34c9830334f83b78c28b6497982ad", "Admin", new DateTimeOffset(new DateTime(2024, 9, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)), null, null, "Admin", new DateTimeOffset(new DateTime(2024, 9, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)), "Stackable Rings", "Classic colorful stacking rings toy for toddlers.", "stacking_rings.webp", "Stacking Rings", 150000000, 5, 20 },
                    { "bd01c39be0554c0eb0aa7d7c0eeed582", "Admin", new DateTimeOffset(new DateTime(2024, 9, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)), null, null, "Admin", new DateTimeOffset(new DateTime(2024, 9, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)), "Interactive Learning", "A vibrant interactive toy set designed for toddlers to learn shapes, numbers, and colors.", "1.webp", "Educational Toy Set", 200000000, 8, 12 },
                    { "ffefbd4170054f91891918f442e7fad5", "Admin", new DateTimeOffset(new DateTime(2024, 9, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)), null, null, "Admin", new DateTimeOffset(new DateTime(2024, 9, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)), "Puzzle", "A wooden puzzle with animal shapes and numbers.", "wooden_puzzle.webp", "Wooden Puzzle", 120000, 6, 15 }
                });
        }
    }
}
