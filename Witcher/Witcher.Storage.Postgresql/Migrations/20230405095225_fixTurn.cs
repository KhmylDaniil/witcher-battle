using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Witcher.Storage.Postgresql.Migrations
{
    public partial class fixTurn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Turn_AdditionalAttackMaked",
                schema: "Battles",
                table: "Creatures");

            migrationBuilder.DropColumn(
                name: "Turn_TurnBeginningEffectsAreTriggered",
                schema: "Battles",
                table: "Creatures");

            migrationBuilder.RenameColumn(
                name: "Turn_DefenseInThisTurnMaked",
                schema: "Battles",
                table: "Creatures",
                newName: "Turn_TurnState");

            migrationBuilder.AddColumn<int>(
                name: "Turn_IsDefenseInThisTurnPerformed",
                schema: "Battles",
                table: "Creatures",
                type: "integer",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Turn_IsDefenseInThisTurnPerformed",
                schema: "Battles",
                table: "Creatures");

            migrationBuilder.RenameColumn(
                name: "Turn_TurnState",
                schema: "Battles",
                table: "Creatures",
                newName: "Turn_DefenseInThisTurnMaked");

            migrationBuilder.AddColumn<bool>(
                name: "Turn_AdditionalAttackMaked",
                schema: "Battles",
                table: "Creatures",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Turn_TurnBeginningEffectsAreTriggered",
                schema: "Battles",
                table: "Creatures",
                type: "boolean",
                nullable: true);
        }
    }
}
