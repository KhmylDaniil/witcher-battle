using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sindie.ApiService.Storage.Postgresql.Migrations
{
    public partial class NewItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Body_Slot_SlotId",
                table: "Body");

            migrationBuilder.DropForeignKey(
                name: "FK_Characters_UserCharacter_UserActivateId",
                table: "Characters");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_Games_GameId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_Slot_SlotId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Slot_Games_GameId",
                table: "Slot");

            migrationBuilder.DropForeignKey(
                name: "FK_UserCharacter_Characters_CharacterId",
                table: "UserCharacter");

            migrationBuilder.DropForeignKey(
                name: "FK_UserCharacter_Users_UserId",
                table: "UserCharacter");

            migrationBuilder.DropForeignKey(
                name: "FK_UserGame_Games_GameId",
                table: "UserGame");

            migrationBuilder.DropForeignKey(
                name: "FK_UserGame_Users_UserId",
                table: "UserGame");

            migrationBuilder.DropIndex(
                name: "IX_Items_GameId",
                table: "Items");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserGame",
                table: "UserGame");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserCharacter",
                table: "UserCharacter");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Slot",
                table: "Slot");

            migrationBuilder.DropColumn(
                name: "GameId",
                table: "Items");

            migrationBuilder.RenameTable(
                name: "UserGame",
                newName: "UserGames");

            migrationBuilder.RenameTable(
                name: "UserCharacter",
                newName: "UserCharacters");

            migrationBuilder.RenameTable(
                name: "Slot",
                newName: "Slots");

            migrationBuilder.RenameIndex(
                name: "IX_UserGame_UserId",
                table: "UserGames",
                newName: "IX_UserGames_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserGame_GameId",
                table: "UserGames",
                newName: "IX_UserGames_GameId");

            migrationBuilder.RenameIndex(
                name: "IX_UserCharacter_UserId",
                table: "UserCharacters",
                newName: "IX_UserCharacters_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserCharacter_CharacterId",
                table: "UserCharacters",
                newName: "IX_UserCharacters_CharacterId");

            migrationBuilder.RenameIndex(
                name: "IX_Slot_GameId",
                table: "Slots",
                newName: "IX_Slots_GameId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserGames",
                table: "UserGames",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserCharacters",
                table: "UserCharacters",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Slots",
                table: "Slots",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Body_Slots_SlotId",
                table: "Body",
                column: "SlotId",
                principalTable: "Slots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_UserCharacters_UserActivateId",
                table: "Characters",
                column: "UserActivateId",
                principalTable: "UserCharacters",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Slots_SlotId",
                table: "Items",
                column: "SlotId",
                principalTable: "Slots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Slots_Games_GameId",
                table: "Slots",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserCharacters_Characters_CharacterId",
                table: "UserCharacters",
                column: "CharacterId",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserCharacters_Users_UserId",
                table: "UserCharacters",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserGames_Games_GameId",
                table: "UserGames",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserGames_Users_UserId",
                table: "UserGames",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Body_Slots_SlotId",
                table: "Body");

            migrationBuilder.DropForeignKey(
                name: "FK_Characters_UserCharacters_UserActivateId",
                table: "Characters");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_Slots_SlotId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Slots_Games_GameId",
                table: "Slots");

            migrationBuilder.DropForeignKey(
                name: "FK_UserCharacters_Characters_CharacterId",
                table: "UserCharacters");

            migrationBuilder.DropForeignKey(
                name: "FK_UserCharacters_Users_UserId",
                table: "UserCharacters");

            migrationBuilder.DropForeignKey(
                name: "FK_UserGames_Games_GameId",
                table: "UserGames");

            migrationBuilder.DropForeignKey(
                name: "FK_UserGames_Users_UserId",
                table: "UserGames");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserGames",
                table: "UserGames");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserCharacters",
                table: "UserCharacters");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Slots",
                table: "Slots");

            migrationBuilder.RenameTable(
                name: "UserGames",
                newName: "UserGame");

            migrationBuilder.RenameTable(
                name: "UserCharacters",
                newName: "UserCharacter");

            migrationBuilder.RenameTable(
                name: "Slots",
                newName: "Slot");

            migrationBuilder.RenameIndex(
                name: "IX_UserGames_UserId",
                table: "UserGame",
                newName: "IX_UserGame_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserGames_GameId",
                table: "UserGame",
                newName: "IX_UserGame_GameId");

            migrationBuilder.RenameIndex(
                name: "IX_UserCharacters_UserId",
                table: "UserCharacter",
                newName: "IX_UserCharacter_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserCharacters_CharacterId",
                table: "UserCharacter",
                newName: "IX_UserCharacter_CharacterId");

            migrationBuilder.RenameIndex(
                name: "IX_Slots_GameId",
                table: "Slot",
                newName: "IX_Slot_GameId");

            migrationBuilder.AddColumn<Guid>(
                name: "GameId",
                table: "Items",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserGame",
                table: "UserGame",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserCharacter",
                table: "UserCharacter",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Slot",
                table: "Slot",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Items_GameId",
                table: "Items",
                column: "GameId");

            migrationBuilder.AddForeignKey(
                name: "FK_Body_Slot_SlotId",
                table: "Body",
                column: "SlotId",
                principalTable: "Slot",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_UserCharacter_UserActivateId",
                table: "Characters",
                column: "UserActivateId",
                principalTable: "UserCharacter",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Games_GameId",
                table: "Items",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Slot_SlotId",
                table: "Items",
                column: "SlotId",
                principalTable: "Slot",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Slot_Games_GameId",
                table: "Slot",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserCharacter_Characters_CharacterId",
                table: "UserCharacter",
                column: "CharacterId",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserCharacter_Users_UserId",
                table: "UserCharacter",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserGame_Games_GameId",
                table: "UserGame",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserGame_Users_UserId",
                table: "UserGame",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
