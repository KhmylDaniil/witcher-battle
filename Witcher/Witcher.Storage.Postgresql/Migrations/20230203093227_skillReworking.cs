using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Witcher.Storage.Postgresql.Migrations
{
    public partial class skillReworking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Abilities_Skills_AttackSkillId",
                schema: "GameRules",
                table: "Abilities");

            migrationBuilder.DropForeignKey(
                name: "FK_CreatureSkills_Skills_SkillId",
                schema: "Battles",
                table: "CreatureSkills");

            migrationBuilder.DropForeignKey(
                name: "FK_CreatureTemplateSkills_Skills_SkillId",
                schema: "GameRules",
                table: "CreatureTemplateSkills");

            migrationBuilder.DropTable(
                name: "DefensiveSkills",
                schema: "GameRules");

            migrationBuilder.DropTable(
                name: "Skills",
                schema: "GameRules");

            migrationBuilder.DropIndex(
                name: "IX_CreatureTemplateSkills_SkillId",
                schema: "GameRules",
                table: "CreatureTemplateSkills");

            migrationBuilder.DropIndex(
                name: "IX_CreatureSkills_SkillId",
                schema: "Battles",
                table: "CreatureSkills");

            migrationBuilder.DropIndex(
                name: "IX_Abilities_AttackSkillId",
                schema: "GameRules",
                table: "Abilities");

            migrationBuilder.DropColumn(
                name: "SkillId",
                schema: "GameRules",
                table: "CreatureTemplateSkills");

            migrationBuilder.DropColumn(
                name: "StatName",
                schema: "GameRules",
                table: "CreatureTemplateSkills");

            migrationBuilder.DropColumn(
                name: "SkillId",
                schema: "Battles",
                table: "CreatureSkills");

            migrationBuilder.DropColumn(
                name: "StatName",
                schema: "Battles",
                table: "CreatureSkills");

            migrationBuilder.DropColumn(
                name: "AttackSkillId",
                schema: "GameRules",
                table: "Abilities");

            migrationBuilder.AddColumn<string>(
                name: "Skill",
                schema: "GameRules",
                table: "CreatureTemplateSkills",
                type: "text",
                nullable: false,
                defaultValue: "",
                comment: "Навык");

            migrationBuilder.AddColumn<string>(
                name: "Skill",
                schema: "Battles",
                table: "CreatureSkills",
                type: "text",
                nullable: false,
                defaultValue: "",
                comment: "Навык");

            migrationBuilder.AddColumn<int>(
                name: "AttackSkill",
                schema: "GameRules",
                table: "Abilities",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "Навык атаки");

            migrationBuilder.CreateTable(
                name: "DefensiveSkill",
                schema: "GameRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AbilityId = table.Column<Guid>(type: "uuid", nullable: false),
                    Skill = table.Column<int>(type: "integer", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DefensiveSkill", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DefensiveSkill_Abilities_AbilityId",
                        column: x => x.AbilityId,
                        principalSchema: "GameRules",
                        principalTable: "Abilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DefensiveSkill_AbilityId",
                schema: "GameRules",
                table: "DefensiveSkill",
                column: "AbilityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DefensiveSkill",
                schema: "GameRules");

            migrationBuilder.DropColumn(
                name: "Skill",
                schema: "GameRules",
                table: "CreatureTemplateSkills");

            migrationBuilder.DropColumn(
                name: "Skill",
                schema: "Battles",
                table: "CreatureSkills");

            migrationBuilder.DropColumn(
                name: "AttackSkill",
                schema: "GameRules",
                table: "Abilities");

            migrationBuilder.AddColumn<Guid>(
                name: "SkillId",
                schema: "GameRules",
                table: "CreatureTemplateSkills",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "Айди навыка");

            migrationBuilder.AddColumn<string>(
                name: "StatName",
                schema: "GameRules",
                table: "CreatureTemplateSkills",
                type: "text",
                nullable: false,
                defaultValue: "",
                comment: "Название корреспондирующей характеристики");

            migrationBuilder.AddColumn<Guid>(
                name: "SkillId",
                schema: "Battles",
                table: "CreatureSkills",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "Айди навыка");

            migrationBuilder.AddColumn<string>(
                name: "StatName",
                schema: "Battles",
                table: "CreatureSkills",
                type: "text",
                nullable: false,
                defaultValue: "",
                comment: "Название корреспондирующей характеристики");

            migrationBuilder.AddColumn<Guid>(
                name: "AttackSkillId",
                schema: "GameRules",
                table: "Abilities",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "Навык атаки");

            migrationBuilder.CreateTable(
                name: "Skills",
                schema: "GameRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Название навыка"),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true),
                    StatName = table.Column<string>(type: "text", nullable: false, comment: "Название корреспондирующей характеристики")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.Id);
                },
                comment: "Навыки");

            migrationBuilder.CreateTable(
                name: "DefensiveSkills",
                schema: "GameRules",
                columns: table => new
                {
                    AbilitiesForDefenseId = table.Column<Guid>(type: "uuid", nullable: false),
                    DefensiveSkillsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DefensiveSkills", x => new { x.AbilitiesForDefenseId, x.DefensiveSkillsId });
                    table.ForeignKey(
                        name: "FK_DefensiveSkills_Abilities_AbilitiesForDefenseId",
                        column: x => x.AbilitiesForDefenseId,
                        principalSchema: "GameRules",
                        principalTable: "Abilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DefensiveSkills_Skills_DefensiveSkillsId",
                        column: x => x.DefensiveSkillsId,
                        principalSchema: "GameRules",
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "GameRules",
                table: "Skills",
                columns: new[] { "Id", "CreatedByUserId", "CreatedOn", "ModifiedByUserId", "ModifiedOn", "Name", "RoleCreatedUser", "RoleModifiedUser", "StatName" },
                values: new object[,]
                {
                    { new Guid("32ee830e-7bee-4924-9ddf-1070ceffecdd"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Education", "Default", "Default", "Int" },
                    { new Guid("4fcbd3d6-fde0-47c1-899d-a8c82c291751"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "CommonLanguage", "Default", "Default", "Int" },
                    { new Guid("754ef5e9-8960-4c38-a1be-a3c43c92b1cd"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Deduction", "Default", "Default", "Int" },
                    { new Guid("c5f00eea-10d5-426e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Awareness", "Default", "Default", "Int" },
                    { new Guid("c5f01eea-10d5-426e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Business", "Default", "Default", "Int" },
                    { new Guid("c5f03eea-10d5-426e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "ElderLanguage", "Default", "Default", "Int" },
                    { new Guid("c5f04eea-10d5-426e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "DwarfenLanguage", "Default", "Default", "Int" },
                    { new Guid("c5f05eea-10d5-426e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "MonsterLore", "Default", "Default", "Int" },
                    { new Guid("c5f06eea-10d5-426e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "SocialEtiquette", "Default", "Default", "Int" },
                    { new Guid("c5f07eea-10d5-426e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Streetwise", "Default", "Default", "Int" },
                    { new Guid("c5f08eea-10d5-426e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Tactics", "Default", "Default", "Int" },
                    { new Guid("c5f09eea-10d5-426e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Teaching", "Default", "Default", "Int" },
                    { new Guid("c5f10eea-10d5-426e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "WildernessSurvival", "Default", "Default", "Int" },
                    { new Guid("c5f11eea-10d5-428e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Riding", "Default", "Default", "Ref" },
                    { new Guid("c5f12eea-10d5-428e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Sailing", "Default", "Default", "Ref" },
                    { new Guid("c5f13eea-10d5-726e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "SleightOfHand", "Default", "Default", "Dex" },
                    { new Guid("c5f14eea-10d5-826e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Stealth", "Default", "Default", "Dex" },
                    { new Guid("c5f15eea-10d5-826e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Charisma", "Default", "Default", "Emp" },
                    { new Guid("c5f16eea-10d5-826e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Deceit", "Default", "Default", "Emp" },
                    { new Guid("c5f17eea-10d5-826e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "FineArts", "Default", "Default", "Emp" },
                    { new Guid("c5f18eea-10d5-826e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Gambling", "Default", "Default", "Emp" },
                    { new Guid("c5f19eea-10d5-826e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "GroomingAndStyle", "Default", "Default", "Emp" },
                    { new Guid("c5f20eea-10d5-826e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "HumanPerception", "Default", "Default", "Emp" },
                    { new Guid("c5f21eea-10d5-826e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Leadership", "Default", "Default", "Emp" },
                    { new Guid("c5f22eea-10d5-826e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Persuasion", "Default", "Default", "Emp" },
                    { new Guid("c5f23eea-10d5-826e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Perfomance", "Default", "Default", "Emp" },
                    { new Guid("c5f24eea-10d5-826e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Seduction", "Default", "Default", "Emp" },
                    { new Guid("c5f25eea-10d5-026e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Alchemy", "Default", "Default", "Cra" },
                    { new Guid("c5f26eea-10d5-026e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Crafting", "Default", "Default", "Cra" },
                    { new Guid("c5f27eea-10d5-026e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Diguise", "Default", "Default", "Cra" },
                    { new Guid("c5f28eea-10d5-026e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Forgery", "Default", "Default", "Cra" },
                    { new Guid("c5f29eea-10d5-026e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "PickLock", "Default", "Default", "Cra" },
                    { new Guid("c5f30eea-10d5-026e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "TrapCrafting", "Default", "Default", "Cra" },
                    { new Guid("c5f31eea-10d5-026e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Courage", "Default", "Default", "Will" },
                    { new Guid("c5f32eea-10d5-026e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "HexWeaving", "Default", "Default", "Will" },
                    { new Guid("c5f33eea-10d5-026e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Intimidation", "Default", "Default", "Will" },
                    { new Guid("c5f34eea-10d5-026e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "RitualCrafting", "Default", "Default", "Will" },
                    { new Guid("c5f99eea-10d5-026e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "FirstAid", "Default", "Default", "Cra" },
                    { new Guid("c5f99eea-10d5-420e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Staff/Spear", "Default", "Default", "Ref" },
                    { new Guid("c5f99eea-10d5-426e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Brawling", "Default", "Default", "Ref" },
                    { new Guid("c5f99eea-10d5-427e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Dodge", "Default", "Default", "Ref" },
                    { new Guid("c5f99eea-10d5-428e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Melee", "Default", "Default", "Ref" },
                    { new Guid("c5f99eea-10d5-429e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "SmallBlades", "Default", "Default", "Ref" },
                    { new Guid("c5f99eea-10d5-436e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "SpellCasting", "Default", "Default", "Will" },
                    { new Guid("c5f99eea-10d5-446e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "ResistMagic", "Default", "Default", "Will" },
                    { new Guid("c5f99eea-10d5-456e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "ResistCoercion", "Default", "Default", "Will" },
                    { new Guid("c5f99eea-10d5-466e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Needling", "Default", "Default", "Emp" },
                    { new Guid("c5f99eea-10d5-476e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "EyeGouge", "Default", "Default", "Dex" },
                    { new Guid("c5f99eea-10d5-486e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "BleedingWound", "Default", "Default", "Int" },
                    { new Guid("c5f99eea-10d5-496e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "HealingHands", "Default", "Default", "Cra" },
                    { new Guid("c5f99eea-10d5-506e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Physique", "Default", "Default", "Body" },
                    { new Guid("c5f99eea-10d5-526e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Swordsmanship", "Default", "Default", "Ref" },
                    { new Guid("c5f99eea-10d5-626e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Archery", "Default", "Default", "Dex" },
                    { new Guid("c5f99eea-10d5-726e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Athletics", "Default", "Default", "Dex" },
                    { new Guid("c5f99eea-10d5-826e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Crossbow", "Default", "Default", "Dex" },
                    { new Guid("c5f99eea-10d5-926e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Endurance", "Default", "Default", "Body" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CreatureTemplateSkills_SkillId",
                schema: "GameRules",
                table: "CreatureTemplateSkills",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_CreatureSkills_SkillId",
                schema: "Battles",
                table: "CreatureSkills",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_Abilities_AttackSkillId",
                schema: "GameRules",
                table: "Abilities",
                column: "AttackSkillId");

            migrationBuilder.CreateIndex(
                name: "IX_DefensiveSkills_DefensiveSkillsId",
                schema: "GameRules",
                table: "DefensiveSkills",
                column: "DefensiveSkillsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Abilities_Skills_AttackSkillId",
                schema: "GameRules",
                table: "Abilities",
                column: "AttackSkillId",
                principalSchema: "GameRules",
                principalTable: "Skills",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CreatureSkills_Skills_SkillId",
                schema: "Battles",
                table: "CreatureSkills",
                column: "SkillId",
                principalSchema: "GameRules",
                principalTable: "Skills",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CreatureTemplateSkills_Skills_SkillId",
                schema: "GameRules",
                table: "CreatureTemplateSkills",
                column: "SkillId",
                principalSchema: "GameRules",
                principalTable: "Skills",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
