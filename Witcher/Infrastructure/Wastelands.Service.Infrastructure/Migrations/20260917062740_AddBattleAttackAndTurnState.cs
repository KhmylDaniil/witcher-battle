using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Wastelands.Service.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBattleAttackAndTurnState : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrentInitiative",
                table: "Battle",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CurrentRound",
                table: "Battle",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "BattleAttack",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BattleId = table.Column<long>(type: "bigint", nullable: false),
                    AttackerKind = table.Column<int>(type: "integer", nullable: false),
                    AttackerId = table.Column<long>(type: "bigint", nullable: false),
                    AbilityId = table.Column<long>(type: "bigint", nullable: false),
                    AttacksAllowed = table.Column<int>(type: "integer", nullable: false),
                    AttacksUsed = table.Column<int>(type: "integer", nullable: false),
                    DefenderKind = table.Column<int>(type: "integer", nullable: false),
                    DefenderId = table.Column<long>(type: "bigint", nullable: false),
                    TargetedCreaturePartId = table.Column<long>(type: "bigint", nullable: true),
                    AttackRoll = table.Column<int>(type: "integer", nullable: true),
                    AttackerConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    DefensiveSkill = table.Column<int>(type: "integer", nullable: true),
                    DefenseRoll = table.Column<int>(type: "integer", nullable: true),
                    DefenderConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    Phase = table.Column<int>(type: "integer", nullable: false),
                    LastHitSucceeded = table.Column<bool>(type: "boolean", nullable: true),
                    ResolvedCreaturePartId = table.Column<long>(type: "bigint", nullable: true),
                    DamageRoll = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BattleAttack", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BattleAttack_Battle_BattleId",
                        column: x => x.BattleId,
                        principalTable: "Battle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BattleLogEntry",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BattleId = table.Column<long>(type: "bigint", nullable: false),
                    Message = table.Column<string>(type: "varchar(500)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BattleLogEntry", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BattleLogEntry_Battle_BattleId",
                        column: x => x.BattleId,
                        principalTable: "Battle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BattleAttack_BattleId",
                table: "BattleAttack",
                column: "BattleId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BattleLogEntry_BattleId",
                table: "BattleLogEntry",
                column: "BattleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BattleAttack");

            migrationBuilder.DropTable(
                name: "BattleLogEntry");

            migrationBuilder.DropColumn(
                name: "CurrentInitiative",
                table: "Battle");

            migrationBuilder.DropColumn(
                name: "CurrentRound",
                table: "Battle");
        }
    }
}
