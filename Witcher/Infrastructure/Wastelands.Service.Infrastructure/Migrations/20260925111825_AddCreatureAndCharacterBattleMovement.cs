using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wastelands.Service.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatureAndCharacterBattleMovement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrentMovement",
                table: "Creature",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaxMovement",
                table: "Creature",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CurrentMovement",
                table: "BattleCharacter",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaxMovement",
                table: "BattleCharacter",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentMovement",
                table: "Creature");

            migrationBuilder.DropColumn(
                name: "MaxMovement",
                table: "Creature");

            migrationBuilder.DropColumn(
                name: "CurrentMovement",
                table: "BattleCharacter");

            migrationBuilder.DropColumn(
                name: "MaxMovement",
                table: "BattleCharacter");
        }
    }
}
