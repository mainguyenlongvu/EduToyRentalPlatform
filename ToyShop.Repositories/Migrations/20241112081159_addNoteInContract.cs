using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToyShop.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class addNoteInContract : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "ContractEntitys",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Note",
                table: "ContractEntitys");
        }
    }
}
