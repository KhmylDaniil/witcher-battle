using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sindie.ApiService.Storage.Postgresql.Migrations
{
    public partial class feature52_ConfigurationFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameImgFiles_ImgFiles_TextFileId",
                schema: "BaseGame",
                table: "GameImgFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_ImgFiles_AvatarId",
                schema: "BaseGame",
                table: "Games");

            migrationBuilder.RenameColumn(
                name: "TextFileId",
                schema: "BaseGame",
                table: "GameImgFiles",
                newName: "ImgFileId");

            migrationBuilder.RenameIndex(
                name: "IX_GameImgFiles_TextFileId",
                schema: "BaseGame",
                table: "GameImgFiles",
                newName: "IX_GameImgFiles_ImgFileId");

            migrationBuilder.AlterColumn<Guid>(
                name: "AvatarId",
                schema: "BaseGame",
                table: "Games",
                type: "uuid",
                nullable: true,
                comment: "Айди аватара игры",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "Айди аватара игры");

            migrationBuilder.AddForeignKey(
                name: "FK_GameImgFiles_ImgFiles_ImgFileId",
                schema: "BaseGame",
                table: "GameImgFiles",
                column: "ImgFileId",
                principalSchema: "System",
                principalTable: "ImgFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_ImgFiles_AvatarId",
                schema: "BaseGame",
                table: "Games",
                column: "AvatarId",
                principalSchema: "System",
                principalTable: "ImgFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameImgFiles_ImgFiles_ImgFileId",
                schema: "BaseGame",
                table: "GameImgFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_ImgFiles_AvatarId",
                schema: "BaseGame",
                table: "Games");

            migrationBuilder.RenameColumn(
                name: "ImgFileId",
                schema: "BaseGame",
                table: "GameImgFiles",
                newName: "TextFileId");

            migrationBuilder.RenameIndex(
                name: "IX_GameImgFiles_ImgFileId",
                schema: "BaseGame",
                table: "GameImgFiles",
                newName: "IX_GameImgFiles_TextFileId");

            migrationBuilder.AlterColumn<Guid>(
                name: "AvatarId",
                schema: "BaseGame",
                table: "Games",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "Айди аватара игры",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "Айди аватара игры");

            migrationBuilder.AddForeignKey(
                name: "FK_GameImgFiles_ImgFiles_TextFileId",
                schema: "BaseGame",
                table: "GameImgFiles",
                column: "TextFileId",
                principalSchema: "System",
                principalTable: "ImgFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_ImgFiles_AvatarId",
                schema: "BaseGame",
                table: "Games",
                column: "AvatarId",
                principalSchema: "System",
                principalTable: "ImgFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
