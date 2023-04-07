using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Witcher.Storage.Postgresql.Migrations
{
    public partial class addTurnFixDefensiveSkill : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DefensiveSkill",
                schema: "GameRules",
                table: "DefensiveSkill");

            migrationBuilder.DropIndex(
                name: "IX_DefensiveSkill_AbilityId",
                schema: "GameRules",
                table: "DefensiveSkill");

            migrationBuilder.RenameColumn(
                name: "TurnBeginningEffectsAreTriggered",
                schema: "Battles",
                table: "Creatures",
                newName: "Turn_TurnBeginningEffectsAreTriggered");

            migrationBuilder.AlterColumn<bool>(
                name: "Turn_TurnBeginningEffectsAreTriggered",
                schema: "Battles",
                table: "Creatures",
                type: "boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AddColumn<bool>(
                name: "Turn_AdditionalAttackMaked",
                schema: "Battles",
                table: "Creatures",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Turn_DefenseInThisTurnMaked",
                schema: "Battles",
                table: "Creatures",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Turn_EnergySpendInThisTurn",
                schema: "Battles",
                table: "Creatures",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Turn_MuitiattackAbilityId",
                schema: "Battles",
                table: "Creatures",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Turn_MultiattackRemainsQuantity",
                schema: "Battles",
                table: "Creatures",
                type: "integer",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DefensiveSkill",
                schema: "GameRules",
                table: "DefensiveSkill",
                columns: new[] { "AbilityId", "Id" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DefensiveSkill",
                schema: "GameRules",
                table: "DefensiveSkill");

            migrationBuilder.DropColumn(
                name: "Turn_AdditionalAttackMaked",
                schema: "Battles",
                table: "Creatures");

            migrationBuilder.DropColumn(
                name: "Turn_DefenseInThisTurnMaked",
                schema: "Battles",
                table: "Creatures");

            migrationBuilder.DropColumn(
                name: "Turn_EnergySpendInThisTurn",
                schema: "Battles",
                table: "Creatures");

            migrationBuilder.DropColumn(
                name: "Turn_MuitiattackAbilityId",
                schema: "Battles",
                table: "Creatures");

            migrationBuilder.DropColumn(
                name: "Turn_MultiattackRemainsQuantity",
                schema: "Battles",
                table: "Creatures");

            migrationBuilder.RenameColumn(
                name: "Turn_TurnBeginningEffectsAreTriggered",
                schema: "Battles",
                table: "Creatures",
                newName: "TurnBeginningEffectsAreTriggered");

            migrationBuilder.AlterColumn<bool>(
                name: "TurnBeginningEffectsAreTriggered",
                schema: "Battles",
                table: "Creatures",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DefensiveSkill",
                schema: "GameRules",
                table: "DefensiveSkill",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_DefensiveSkill_AbilityId",
                schema: "GameRules",
                table: "DefensiveSkill",
                column: "AbilityId");
        }
    }
}
