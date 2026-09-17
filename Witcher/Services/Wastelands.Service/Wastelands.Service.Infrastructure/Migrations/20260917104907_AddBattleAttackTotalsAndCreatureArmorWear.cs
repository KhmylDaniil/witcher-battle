using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wastelands.Service.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBattleAttackTotalsAndCreatureArmorWear : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Dictionary<long, int>>(
                name: "ArmorReductionByPartId",
                table: "Creature",
                type: "jsonb",
                nullable: false,
                defaultValueSql: "'{}'",
                comment: "Износ брони по частям тела, накопленный этим существом в этом бою");

            migrationBuilder.AddColumn<int>(
                name: "AttackTotal",
                table: "BattleAttack",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DefenseTotal",
                table: "BattleAttack",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArmorReductionByPartId",
                table: "Creature");

            migrationBuilder.DropColumn(
                name: "AttackTotal",
                table: "BattleAttack");

            migrationBuilder.DropColumn(
                name: "DefenseTotal",
                table: "BattleAttack");
        }
    }
}
