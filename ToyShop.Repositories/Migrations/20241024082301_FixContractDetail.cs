using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ToyShop.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class FixContractDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Toy",
                keyColumn: "Id",
                keyValue: "166ee3e4648d428799e27e2d8dfa1eb3");

            migrationBuilder.DeleteData(
                table: "Toy",
                keyColumn: "Id",
                keyValue: "4074b81ea0f74052922780749d74bfce");

            migrationBuilder.DeleteData(
                table: "Toy",
                keyColumn: "Id",
                keyValue: "79b8a98793284650ba81456317dbe3fc");

            migrationBuilder.AlterColumn<int>(
                name: "ToyQuality",
                table: "RestoreToyDetails",
                type: "int",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TotalMoney",
                table: "RestoreToyDetails",
                type: "int",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Compensation",
                table: "RestoreToyDetails",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ToyId",
                table: "RestoreToyDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Price",
                table: "ContractDetails",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.InsertData(
                table: "Toy",
                columns: new[] { "Id", "CreatedBy", "CreatedTime", "DeletedBy", "DeletedTime", "LastUpdatedBy", "LastUpdatedTime", "Option", "ToyDescription", "ToyImg", "ToyName", "ToyPriceRent", "ToyPriceSale", "ToyQuantitySold", "ToyRemainingQuantity" },
                values: new object[,]
                {
                    { "5a5fc0f48eae4c2faa7ebe311cfc182b", "Admin", new DateTimeOffset(new DateTime(2024, 9, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)), null, null, "Admin", new DateTimeOffset(new DateTime(2024, 9, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)), "Puzzle", "A wooden puzzle with animal shapes and numbers.", "wooden_puzzle.webp", "Wooden Puzzle", 100, 120000, 6, 15 },
                    { "c1375a1719e54ca7ac2b31270cf6ed0f", "Admin", new DateTimeOffset(new DateTime(2024, 9, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)), null, null, "Admin", new DateTimeOffset(new DateTime(2024, 9, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)), "Interactive Learning", "A vibrant interactive toy set designed for toddlers to learn shapes, numbers, and colors.", "1.webp", "Educational Toy Set", 1000, 200000000, 8, 12 },
                    { "d7aed8d4cc1d4c40a85c97a333242a12", "Admin", new DateTimeOffset(new DateTime(2024, 9, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)), null, null, "Admin", new DateTimeOffset(new DateTime(2024, 9, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)), "Stackable Rings", "Classic colorful stacking rings toy for toddlers.", "stacking_rings.webp", "Stacking Rings", 1500, 150000000, 5, 20 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Toy",
                keyColumn: "Id",
                keyValue: "5a5fc0f48eae4c2faa7ebe311cfc182b");

            migrationBuilder.DeleteData(
                table: "Toy",
                keyColumn: "Id",
                keyValue: "c1375a1719e54ca7ac2b31270cf6ed0f");

            migrationBuilder.DeleteData(
                table: "Toy",
                keyColumn: "Id",
                keyValue: "d7aed8d4cc1d4c40a85c97a333242a12");

            migrationBuilder.DropColumn(
                name: "Compensation",
                table: "RestoreToyDetails");

            migrationBuilder.DropColumn(
                name: "ToyId",
                table: "RestoreToyDetails");

            migrationBuilder.AlterColumn<double>(
                name: "ToyQuality",
                table: "RestoreToyDetails",
                type: "float",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "TotalMoney",
                table: "RestoreToyDetails",
                type: "float",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "ContractDetails",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.InsertData(
                table: "Toy",
                columns: new[] { "Id", "CreatedBy", "CreatedTime", "DeletedBy", "DeletedTime", "LastUpdatedBy", "LastUpdatedTime", "Option", "ToyDescription", "ToyImg", "ToyName", "ToyPriceRent", "ToyPriceSale", "ToyQuantitySold", "ToyRemainingQuantity" },
                values: new object[,]
                {
                    { "166ee3e4648d428799e27e2d8dfa1eb3", "Admin", new DateTimeOffset(new DateTime(2024, 9, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)), null, null, "Admin", new DateTimeOffset(new DateTime(2024, 9, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)), "Puzzle", "A wooden puzzle with animal shapes and numbers.", "wooden_puzzle.webp", "Wooden Puzzle", 100, 120000, 6, 15 },
                    { "4074b81ea0f74052922780749d74bfce", "Admin", new DateTimeOffset(new DateTime(2024, 9, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)), null, null, "Admin", new DateTimeOffset(new DateTime(2024, 9, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)), "Stackable Rings", "Classic colorful stacking rings toy for toddlers.", "stacking_rings.webp", "Stacking Rings", 1500, 150000000, 5, 20 },
                    { "79b8a98793284650ba81456317dbe3fc", "Admin", new DateTimeOffset(new DateTime(2024, 9, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)), null, null, "Admin", new DateTimeOffset(new DateTime(2024, 9, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)), "Interactive Learning", "A vibrant interactive toy set designed for toddlers to learn shapes, numbers, and colors.", "1.webp", "Educational Toy Set", 1000, 200000000, 8, 12 }
                });
        }
    }
}
