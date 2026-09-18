using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wastelands.Service.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameItemAttacksPerTurnToIsMultiAttack : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttacksPerTurn",
                table: "ItemTemplate");

            migrationBuilder.DropColumn(
                name: "AttacksPerTurn",
                table: "Item");

            migrationBuilder.AddColumn<bool>(
                name: "IsMultiAttack",
                table: "ItemTemplate",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsMultiAttack",
                table: "Item",
                type: "boolean",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsMultiAttack",
                table: "ItemTemplate");

            migrationBuilder.DropColumn(
                name: "IsMultiAttack",
                table: "Item");

            migrationBuilder.AddColumn<int>(
                name: "AttacksPerTurn",
                table: "ItemTemplate",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AttacksPerTurn",
                table: "Item",
                type: "integer",
                nullable: true);
        }
    }
}
