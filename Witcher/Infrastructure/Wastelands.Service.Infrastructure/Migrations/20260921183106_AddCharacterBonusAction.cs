using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wastelands.Service.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCharacterBonusAction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasActedThisTurn",
                table: "BattleCharacter",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "Основное действие текущего хода уже потрачено — открыто окно дополнительного действия");

            migrationBuilder.AddColumn<bool>(
                name: "IsBonusAction",
                table: "BattleAttack",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasActedThisTurn",
                table: "BattleCharacter");

            migrationBuilder.DropColumn(
                name: "IsBonusAction",
                table: "BattleAttack");
        }
    }
}
