using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wastelands.Service.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddStunSaveToBattleAttack : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StunSaveRoll",
                table: "BattleAttack",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "StunSaveSucceeded",
                table: "BattleAttack",
                type: "boolean",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StunSaveRoll",
                table: "BattleAttack");

            migrationBuilder.DropColumn(
                name: "StunSaveSucceeded",
                table: "BattleAttack");
        }
    }
}
