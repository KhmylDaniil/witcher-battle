using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wastelands.Service.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFumbleStunSaveOwnerAndFumbleResolvedFlags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AttackerFumbleResolved",
                table: "BattleAttack",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "DefenderFumbleResolved",
                table: "BattleAttack",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "StunSaveOwnerId",
                table: "BattleAttack",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StunSaveOwnerKind",
                table: "BattleAttack",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttackerFumbleResolved",
                table: "BattleAttack");

            migrationBuilder.DropColumn(
                name: "DefenderFumbleResolved",
                table: "BattleAttack");

            migrationBuilder.DropColumn(
                name: "StunSaveOwnerId",
                table: "BattleAttack");

            migrationBuilder.DropColumn(
                name: "StunSaveOwnerKind",
                table: "BattleAttack");
        }
    }
}
