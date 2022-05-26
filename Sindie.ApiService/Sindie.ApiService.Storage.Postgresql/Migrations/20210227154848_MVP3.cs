using Microsoft.EntityFrameworkCore.Migrations;

namespace Sindie.ApiService.Storage.Postgresql.Migrations
{
    public partial class MVP3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Characters_UserActivateId",
                table: "Characters",
                column: "UserActivateId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_UserCharacter_UserActivateId",
                table: "Characters",
                column: "UserActivateId",
                principalTable: "UserCharacter",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Characters_UserCharacter_UserActivateId",
                table: "Characters");

            migrationBuilder.DropIndex(
                name: "IX_Characters_UserActivateId",
                table: "Characters");
        }
    }
}
