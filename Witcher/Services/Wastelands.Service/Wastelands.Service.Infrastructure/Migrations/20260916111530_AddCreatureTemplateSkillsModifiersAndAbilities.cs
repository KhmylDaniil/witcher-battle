using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Wastelands.Service.Domain.Enums;

#nullable disable

namespace Wastelands.Service.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatureTemplateSkillsModifiersAndAbilities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Dictionary<DamageType, DamageTypeModifier>>(
                name: "DamageTypeModifiers",
                table: "CreatureTemplate",
                type: "jsonb",
                nullable: false,
                defaultValueSql: "'{}'::jsonb",
                comment: "DamageTypeModifiers");

            migrationBuilder.AddColumn<Dictionary<Skill, int>>(
                name: "Skills",
                table: "CreatureTemplate",
                type: "jsonb",
                nullable: false,
                defaultValueSql: "'{}'::jsonb",
                comment: "Skills");

            migrationBuilder.CreateTable(
                name: "Ability",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatureTemplateId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", nullable: false),
                    AttackSkill = table.Column<int>(type: "integer", nullable: false),
                    AttacksPerTurn = table.Column<int>(type: "integer", nullable: false),
                    DamageDiceCount = table.Column<int>(type: "integer", nullable: false),
                    DamageModifier = table.Column<int>(type: "integer", nullable: false),
                    DamageType = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ability", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ability_CreatureTemplate_CreatureTemplateId",
                        column: x => x.CreatureTemplateId,
                        principalTable: "CreatureTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbilityAppliedCondition",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AbilityId = table.Column<long>(type: "bigint", nullable: false),
                    Condition = table.Column<int>(type: "integer", nullable: false),
                    ApplyChance = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbilityAppliedCondition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbilityAppliedCondition_Ability_AbilityId",
                        column: x => x.AbilityId,
                        principalTable: "Ability",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbilityDefensiveSkill",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AbilityId = table.Column<long>(type: "bigint", nullable: false),
                    Skill = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbilityDefensiveSkill", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbilityDefensiveSkill_Ability_AbilityId",
                        column: x => x.AbilityId,
                        principalTable: "Ability",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ability_CreatureTemplateId",
                table: "Ability",
                column: "CreatureTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_AbilityAppliedCondition_AbilityId",
                table: "AbilityAppliedCondition",
                column: "AbilityId");

            migrationBuilder.CreateIndex(
                name: "IX_AbilityDefensiveSkill_AbilityId",
                table: "AbilityDefensiveSkill",
                column: "AbilityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AbilityAppliedCondition");

            migrationBuilder.DropTable(
                name: "AbilityDefensiveSkill");

            migrationBuilder.DropTable(
                name: "Ability");

            migrationBuilder.DropColumn(
                name: "DamageTypeModifiers",
                table: "CreatureTemplate");

            migrationBuilder.DropColumn(
                name: "Skills",
                table: "CreatureTemplate");
        }
    }
}
