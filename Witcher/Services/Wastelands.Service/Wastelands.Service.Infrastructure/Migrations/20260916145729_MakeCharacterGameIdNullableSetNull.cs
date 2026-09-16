using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wastelands.Service.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MakeCharacterGameIdNullableSetNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Character_Game_GameId",
                table: "Character");

            migrationBuilder.AlterColumn<long>(
                name: "GameId",
                table: "Character",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_Character_Game_GameId",
                table: "Character",
                column: "GameId",
                principalTable: "Game",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Character_Game_GameId",
                table: "Character");

            migrationBuilder.AlterColumn<long>(
                name: "GameId",
                table: "Character",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Character_Game_GameId",
                table: "Character",
                column: "GameId",
                principalTable: "Game",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
