using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Witcher.Storage.Postgresql.Migrations
{
    public partial class damageTypeReworking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Abilities_DamageTypes_DamageTypeId",
                schema: "GameRules",
                table: "Abilities");

            migrationBuilder.DropTable(
                name: "CreatureResistances",
                schema: "Battles");

            migrationBuilder.DropTable(
                name: "CreatureTemplateResistances",
                schema: "GameRules");

            migrationBuilder.DropTable(
                name: "CreatureTemplateVulnerables",
                schema: "GameRules");

            migrationBuilder.DropTable(
                name: "CreatureVulnerables",
                schema: "Battles");

            migrationBuilder.DropTable(
                name: "DamageTypes",
                schema: "GameRules");

            migrationBuilder.DropIndex(
                name: "IX_Abilities_DamageTypeId",
                schema: "GameRules",
                table: "Abilities");

            migrationBuilder.DropColumn(
                name: "DamageTypeId",
                schema: "GameRules",
                table: "Abilities");

            migrationBuilder.AlterColumn<string>(
                name: "Severity",
                schema: "Battles",
                table: "CritEffects",
                type: "text",
                nullable: false,
                comment: "Тяжесть критического эффекта",
                oldClrType: typeof(int),
                oldType: "integer",
                oldComment: "Тяжесть критического эффекта");

            migrationBuilder.AlterColumn<string>(
                name: "BodyPartLocation",
                schema: "Battles",
                table: "CritEffects",
                type: "text",
                nullable: false,
                comment: "Тип части тела",
                oldClrType: typeof(int),
                oldType: "integer",
                oldComment: "Тип части тела");

            migrationBuilder.AlterColumn<string>(
                name: "CreatureType",
                schema: "GameRules",
                table: "CreatureTemplates",
                type: "text",
                nullable: false,
                comment: "Тип существа",
                oldClrType: typeof(int),
                oldType: "integer",
                oldComment: "Тип существа");

            migrationBuilder.AlterColumn<string>(
                name: "CreatureType",
                schema: "Battles",
                table: "Creatures",
                type: "text",
                nullable: false,
                comment: "Тип существа",
                oldClrType: typeof(int),
                oldType: "integer",
                oldComment: "Тип существа");

            migrationBuilder.AlterColumn<string>(
                name: "BodyPartType",
                schema: "GameRules",
                table: "BodyParts",
                type: "text",
                nullable: false,
                comment: "Тип части тела",
                oldClrType: typeof(int),
                oldType: "integer",
                oldComment: "Айди типа части тела");

            migrationBuilder.AlterColumn<string>(
                name: "AttackSkill",
                schema: "GameRules",
                table: "Abilities",
                type: "text",
                nullable: false,
                comment: "Навык атаки",
                oldClrType: typeof(int),
                oldType: "integer",
                oldComment: "Навык атаки");

            migrationBuilder.AddColumn<string>(
                name: "DamageType",
                schema: "GameRules",
                table: "Abilities",
                type: "text",
                nullable: false,
                defaultValue: "",
                comment: "Тип урона");

            migrationBuilder.CreateTable(
                name: "CreatureDamageTypeModifier",
                schema: "Battles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatureId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true),
                    DamageType = table.Column<int>(type: "integer", nullable: false),
                    DamageTypeModifier = table.Column<int>(type: "integer", nullable: false),
                    PrimaryEntityid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreatureDamageTypeModifier", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CreatureDamageTypeModifier_Creatures_CreatureId",
                        column: x => x.CreatureId,
                        principalSchema: "Battles",
                        principalTable: "Creatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CreatureTemplateDamageTypeModifier",
                schema: "GameRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DamageType = table.Column<int>(type: "integer", nullable: false),
                    DamageTypeModifier = table.Column<int>(type: "integer", nullable: false),
                    PrimaryEntityid = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatureTemplateId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreatureTemplateDamageTypeModifier", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CreatureTemplateDamageTypeModifier_CreatureTemplates_Creatu~",
                        column: x => x.CreatureTemplateId,
                        principalSchema: "GameRules",
                        principalTable: "CreatureTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CreatureDamageTypeModifier_CreatureId",
                schema: "Battles",
                table: "CreatureDamageTypeModifier",
                column: "CreatureId");

            migrationBuilder.CreateIndex(
                name: "IX_CreatureTemplateDamageTypeModifier_CreatureTemplateId",
                schema: "GameRules",
                table: "CreatureTemplateDamageTypeModifier",
                column: "CreatureTemplateId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CreatureDamageTypeModifier",
                schema: "Battles");

            migrationBuilder.DropTable(
                name: "CreatureTemplateDamageTypeModifier",
                schema: "GameRules");

            migrationBuilder.DropColumn(
                name: "DamageType",
                schema: "GameRules",
                table: "Abilities");

            migrationBuilder.AlterColumn<int>(
                name: "Severity",
                schema: "Battles",
                table: "CritEffects",
                type: "integer",
                nullable: false,
                comment: "Тяжесть критического эффекта",
                oldClrType: typeof(string),
                oldType: "text",
                oldComment: "Тяжесть критического эффекта");

            migrationBuilder.AlterColumn<int>(
                name: "BodyPartLocation",
                schema: "Battles",
                table: "CritEffects",
                type: "integer",
                nullable: false,
                comment: "Тип части тела",
                oldClrType: typeof(string),
                oldType: "text",
                oldComment: "Тип части тела");

            migrationBuilder.AlterColumn<int>(
                name: "CreatureType",
                schema: "GameRules",
                table: "CreatureTemplates",
                type: "integer",
                nullable: false,
                comment: "Тип существа",
                oldClrType: typeof(string),
                oldType: "text",
                oldComment: "Тип существа");

            migrationBuilder.AlterColumn<int>(
                name: "CreatureType",
                schema: "Battles",
                table: "Creatures",
                type: "integer",
                nullable: false,
                comment: "Тип существа",
                oldClrType: typeof(string),
                oldType: "text",
                oldComment: "Тип существа");

            migrationBuilder.AlterColumn<int>(
                name: "BodyPartType",
                schema: "GameRules",
                table: "BodyParts",
                type: "integer",
                nullable: false,
                comment: "Айди типа части тела",
                oldClrType: typeof(string),
                oldType: "text",
                oldComment: "Тип части тела");

            migrationBuilder.AlterColumn<int>(
                name: "AttackSkill",
                schema: "GameRules",
                table: "Abilities",
                type: "integer",
                nullable: false,
                comment: "Навык атаки",
                oldClrType: typeof(string),
                oldType: "text",
                oldComment: "Навык атаки");

            migrationBuilder.AddColumn<Guid>(
                name: "DamageTypeId",
                schema: "GameRules",
                table: "Abilities",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "DamageTypes",
                schema: "GameRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Название"),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DamageTypes", x => x.Id);
                },
                comment: "Типы урона");

            migrationBuilder.CreateTable(
                name: "CreatureResistances",
                schema: "Battles",
                columns: table => new
                {
                    ResistancesId = table.Column<Guid>(type: "uuid", nullable: false),
                    ResistantCreaturesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreatureResistances", x => new { x.ResistancesId, x.ResistantCreaturesId });
                    table.ForeignKey(
                        name: "FK_CreatureResistances_Creatures_ResistantCreaturesId",
                        column: x => x.ResistantCreaturesId,
                        principalSchema: "Battles",
                        principalTable: "Creatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CreatureResistances_DamageTypes_ResistancesId",
                        column: x => x.ResistancesId,
                        principalSchema: "GameRules",
                        principalTable: "DamageTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CreatureTemplateResistances",
                schema: "GameRules",
                columns: table => new
                {
                    ResistancesId = table.Column<Guid>(type: "uuid", nullable: false),
                    ResistantCreatureTemplatesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreatureTemplateResistances", x => new { x.ResistancesId, x.ResistantCreatureTemplatesId });
                    table.ForeignKey(
                        name: "FK_CreatureTemplateResistances_CreatureTemplates_ResistantCrea~",
                        column: x => x.ResistantCreatureTemplatesId,
                        principalSchema: "GameRules",
                        principalTable: "CreatureTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CreatureTemplateResistances_DamageTypes_ResistancesId",
                        column: x => x.ResistancesId,
                        principalSchema: "GameRules",
                        principalTable: "DamageTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CreatureTemplateVulnerables",
                schema: "GameRules",
                columns: table => new
                {
                    VulnerableCreatureTemplatesId = table.Column<Guid>(type: "uuid", nullable: false),
                    VulnerablesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreatureTemplateVulnerables", x => new { x.VulnerableCreatureTemplatesId, x.VulnerablesId });
                    table.ForeignKey(
                        name: "FK_CreatureTemplateVulnerables_CreatureTemplates_VulnerableCre~",
                        column: x => x.VulnerableCreatureTemplatesId,
                        principalSchema: "GameRules",
                        principalTable: "CreatureTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CreatureTemplateVulnerables_DamageTypes_VulnerablesId",
                        column: x => x.VulnerablesId,
                        principalSchema: "GameRules",
                        principalTable: "DamageTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CreatureVulnerables",
                schema: "Battles",
                columns: table => new
                {
                    VulnerableCreaturesId = table.Column<Guid>(type: "uuid", nullable: false),
                    VulnerablesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreatureVulnerables", x => new { x.VulnerableCreaturesId, x.VulnerablesId });
                    table.ForeignKey(
                        name: "FK_CreatureVulnerables_Creatures_VulnerableCreaturesId",
                        column: x => x.VulnerableCreaturesId,
                        principalSchema: "Battles",
                        principalTable: "Creatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CreatureVulnerables_DamageTypes_VulnerablesId",
                        column: x => x.VulnerablesId,
                        principalSchema: "GameRules",
                        principalTable: "DamageTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "GameRules",
                table: "DamageTypes",
                columns: new[] { "Id", "CreatedByUserId", "CreatedOn", "ModifiedByUserId", "ModifiedOn", "Name", "RoleCreatedUser", "RoleModifiedUser" },
                values: new object[,]
                {
                    { new Guid("42e5a598-f6e6-4ccd-8de3-d0e0963d1a33"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Slashing", "Default", "Default" },
                    { new Guid("43e5a598-f6e6-4ccd-8de3-d0e0963d1a33"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Piercing", "Default", "Default" },
                    { new Guid("44e5a598-f6e6-4ccd-8de3-d0e0963d1a33"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Bludgeoning", "Default", "Default" },
                    { new Guid("45e5a598-f6e6-4ccd-8de3-d0e0963d1a33"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Elemental", "Default", "Default" },
                    { new Guid("46e5a598-f6e6-4ccd-8de3-d0e0963d1a33"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Fire", "Default", "Default" },
                    { new Guid("47e5a598-f6e6-4ccd-8de3-d0e0963d1a33"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Silver", "Default", "Default" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Abilities_DamageTypeId",
                schema: "GameRules",
                table: "Abilities",
                column: "DamageTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CreatureResistances_ResistantCreaturesId",
                schema: "Battles",
                table: "CreatureResistances",
                column: "ResistantCreaturesId");

            migrationBuilder.CreateIndex(
                name: "IX_CreatureTemplateResistances_ResistantCreatureTemplatesId",
                schema: "GameRules",
                table: "CreatureTemplateResistances",
                column: "ResistantCreatureTemplatesId");

            migrationBuilder.CreateIndex(
                name: "IX_CreatureTemplateVulnerables_VulnerablesId",
                schema: "GameRules",
                table: "CreatureTemplateVulnerables",
                column: "VulnerablesId");

            migrationBuilder.CreateIndex(
                name: "IX_CreatureVulnerables_VulnerablesId",
                schema: "Battles",
                table: "CreatureVulnerables",
                column: "VulnerablesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Abilities_DamageTypes_DamageTypeId",
                schema: "GameRules",
                table: "Abilities",
                column: "DamageTypeId",
                principalSchema: "GameRules",
                principalTable: "DamageTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
