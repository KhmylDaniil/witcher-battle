using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sindie.ApiService.Storage.Postgresql.Migrations
{
    public partial class creatureParameter1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "StatName",
                schema: "GameRules",
                table: "Parameters",
                type: "text",
                nullable: true,
                comment: "Название корреспондирующей характеристики",
                oldClrType: typeof(string),
                oldType: "text",
                oldComment: "Название корреспондирующей характеристики");

            migrationBuilder.AlterColumn<string>(
                name: "StatName",
                schema: "GameInstance",
                table: "CreatureTemplateParameters",
                type: "text",
                nullable: true,
                comment: "Название корреспондирующей характеристики",
                oldClrType: typeof(string),
                oldType: "text",
                oldComment: "Название корреспондирующей характеристики");

            migrationBuilder.AlterColumn<string>(
                name: "StatName",
                schema: "GameInstance",
                table: "CreatureParameters",
                type: "text",
                nullable: true,
                comment: "Название корреспондирующей характеристики",
                oldClrType: typeof(string),
                oldType: "text",
                oldComment: "Название корреспондирующей характеристики");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "StatName",
                schema: "GameRules",
                table: "Parameters",
                type: "text",
                nullable: false,
                defaultValue: "",
                comment: "Название корреспондирующей характеристики",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "Название корреспондирующей характеристики");

            migrationBuilder.AlterColumn<string>(
                name: "StatName",
                schema: "GameInstance",
                table: "CreatureTemplateParameters",
                type: "text",
                nullable: false,
                defaultValue: "",
                comment: "Название корреспондирующей характеристики",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "Название корреспондирующей характеристики");

            migrationBuilder.AlterColumn<string>(
                name: "StatName",
                schema: "GameInstance",
                table: "CreatureParameters",
                type: "text",
                nullable: false,
                defaultValue: "",
                comment: "Название корреспондирующей характеристики",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "Название корреспондирующей характеристики");
        }
    }
}
