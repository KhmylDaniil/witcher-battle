using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sindie.ApiService.Storage.Postgresql.Migrations
{
    public partial class fixBag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CharacterTemplates_Interfaces_InterfaceId",
                schema: "GameRules",
                table: "CharacterTemplates");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_BagId",
                schema: "GameInstance",
                table: "Characters",
                column: "BagId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_Bags_BagId",
                schema: "GameInstance",
                table: "Characters",
                column: "BagId",
                principalSchema: "GameInstance",
                principalTable: "Bags",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_CharacterTemplates_Interfaces_InterfaceId",
                schema: "GameRules",
                table: "CharacterTemplates",
                column: "InterfaceId",
                principalSchema: "System",
                principalTable: "Interfaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Characters_Bags_BagId",
                schema: "GameInstance",
                table: "Characters");

            migrationBuilder.DropForeignKey(
                name: "FK_CharacterTemplates_Interfaces_InterfaceId",
                schema: "GameRules",
                table: "CharacterTemplates");

            migrationBuilder.DropIndex(
                name: "IX_Characters_BagId",
                schema: "GameInstance",
                table: "Characters");

            migrationBuilder.AddForeignKey(
                name: "FK_CharacterTemplates_Interfaces_InterfaceId",
                schema: "GameRules",
                table: "CharacterTemplates",
                column: "InterfaceId",
                principalSchema: "System",
                principalTable: "Interfaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
