using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ToyShop.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class AddContractDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContractEntitys_Toy_ToyId",
                table: "ContractEntitys");

            migrationBuilder.DropIndex(
                name: "IX_ContractEntitys_ToyId",
                table: "ContractEntitys");

            migrationBuilder.DropColumn(
                name: "ToyId",
                table: "ContractEntitys");

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

            migrationBuilder.CreateTable(
                name: "ContractDetails",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ContractId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ToyId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastUpdatedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DeletedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContractDetails_ContractEntitys_ContractId",
                        column: x => x.ContractId,
                        principalTable: "ContractEntitys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContractDetails_Toy_ToyId",
                        column: x => x.ToyId,
                        principalTable: "Toy",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Toy",
                columns: new[] { "Id", "CreatedBy", "CreatedTime", "DeletedBy", "DeletedTime", "LastUpdatedBy", "LastUpdatedTime", "Option", "ToyDescription", "ToyImg", "ToyName", "ToyPrice", "ToyQuantitySold", "ToyRemainingQuantity" },
                values: new object[,]
                {
                    { "87b34c9830334f83b78c28b6497982ad", "Admin", new DateTimeOffset(new DateTime(2024, 9, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)), null, null, "Admin", new DateTimeOffset(new DateTime(2024, 9, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)), "Stackable Rings", "Classic colorful stacking rings toy for toddlers.", "stacking_rings.webp", "Stacking Rings", 150000000, 5, 20 },
                    { "bd01c39be0554c0eb0aa7d7c0eeed582", "Admin", new DateTimeOffset(new DateTime(2024, 9, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)), null, null, "Admin", new DateTimeOffset(new DateTime(2024, 9, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)), "Interactive Learning", "A vibrant interactive toy set designed for toddlers to learn shapes, numbers, and colors.", "1.webp", "Educational Toy Set", 200000000, 8, 12 },
                    { "ffefbd4170054f91891918f442e7fad5", "Admin", new DateTimeOffset(new DateTime(2024, 9, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)), null, null, "Admin", new DateTimeOffset(new DateTime(2024, 9, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)), "Puzzle", "A wooden puzzle with animal shapes and numbers.", "wooden_puzzle.webp", "Wooden Puzzle", 120000, 6, 15 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_FeedBack_UserId1",
                table: "FeedBack",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_ContractDetails_ContractId",
                table: "ContractDetails",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractDetails_ToyId",
                table: "ContractDetails",
                column: "ToyId");

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

            migrationBuilder.DropTable(
                name: "ContractDetails");

            migrationBuilder.DropIndex(
                name: "IX_FeedBack_UserId1",
                table: "FeedBack");

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

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "FeedBack");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "FeedBack");

            migrationBuilder.AddColumn<string>(
                name: "ToyId",
                table: "ContractEntitys",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContractEntitys_ToyId",
                table: "ContractEntitys",
                column: "ToyId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContractEntitys_Toy_ToyId",
                table: "ContractEntitys",
                column: "ToyId",
                principalTable: "Toy",
                principalColumn: "Id");
        }
    }
}
