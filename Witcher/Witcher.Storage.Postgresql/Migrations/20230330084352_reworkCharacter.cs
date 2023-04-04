using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Witcher.Storage.Postgresql.Migrations
{
    public partial class reworkCharacter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserGameCharacters_Interfaces_InterfaceId",
                schema: "Characters",
                table: "UserGameCharacters");

            migrationBuilder.DropIndex(
                name: "IX_UserGameCharacters_InterfaceId",
                schema: "Characters",
                table: "UserGameCharacters");

            migrationBuilder.DropColumn(
                name: "InterfaceId",
                schema: "Characters",
                table: "UserGameCharacters");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                schema: "Battles",
                table: "Creatures",
                type: "text",
                nullable: true,
                comment: "Описание",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "Описание шаблона");

            migrationBuilder.AlterColumn<Guid>(
                name: "BattleId",
                schema: "Battles",
                table: "Creatures",
                type: "uuid",
                nullable: true,
                comment: "Айди боя",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "Айди боя");

            migrationBuilder.AddColumn<Guid>(
                name: "GameId",
                schema: "Characters",
                table: "Characters",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "Айди игры");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_GameId",
                schema: "Characters",
                table: "Characters",
                column: "GameId");

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_Games_GameId",
                schema: "Characters",
                table: "Characters",
                column: "GameId",
                principalSchema: "BaseGame",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Characters_Games_GameId",
                schema: "Characters",
                table: "Characters");

            migrationBuilder.DropIndex(
                name: "IX_Characters_GameId",
                schema: "Characters",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "GameId",
                schema: "Characters",
                table: "Characters");

            migrationBuilder.AddColumn<Guid>(
                name: "InterfaceId",
                schema: "Characters",
                table: "UserGameCharacters",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "Айди интерфейса");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                schema: "Battles",
                table: "Creatures",
                type: "text",
                nullable: true,
                comment: "Описание шаблона",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "Описание");

            migrationBuilder.AlterColumn<Guid>(
                name: "BattleId",
                schema: "Battles",
                table: "Creatures",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "Айди боя",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "Айди боя");

            migrationBuilder.CreateIndex(
                name: "IX_UserGameCharacters_InterfaceId",
                schema: "Characters",
                table: "UserGameCharacters",
                column: "InterfaceId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserGameCharacters_Interfaces_InterfaceId",
                schema: "Characters",
                table: "UserGameCharacters",
                column: "InterfaceId",
                principalSchema: "System",
                principalTable: "Interfaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
