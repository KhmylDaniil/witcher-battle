using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wastelands.Service.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCharacterAbilities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "CreatureTemplateId",
                table: "Ability",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "CharacterId",
                table: "Ability",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ability_CharacterId",
                table: "Ability",
                column: "CharacterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ability_Character_CharacterId",
                table: "Ability",
                column: "CharacterId",
                principalTable: "Character",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ability_Character_CharacterId",
                table: "Ability");

            migrationBuilder.DropIndex(
                name: "IX_Ability_CharacterId",
                table: "Ability");

            migrationBuilder.DropColumn(
                name: "CharacterId",
                table: "Ability");

            migrationBuilder.AlterColumn<long>(
                name: "CreatureTemplateId",
                table: "Ability",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);
        }
    }
}
