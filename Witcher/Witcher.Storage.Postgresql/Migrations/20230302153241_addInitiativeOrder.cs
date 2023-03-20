using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Witcher.Storage.Postgresql.Migrations
{
    public partial class addInitiativeOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActivationTime",
                schema: "Battles",
                table: "Battles");

            migrationBuilder.DropColumn(
                name: "Round",
                schema: "Battles",
                table: "Battles");

            migrationBuilder.AddColumn<int>(
                name: "InitiativeInBattle",
                schema: "Battles",
                table: "Creatures",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "Значение инициативы в битве");

            migrationBuilder.AddColumn<int>(
                name: "NextInitiative",
                schema: "Battles",
                table: "Battles",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "Значение инициативы следующего существа");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InitiativeInBattle",
                schema: "Battles",
                table: "Creatures");

            migrationBuilder.DropColumn(
                name: "NextInitiative",
                schema: "Battles",
                table: "Battles");

            migrationBuilder.AddColumn<DateTime>(
                name: "ActivationTime",
                schema: "Battles",
                table: "Battles",
                type: "timestamp with time zone",
                nullable: true,
                comment: "Время активации боя");

            migrationBuilder.AddColumn<int>(
                name: "Round",
                schema: "Battles",
                table: "Battles",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
