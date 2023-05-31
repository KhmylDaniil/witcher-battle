using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Witcher.Storage.Postgresql.Migrations
{
    public partial class ArmorTemplates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WeaponTeemplates_ItemTemplates_Id",
                schema: "Items",
                table: "WeaponTeemplates");

            migrationBuilder.DropTable(
                name: "CreatureDamageTypeModifier",
                schema: "Battles");

            migrationBuilder.DropTable(
                name: "CreatureTemplateDamageTypeModifier",
                schema: "GameRules");

            migrationBuilder.DropTable(
                name: "WeaponTeemplates_AppliedConditions",
                schema: "Items");

            migrationBuilder.DropTable(
                name: "WeaponTeemplates_DefensiveSkills",
                schema: "Items");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WeaponTeemplates",
                schema: "Items",
                table: "WeaponTeemplates");

            migrationBuilder.RenameTable(
                name: "WeaponTeemplates",
                schema: "Items",
                newName: "WeaponTemplates",
                newSchema: "Items");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WeaponTemplates",
                schema: "Items",
                table: "WeaponTemplates",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ArmorTemplates",
                schema: "Items",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    BodyTemplateId = table.Column<Guid>(type: "uuid", nullable: false),
                    Armor = table.Column<int>(type: "integer", nullable: false, comment: "Броня"),
                    EncumbranceValue = table.Column<int>(type: "integer", nullable: false, comment: "Скованность движений")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArmorTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArmorTemplates_BodyTemplates_BodyTemplateId",
                        column: x => x.BodyTemplateId,
                        principalSchema: "GameRules",
                        principalTable: "BodyTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArmorTemplates_ItemTemplates_Id",
                        column: x => x.Id,
                        principalSchema: "Items",
                        principalTable: "ItemTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Шаблоны брони");

            migrationBuilder.CreateTable(
                name: "Creatures_DamageTypeModifiers",
                schema: "Battles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DamageType = table.Column<int>(type: "integer", nullable: false),
                    DamageTypeModifier = table.Column<int>(type: "integer", nullable: false),
                    PrimaryEntityid = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatureId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Creatures_DamageTypeModifiers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Creatures_DamageTypeModifiers_Creatures_CreatureId",
                        column: x => x.CreatureId,
                        principalSchema: "Battles",
                        principalTable: "Creatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CreatureTemplates_DamageTypeModifiers",
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
                    table.PrimaryKey("PK_CreatureTemplates_DamageTypeModifiers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CreatureTemplates_DamageTypeModifiers_CreatureTemplates_Cre~",
                        column: x => x.CreatureTemplateId,
                        principalSchema: "GameRules",
                        principalTable: "CreatureTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WeaponTemplates_AppliedConditions",
                schema: "Items",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WeaponTemplateId = table.Column<Guid>(type: "uuid", nullable: false),
                    ApplyChance = table.Column<int>(type: "integer", nullable: false, comment: "Шанс применения"),
                    Condition = table.Column<string>(type: "text", nullable: false, comment: "Тип состояния"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeaponTemplates_AppliedConditions", x => new { x.WeaponTemplateId, x.Id });
                    table.ForeignKey(
                        name: "FK_WeaponTemplates_AppliedConditions_WeaponTemplates_WeaponTem~",
                        column: x => x.WeaponTemplateId,
                        principalSchema: "Items",
                        principalTable: "WeaponTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WeaponTemplates_DefensiveSkills",
                schema: "Items",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WeaponTemplateId = table.Column<Guid>(type: "uuid", nullable: false),
                    DefensiveSkill = table.Column<string>(type: "text", nullable: false, comment: "Защитный навык"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeaponTemplates_DefensiveSkills", x => new { x.WeaponTemplateId, x.Id });
                    table.ForeignKey(
                        name: "FK_WeaponTemplates_DefensiveSkills_WeaponTemplates_WeaponTempl~",
                        column: x => x.WeaponTemplateId,
                        principalSchema: "Items",
                        principalTable: "WeaponTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ArmorTemplateBodyTemplatePart",
                columns: table => new
                {
                    ArmorTemplatesId = table.Column<Guid>(type: "uuid", nullable: false),
                    BodyTemplatePartsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArmorTemplateBodyTemplatePart", x => new { x.ArmorTemplatesId, x.BodyTemplatePartsId });
                    table.ForeignKey(
                        name: "FK_ArmorTemplateBodyTemplatePart_ArmorTemplates_ArmorTemplates~",
                        column: x => x.ArmorTemplatesId,
                        principalSchema: "Items",
                        principalTable: "ArmorTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArmorTemplateBodyTemplatePart_BodyTemplateParts_BodyTemplat~",
                        column: x => x.BodyTemplatePartsId,
                        principalSchema: "GameRules",
                        principalTable: "BodyTemplateParts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ArmorTemplates_DamageTypeModifiers",
                schema: "Items",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DamageType = table.Column<int>(type: "integer", nullable: false),
                    DamageTypeModifier = table.Column<int>(type: "integer", nullable: false),
                    PrimaryEntityid = table.Column<Guid>(type: "uuid", nullable: false),
                    ArmorTemplateId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArmorTemplates_DamageTypeModifiers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArmorTemplates_DamageTypeModifiers_ArmorTemplates_ArmorTemp~",
                        column: x => x.ArmorTemplateId,
                        principalSchema: "Items",
                        principalTable: "ArmorTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArmorTemplateBodyTemplatePart_BodyTemplatePartsId",
                table: "ArmorTemplateBodyTemplatePart",
                column: "BodyTemplatePartsId");

            migrationBuilder.CreateIndex(
                name: "IX_ArmorTemplates_BodyTemplateId",
                schema: "Items",
                table: "ArmorTemplates",
                column: "BodyTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_ArmorTemplates_DamageTypeModifiers_ArmorTemplateId",
                schema: "Items",
                table: "ArmorTemplates_DamageTypeModifiers",
                column: "ArmorTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Creatures_DamageTypeModifiers_CreatureId",
                schema: "Battles",
                table: "Creatures_DamageTypeModifiers",
                column: "CreatureId");

            migrationBuilder.CreateIndex(
                name: "IX_CreatureTemplates_DamageTypeModifiers_CreatureTemplateId",
                schema: "GameRules",
                table: "CreatureTemplates_DamageTypeModifiers",
                column: "CreatureTemplateId");

            migrationBuilder.AddForeignKey(
                name: "FK_WeaponTemplates_ItemTemplates_Id",
                schema: "Items",
                table: "WeaponTemplates",
                column: "Id",
                principalSchema: "Items",
                principalTable: "ItemTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WeaponTemplates_ItemTemplates_Id",
                schema: "Items",
                table: "WeaponTemplates");

            migrationBuilder.DropTable(
                name: "ArmorTemplateBodyTemplatePart");

            migrationBuilder.DropTable(
                name: "ArmorTemplates_DamageTypeModifiers",
                schema: "Items");

            migrationBuilder.DropTable(
                name: "Creatures_DamageTypeModifiers",
                schema: "Battles");

            migrationBuilder.DropTable(
                name: "CreatureTemplates_DamageTypeModifiers",
                schema: "GameRules");

            migrationBuilder.DropTable(
                name: "WeaponTemplates_AppliedConditions",
                schema: "Items");

            migrationBuilder.DropTable(
                name: "WeaponTemplates_DefensiveSkills",
                schema: "Items");

            migrationBuilder.DropTable(
                name: "ArmorTemplates",
                schema: "Items");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WeaponTemplates",
                schema: "Items",
                table: "WeaponTemplates");

            migrationBuilder.RenameTable(
                name: "WeaponTemplates",
                schema: "Items",
                newName: "WeaponTeemplates",
                newSchema: "Items");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WeaponTeemplates",
                schema: "Items",
                table: "WeaponTeemplates",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "CreatureDamageTypeModifier",
                schema: "Battles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatureId = table.Column<Guid>(type: "uuid", nullable: false),
                    DamageType = table.Column<int>(type: "integer", nullable: false),
                    DamageTypeModifier = table.Column<int>(type: "integer", nullable: false),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PrimaryEntityid = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
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
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatureTemplateId = table.Column<Guid>(type: "uuid", nullable: false),
                    DamageType = table.Column<int>(type: "integer", nullable: false),
                    DamageTypeModifier = table.Column<int>(type: "integer", nullable: false),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PrimaryEntityid = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
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

            migrationBuilder.CreateTable(
                name: "WeaponTeemplates_AppliedConditions",
                schema: "Items",
                columns: table => new
                {
                    WeaponTemplateId = table.Column<Guid>(type: "uuid", nullable: false),
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ApplyChance = table.Column<int>(type: "integer", nullable: false, comment: "Шанс применения"),
                    Condition = table.Column<string>(type: "text", nullable: false, comment: "Тип состояния"),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeaponTeemplates_AppliedConditions", x => new { x.WeaponTemplateId, x.Id });
                    table.ForeignKey(
                        name: "FK_WeaponTeemplates_AppliedConditions_WeaponTeemplates_WeaponT~",
                        column: x => x.WeaponTemplateId,
                        principalSchema: "Items",
                        principalTable: "WeaponTeemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WeaponTeemplates_DefensiveSkills",
                schema: "Items",
                columns: table => new
                {
                    WeaponTemplateId = table.Column<Guid>(type: "uuid", nullable: false),
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true),
                    DefensiveSkill = table.Column<string>(type: "text", nullable: false, comment: "Защитный навык")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeaponTeemplates_DefensiveSkills", x => new { x.WeaponTemplateId, x.Id });
                    table.ForeignKey(
                        name: "FK_WeaponTeemplates_DefensiveSkills_WeaponTeemplates_WeaponTem~",
                        column: x => x.WeaponTemplateId,
                        principalSchema: "Items",
                        principalTable: "WeaponTeemplates",
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

            migrationBuilder.AddForeignKey(
                name: "FK_WeaponTeemplates_ItemTemplates_Id",
                schema: "Items",
                table: "WeaponTeemplates",
                column: "Id",
                principalSchema: "Items",
                principalTable: "ItemTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
