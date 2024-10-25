using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ToyShop.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class FixDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "OverdueTime",
                table: "RestoreToys");

            migrationBuilder.DropColumn(
                name: "Reward",
                table: "RestoreToys");

            migrationBuilder.DropColumn(
                name: "ToyQuality",
                table: "RestoreToys");

            migrationBuilder.DropColumn(
                name: "ContractType",
                table: "ContractEntitys");

            migrationBuilder.DropColumn(
                name: "DateEnd",
                table: "ContractEntitys");

            migrationBuilder.DropColumn(
                name: "DateStart",
                table: "ContractEntitys");

            migrationBuilder.RenameColumn(
                name: "ToyPrice",
                table: "Toy",
                newName: "ToyPriceSale");

            migrationBuilder.AddColumn<int>(
                name: "ToyPriceRent",
                table: "Toy",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<double>(
                name: "TotalMoney",
                table: "RestoreToys",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "RestoreToys",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TotalReward",
                table: "RestoreToys",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "TotalToyQuality",
                table: "RestoreToys",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ContractType",
                table: "ContractDetails",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "DateEnd",
                table: "ContractDetails",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "DateStart",
                table: "ContractDetails",
                type: "date",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RestoreToyDetails",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RestoreToyId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ToyQuality = table.Column<double>(type: "float", nullable: true),
                    Reward = table.Column<int>(type: "int", nullable: true),
                    ToyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsReturn = table.Column<bool>(type: "bit", nullable: true),
                    OverdueTime = table.Column<double>(type: "float", nullable: true),
                    TotalMoney = table.Column<double>(type: "float", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastUpdatedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DeletedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RestoreToyDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RestoreToyDetails_RestoreToys_RestoreToyId",
                        column: x => x.RestoreToyId,
                        principalTable: "RestoreToys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Toy",
                columns: new[] { "Id", "CreatedBy", "CreatedTime", "DeletedBy", "DeletedTime", "LastUpdatedBy", "LastUpdatedTime", "Option", "ToyDescription", "ToyImg", "ToyName", "ToyPriceRent", "ToyPriceSale", "ToyQuantitySold", "ToyRemainingQuantity" },
                values: new object[,]
                {
                    { "166ee3e4648d428799e27e2d8dfa1eb3", "Admin", new DateTimeOffset(new DateTime(2024, 9, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)), null, null, "Admin", new DateTimeOffset(new DateTime(2024, 9, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)), "Puzzle", "A wooden puzzle with animal shapes and numbers.", "wooden_puzzle.webp", "Wooden Puzzle", 100, 120000, 6, 15 },
                    { "4074b81ea0f74052922780749d74bfce", "Admin", new DateTimeOffset(new DateTime(2024, 9, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)), null, null, "Admin", new DateTimeOffset(new DateTime(2024, 9, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)), "Stackable Rings", "Classic colorful stacking rings toy for toddlers.", "stacking_rings.webp", "Stacking Rings", 1500, 150000000, 5, 20 },
                    { "79b8a98793284650ba81456317dbe3fc", "Admin", new DateTimeOffset(new DateTime(2024, 9, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)), null, null, "Admin", new DateTimeOffset(new DateTime(2024, 9, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)), "Interactive Learning", "A vibrant interactive toy set designed for toddlers to learn shapes, numbers, and colors.", "1.webp", "Educational Toy Set", 1000, 200000000, 8, 12 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_RestoreToyDetails_RestoreToyId",
                table: "RestoreToyDetails",
                column: "RestoreToyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RestoreToyDetails");

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

            migrationBuilder.DropColumn(
                name: "ToyPriceRent",
                table: "Toy");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "RestoreToys");

            migrationBuilder.DropColumn(
                name: "TotalReward",
                table: "RestoreToys");

            migrationBuilder.DropColumn(
                name: "TotalToyQuality",
                table: "RestoreToys");

            migrationBuilder.DropColumn(
                name: "ContractType",
                table: "ContractDetails");

            migrationBuilder.DropColumn(
                name: "DateEnd",
                table: "ContractDetails");

            migrationBuilder.DropColumn(
                name: "DateStart",
                table: "ContractDetails");

            migrationBuilder.RenameColumn(
                name: "ToyPriceSale",
                table: "Toy",
                newName: "ToyPrice");

            migrationBuilder.AlterColumn<double>(
                name: "TotalMoney",
                table: "RestoreToys",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AddColumn<double>(
                name: "OverdueTime",
                table: "RestoreToys",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "Reward",
                table: "RestoreToys",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "ToyQuality",
                table: "RestoreToys",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<bool>(
                name: "ContractType",
                table: "ContractEntitys",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "DateEnd",
                table: "ContractEntitys",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "DateStart",
                table: "ContractEntitys",
                type: "date",
                nullable: true);

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
    }
}
