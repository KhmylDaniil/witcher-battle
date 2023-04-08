using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Witcher.Storage.Postgresql.Migrations
{
    public partial class addWeapons : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Characters_GameId",
                schema: "Items",
                table: "Items");

            migrationBuilder.DropTable(
                name: "AppliedConditions",
                schema: "GameRules");

            migrationBuilder.DropTable(
                name: "DefensiveSkill",
                schema: "GameRules");

            migrationBuilder.DropColumn(
                name: "CharacterId",
                schema: "Items",
                table: "Items");

            migrationBuilder.CreateTable(
                name: "Abilities_AppliedConditions",
                schema: "GameRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AbilityId = table.Column<Guid>(type: "uuid", nullable: false),
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
                    table.PrimaryKey("PK_Abilities_AppliedConditions", x => new { x.AbilityId, x.Id });
                    table.ForeignKey(
                        name: "FK_Abilities_AppliedConditions_Abilities_AbilityId",
                        column: x => x.AbilityId,
                        principalSchema: "GameRules",
                        principalTable: "Abilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Abilities_DefensiveSkills",
                schema: "GameRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AbilityId = table.Column<Guid>(type: "uuid", nullable: false),
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
                    table.PrimaryKey("PK_Abilities_DefensiveSkills", x => new { x.AbilityId, x.Id });
                    table.ForeignKey(
                        name: "FK_Abilities_DefensiveSkills_Abilities_AbilityId",
                        column: x => x.AbilityId,
                        principalSchema: "GameRules",
                        principalTable: "Abilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CharacterItem",
                columns: table => new
                {
                    CharactersId = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterItem", x => new { x.CharactersId, x.ItemsId });
                    table.ForeignKey(
                        name: "FK_CharacterItem_Characters_CharactersId",
                        column: x => x.CharactersId,
                        principalSchema: "Characters",
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterItem_Items_ItemsId",
                        column: x => x.ItemsId,
                        principalSchema: "Items",
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Weapons",
                schema: "Items",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    AttackSkill = table.Column<string>(type: "text", nullable: false, comment: "Навык атаки"),
                    DamageType = table.Column<string>(type: "text", nullable: false, comment: "Тип урона"),
                    AttackDiceQuantity = table.Column<int>(type: "integer", nullable: false, comment: "Количество кубов атаки"),
                    DamageModifier = table.Column<int>(type: "integer", nullable: false, comment: "Модификатор атаки"),
                    Accuracy = table.Column<int>(type: "integer", nullable: false, comment: "Точность атаки")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weapons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Weapons_Items_Id",
                        column: x => x.Id,
                        principalSchema: "Items",
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Оружие");

            migrationBuilder.CreateTable(
                name: "EquippedWeapons",
                schema: "Items",
                columns: table => new
                {
                    EquippedByCharacterId = table.Column<Guid>(type: "uuid", nullable: false),
                    EquippedWeaponsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquippedWeapons", x => new { x.EquippedByCharacterId, x.EquippedWeaponsId });
                    table.ForeignKey(
                        name: "FK_EquippedWeapons_Characters_EquippedByCharacterId",
                        column: x => x.EquippedByCharacterId,
                        principalSchema: "Characters",
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EquippedWeapons_Weapons_EquippedWeaponsId",
                        column: x => x.EquippedWeaponsId,
                        principalSchema: "Items",
                        principalTable: "Weapons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Weapons_AppliedConditions",
                schema: "Items",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WeaponId = table.Column<Guid>(type: "uuid", nullable: false),
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
                    table.PrimaryKey("PK_Weapons_AppliedConditions", x => new { x.WeaponId, x.Id });
                    table.ForeignKey(
                        name: "FK_Weapons_AppliedConditions_Weapons_WeaponId",
                        column: x => x.WeaponId,
                        principalSchema: "Items",
                        principalTable: "Weapons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Weapons_DefensiveSkills",
                schema: "Items",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WeaponId = table.Column<Guid>(type: "uuid", nullable: false),
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
                    table.PrimaryKey("PK_Weapons_DefensiveSkills", x => new { x.WeaponId, x.Id });
                    table.ForeignKey(
                        name: "FK_Weapons_DefensiveSkills_Weapons_WeaponId",
                        column: x => x.WeaponId,
                        principalSchema: "Items",
                        principalTable: "Weapons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CharacterItem_ItemsId",
                table: "CharacterItem",
                column: "ItemsId");

            migrationBuilder.CreateIndex(
                name: "IX_EquippedWeapons_EquippedWeaponsId",
                schema: "Items",
                table: "EquippedWeapons",
                column: "EquippedWeaponsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Abilities_AppliedConditions",
                schema: "GameRules");

            migrationBuilder.DropTable(
                name: "Abilities_DefensiveSkills",
                schema: "GameRules");

            migrationBuilder.DropTable(
                name: "CharacterItem");

            migrationBuilder.DropTable(
                name: "EquippedWeapons",
                schema: "Items");

            migrationBuilder.DropTable(
                name: "Weapons_AppliedConditions",
                schema: "Items");

            migrationBuilder.DropTable(
                name: "Weapons_DefensiveSkills",
                schema: "Items");

            migrationBuilder.DropTable(
                name: "Weapons",
                schema: "Items");

            migrationBuilder.AddColumn<Guid>(
                name: "CharacterId",
                schema: "Items",
                table: "Items",
                type: "uuid",
                nullable: true,
                comment: "Айди персонажа");

            migrationBuilder.CreateTable(
                name: "AppliedConditions",
                schema: "GameRules",
                columns: table => new
                {
                    AbilityId = table.Column<Guid>(type: "uuid", nullable: false),
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
                    table.PrimaryKey("PK_AppliedConditions", x => new { x.AbilityId, x.Id });
                    table.ForeignKey(
                        name: "FK_AppliedConditions_Abilities_AbilityId",
                        column: x => x.AbilityId,
                        principalSchema: "GameRules",
                        principalTable: "Abilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DefensiveSkill",
                schema: "GameRules",
                columns: table => new
                {
                    AbilityId = table.Column<Guid>(type: "uuid", nullable: false),
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true),
                    Skill = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DefensiveSkill", x => new { x.AbilityId, x.Id });
                    table.ForeignKey(
                        name: "FK_DefensiveSkill_Abilities_AbilityId",
                        column: x => x.AbilityId,
                        principalSchema: "GameRules",
                        principalTable: "Abilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Characters_GameId",
                schema: "Items",
                table: "Items",
                column: "GameId",
                principalSchema: "Characters",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
