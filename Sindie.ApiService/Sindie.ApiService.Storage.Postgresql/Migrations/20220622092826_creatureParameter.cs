using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sindie.ApiService.Storage.Postgresql.Migrations
{
    public partial class creatureParameter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StatName",
                schema: "GameRules",
                table: "Parameters",
                type: "text",
                nullable: false,
                defaultValue: "",
                comment: "Название корреспондирующей характеристики");

            migrationBuilder.AddColumn<string>(
                name: "StatName",
                schema: "GameInstance",
                table: "CreatureTemplateParameters",
                type: "text",
                nullable: false,
                defaultValue: "",
                comment: "Название корреспондирующей характеристики");

            migrationBuilder.AddColumn<string>(
                name: "StatName",
                schema: "GameInstance",
                table: "CreatureParameters",
                type: "text",
                nullable: false,
                defaultValue: "",
                comment: "Название корреспондирующей характеристики");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StatName",
                schema: "GameRules",
                table: "Parameters");

            migrationBuilder.DropColumn(
                name: "StatName",
                schema: "GameInstance",
                table: "CreatureTemplateParameters");

            migrationBuilder.DropColumn(
                name: "StatName",
                schema: "GameInstance",
                table: "CreatureParameters");
        }
    }
}
