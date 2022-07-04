using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sindie.ApiService.Storage.Postgresql.Migrations
{
    public partial class AbilityRework : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Abilities_CreatureTemplates_CreatureTemplateId",
                schema: "GameRules",
                table: "Abilities");

            migrationBuilder.DropForeignKey(
                name: "FK_Conditions_Games_GameId",
                schema: "GameRules",
                table: "Conditions");

            migrationBuilder.DropIndex(
                name: "IX_Conditions_GameId",
                schema: "GameRules",
                table: "Conditions");

            migrationBuilder.DropIndex(
                name: "IX_Abilities_CreatureTemplateId",
                schema: "GameRules",
                table: "Abilities");

            migrationBuilder.DropColumn(
                name: "GameId",
                schema: "GameRules",
                table: "Conditions");

            migrationBuilder.DropColumn(
                name: "CreatureTemplateId",
                schema: "GameRules",
                table: "Abilities");

            migrationBuilder.AddColumn<Guid>(
                name: "AbilityId",
                schema: "GameRules",
                table: "Parameters",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "GameId",
                schema: "GameRules",
                table: "Abilities",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "Айди игры");

            migrationBuilder.CreateTable(
                name: "CreatureTemplateAbilities",
                schema: "GameRules",
                columns: table => new
                {
                    AbilitiesId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatureTemplatesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreatureTemplateAbilities", x => new { x.AbilitiesId, x.CreatureTemplatesId });
                    table.ForeignKey(
                        name: "FK_CreatureTemplateAbilities_Abilities_AbilitiesId",
                        column: x => x.AbilitiesId,
                        principalSchema: "GameRules",
                        principalTable: "Abilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CreatureTemplateAbilities_CreatureTemplates_CreatureTemplat~",
                        column: x => x.CreatureTemplatesId,
                        principalSchema: "GameRules",
                        principalTable: "CreatureTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DamageTypes",
                schema: "GameRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Название"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DamageTypes", x => x.Id);
                },
                comment: "Типы урона");

            migrationBuilder.CreateTable(
                name: "AbilityDamageTypes",
                schema: "GameRules",
                columns: table => new
                {
                    AbilitiesId = table.Column<Guid>(type: "uuid", nullable: false),
                    DamageTypesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbilityDamageTypes", x => new { x.AbilitiesId, x.DamageTypesId });
                    table.ForeignKey(
                        name: "FK_AbilityDamageTypes_Abilities_AbilitiesId",
                        column: x => x.AbilitiesId,
                        principalSchema: "GameRules",
                        principalTable: "Abilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AbilityDamageTypes_DamageTypes_DamageTypesId",
                        column: x => x.DamageTypesId,
                        principalSchema: "GameRules",
                        principalTable: "DamageTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CreatureImmunities",
                schema: "GameInstance",
                columns: table => new
                {
                    ImmuneCreaturesId = table.Column<Guid>(type: "uuid", nullable: false),
                    ImmunitiesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreatureImmunities", x => new { x.ImmuneCreaturesId, x.ImmunitiesId });
                    table.ForeignKey(
                        name: "FK_CreatureImmunities_Creatures_ImmuneCreaturesId",
                        column: x => x.ImmuneCreaturesId,
                        principalSchema: "GameInstance",
                        principalTable: "Creatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CreatureImmunities_DamageTypes_ImmunitiesId",
                        column: x => x.ImmunitiesId,
                        principalSchema: "GameRules",
                        principalTable: "DamageTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CreatureResistances",
                schema: "GameInstance",
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
                        principalSchema: "GameInstance",
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
                name: "CreatureTemplateImmunities",
                schema: "GameRules",
                columns: table => new
                {
                    ImmuneCreatureCreatureTemplatesId = table.Column<Guid>(type: "uuid", nullable: false),
                    ImmunitiesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreatureTemplateImmunities", x => new { x.ImmuneCreatureCreatureTemplatesId, x.ImmunitiesId });
                    table.ForeignKey(
                        name: "FK_CreatureTemplateImmunities_CreatureTemplates_ImmuneCreature~",
                        column: x => x.ImmuneCreatureCreatureTemplatesId,
                        principalSchema: "GameRules",
                        principalTable: "CreatureTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CreatureTemplateImmunities_DamageTypes_ImmunitiesId",
                        column: x => x.ImmunitiesId,
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
                schema: "GameInstance",
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
                        principalSchema: "GameInstance",
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
                table: "Conditions",
                columns: new[] { "Id", "CreatedByUserId", "CreatedOn", "ModifiedByUserId", "ModifiedOn", "Name", "RoleCreatedUser", "RoleModifiedUser" },
                values: new object[,]
                {
                    { new Guid("8894e0d0-3147-4791-9053-9667cbe127d7"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Poison", "Default", "Default" },
                    { new Guid("9994e0d0-3147-4791-9053-9667cbe127d7"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Bleed", "Default", "Default" }
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
                name: "IX_Parameters_AbilityId",
                schema: "GameRules",
                table: "Parameters",
                column: "AbilityId");

            migrationBuilder.CreateIndex(
                name: "IX_Abilities_GameId",
                schema: "GameRules",
                table: "Abilities",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_AbilityDamageTypes_DamageTypesId",
                schema: "GameRules",
                table: "AbilityDamageTypes",
                column: "DamageTypesId");

            migrationBuilder.CreateIndex(
                name: "IX_CreatureImmunities_ImmunitiesId",
                schema: "GameInstance",
                table: "CreatureImmunities",
                column: "ImmunitiesId");

            migrationBuilder.CreateIndex(
                name: "IX_CreatureResistances_ResistantCreaturesId",
                schema: "GameInstance",
                table: "CreatureResistances",
                column: "ResistantCreaturesId");

            migrationBuilder.CreateIndex(
                name: "IX_CreatureTemplateAbilities_CreatureTemplatesId",
                schema: "GameRules",
                table: "CreatureTemplateAbilities",
                column: "CreatureTemplatesId");

            migrationBuilder.CreateIndex(
                name: "IX_CreatureTemplateImmunities_ImmunitiesId",
                schema: "GameRules",
                table: "CreatureTemplateImmunities",
                column: "ImmunitiesId");

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
                schema: "GameInstance",
                table: "CreatureVulnerables",
                column: "VulnerablesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Abilities_Games_GameId",
                schema: "GameRules",
                table: "Abilities",
                column: "GameId",
                principalSchema: "BaseGame",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Parameters_Abilities_AbilityId",
                schema: "GameRules",
                table: "Parameters",
                column: "AbilityId",
                principalSchema: "GameRules",
                principalTable: "Abilities",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Abilities_Games_GameId",
                schema: "GameRules",
                table: "Abilities");

            migrationBuilder.DropForeignKey(
                name: "FK_Parameters_Abilities_AbilityId",
                schema: "GameRules",
                table: "Parameters");

            migrationBuilder.DropTable(
                name: "AbilityDamageTypes",
                schema: "GameRules");

            migrationBuilder.DropTable(
                name: "CreatureImmunities",
                schema: "GameInstance");

            migrationBuilder.DropTable(
                name: "CreatureResistances",
                schema: "GameInstance");

            migrationBuilder.DropTable(
                name: "CreatureTemplateAbilities",
                schema: "GameRules");

            migrationBuilder.DropTable(
                name: "CreatureTemplateImmunities",
                schema: "GameRules");

            migrationBuilder.DropTable(
                name: "CreatureTemplateResistances",
                schema: "GameRules");

            migrationBuilder.DropTable(
                name: "CreatureTemplateVulnerables",
                schema: "GameRules");

            migrationBuilder.DropTable(
                name: "CreatureVulnerables",
                schema: "GameInstance");

            migrationBuilder.DropTable(
                name: "DamageTypes",
                schema: "GameRules");

            migrationBuilder.DropIndex(
                name: "IX_Parameters_AbilityId",
                schema: "GameRules",
                table: "Parameters");

            migrationBuilder.DropIndex(
                name: "IX_Abilities_GameId",
                schema: "GameRules",
                table: "Abilities");

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Conditions",
                keyColumn: "Id",
                keyValue: new Guid("8894e0d0-3147-4791-9053-9667cbe127d7"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Conditions",
                keyColumn: "Id",
                keyValue: new Guid("9994e0d0-3147-4791-9053-9667cbe127d7"));

            migrationBuilder.DropColumn(
                name: "AbilityId",
                schema: "GameRules",
                table: "Parameters");

            migrationBuilder.DropColumn(
                name: "GameId",
                schema: "GameRules",
                table: "Abilities");

            migrationBuilder.AddColumn<Guid>(
                name: "GameId",
                schema: "GameRules",
                table: "Conditions",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "Айди игры");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatureTemplateId",
                schema: "GameRules",
                table: "Abilities",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "Айди шаблона существа");

            migrationBuilder.CreateIndex(
                name: "IX_Conditions_GameId",
                schema: "GameRules",
                table: "Conditions",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Abilities_CreatureTemplateId",
                schema: "GameRules",
                table: "Abilities",
                column: "CreatureTemplateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Abilities_CreatureTemplates_CreatureTemplateId",
                schema: "GameRules",
                table: "Abilities",
                column: "CreatureTemplateId",
                principalSchema: "GameRules",
                principalTable: "CreatureTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Conditions_Games_GameId",
                schema: "GameRules",
                table: "Conditions",
                column: "GameId",
                principalSchema: "BaseGame",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
