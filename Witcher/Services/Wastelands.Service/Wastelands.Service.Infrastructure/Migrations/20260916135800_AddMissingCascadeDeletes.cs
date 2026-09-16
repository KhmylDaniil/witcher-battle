using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wastelands.Service.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMissingCascadeDeletes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_UserGame_GameId",
                table: "UserGame",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGame_UserId",
                table: "UserGame",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_GameJoinRequest_GameId",
                table: "GameJoinRequest",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_GameJoinRequest_UserId",
                table: "GameJoinRequest",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Game_CreatedByUserId",
                table: "Game",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CreatureTemplate_BodyTemplateId",
                table: "CreatureTemplate",
                column: "BodyTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_CreatureTemplate_GameId",
                table: "CreatureTemplate",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_BodyTemplate_GameId",
                table: "BodyTemplate",
                column: "GameId");

            migrationBuilder.AddForeignKey(
                name: "FK_BodyTemplate_Game_GameId",
                table: "BodyTemplate",
                column: "GameId",
                principalTable: "Game",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CreatureTemplate_BodyTemplate_BodyTemplateId",
                table: "CreatureTemplate",
                column: "BodyTemplateId",
                principalTable: "BodyTemplate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CreatureTemplate_Game_GameId",
                table: "CreatureTemplate",
                column: "GameId",
                principalTable: "Game",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Game_User_CreatedByUserId",
                table: "Game",
                column: "CreatedByUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameJoinRequest_Game_GameId",
                table: "GameJoinRequest",
                column: "GameId",
                principalTable: "Game",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameJoinRequest_User_UserId",
                table: "GameJoinRequest",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserGame_Game_GameId",
                table: "UserGame",
                column: "GameId",
                principalTable: "Game",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserGame_User_UserId",
                table: "UserGame",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BodyTemplate_Game_GameId",
                table: "BodyTemplate");

            migrationBuilder.DropForeignKey(
                name: "FK_CreatureTemplate_BodyTemplate_BodyTemplateId",
                table: "CreatureTemplate");

            migrationBuilder.DropForeignKey(
                name: "FK_CreatureTemplate_Game_GameId",
                table: "CreatureTemplate");

            migrationBuilder.DropForeignKey(
                name: "FK_Game_User_CreatedByUserId",
                table: "Game");

            migrationBuilder.DropForeignKey(
                name: "FK_GameJoinRequest_Game_GameId",
                table: "GameJoinRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_GameJoinRequest_User_UserId",
                table: "GameJoinRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_UserGame_Game_GameId",
                table: "UserGame");

            migrationBuilder.DropForeignKey(
                name: "FK_UserGame_User_UserId",
                table: "UserGame");

            migrationBuilder.DropIndex(
                name: "IX_UserGame_GameId",
                table: "UserGame");

            migrationBuilder.DropIndex(
                name: "IX_UserGame_UserId",
                table: "UserGame");

            migrationBuilder.DropIndex(
                name: "IX_GameJoinRequest_GameId",
                table: "GameJoinRequest");

            migrationBuilder.DropIndex(
                name: "IX_GameJoinRequest_UserId",
                table: "GameJoinRequest");

            migrationBuilder.DropIndex(
                name: "IX_Game_CreatedByUserId",
                table: "Game");

            migrationBuilder.DropIndex(
                name: "IX_CreatureTemplate_BodyTemplateId",
                table: "CreatureTemplate");

            migrationBuilder.DropIndex(
                name: "IX_CreatureTemplate_GameId",
                table: "CreatureTemplate");

            migrationBuilder.DropIndex(
                name: "IX_BodyTemplate_GameId",
                table: "BodyTemplate");
        }
    }
}
