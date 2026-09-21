using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wastelands.Service.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAbilityAttackModifier : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AttackModifier",
                table: "ItemTemplate",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AttackModifier",
                table: "Item",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AttackModifier",
                table: "Ability",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttackModifier",
                table: "ItemTemplate");

            migrationBuilder.DropColumn(
                name: "AttackModifier",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "AttackModifier",
                table: "Ability");
        }
    }
}
