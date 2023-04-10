using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Witcher.Storage.Postgresql.Migrations
{
    public partial class reworkItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Games_GameId",
                schema: "Items",
                table: "Items");

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

            migrationBuilder.DropIndex(
                name: "IX_Items_GameId",
                schema: "Items",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Accuracy",
                schema: "Items",
                table: "Weapons");

            migrationBuilder.DropColumn(
                name: "AttackDiceQuantity",
                schema: "Items",
                table: "Weapons");

            migrationBuilder.DropColumn(
                name: "AttackSkill",
                schema: "Items",
                table: "Weapons");

            migrationBuilder.DropColumn(
                name: "DamageModifier",
                schema: "Items",
                table: "Weapons");

            migrationBuilder.DropColumn(
                name: "DamageType",
                schema: "Items",
                table: "Weapons");

            migrationBuilder.DropColumn(
                name: "Description",
                schema: "Items",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "GameId",
                schema: "Items",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "IsStackable",
                schema: "Items",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "Items",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Price",
                schema: "Items",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Weight",
                schema: "Items",
                table: "Items");

            migrationBuilder.RenameColumn(
                name: "Turn_MuitiattackAbilityId",
                schema: "Battles",
                table: "Creatures",
                newName: "Turn_MuitiattackAttackFormulaId");

            migrationBuilder.AddColumn<int>(
                name: "CurrentDurability",
                schema: "Items",
                table: "Weapons",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "Текущая прочность");

            migrationBuilder.AddColumn<Guid>(
                name: "EquippedByCharacterId",
                schema: "Items",
                table: "Weapons",
                type: "uuid",
                nullable: true,
                comment: "Экипировавший персонаж");

            migrationBuilder.AddColumn<Guid>(
                name: "BagId",
                schema: "Items",
                table: "Items",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "Айди сумки");

            migrationBuilder.AddColumn<Guid>(
                name: "ItemTemplateId",
                schema: "Items",
                table: "Items",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "Айди шаблона предмета");

            migrationBuilder.AddColumn<Guid>(
                name: "BagId",
                schema: "Characters",
                table: "Characters",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Bags",
                schema: "Characters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    CharacterId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди персонажа"),
                    MaxWeight = table.Column<int>(type: "integer", nullable: false, comment: "Макимальный вес"),
                    TotalWeight = table.Column<int>(type: "integer", nullable: false, comment: "Общий вес"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bags_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalSchema: "Characters",
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Сумки");

            migrationBuilder.CreateTable(
                name: "ItemTemplates",
                schema: "Items",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    GameId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди игры"),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Название"),
                    Description = table.Column<string>(type: "text", nullable: true, comment: "Описание"),
                    Weight = table.Column<int>(type: "integer", nullable: false, comment: "Вес"),
                    Price = table.Column<int>(type: "integer", nullable: false, comment: "Цена"),
                    IsStackable = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemTemplates_Games_GameId",
                        column: x => x.GameId,
                        principalSchema: "BaseGame",
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Шаблоны предметов");

            migrationBuilder.CreateTable(
                name: "WeaponTeemplates",
                schema: "Items",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    AttackSkill = table.Column<string>(type: "text", nullable: false, comment: "Навык атаки"),
                    DamageType = table.Column<string>(type: "text", nullable: false, comment: "Тип урона"),
                    AttackDiceQuantity = table.Column<int>(type: "integer", nullable: false, comment: "Количество кубов атаки"),
                    DamageModifier = table.Column<int>(type: "integer", nullable: false, comment: "Модификатор атаки"),
                    Accuracy = table.Column<int>(type: "integer", nullable: false, comment: "Точность атаки"),
                    Durability = table.Column<int>(type: "integer", nullable: false, comment: "Прочность")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeaponTeemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeaponTeemplates_ItemTemplates_Id",
                        column: x => x.Id,
                        principalSchema: "Items",
                        principalTable: "ItemTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Шаблоны оружия");

            migrationBuilder.CreateTable(
                name: "WeaponTeemplates_AppliedConditions",
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
                name: "IX_Weapons_EquippedByCharacterId",
                schema: "Items",
                table: "Weapons",
                column: "EquippedByCharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_BagId",
                schema: "Items",
                table: "Items",
                column: "BagId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_ItemTemplateId",
                schema: "Items",
                table: "Items",
                column: "ItemTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Bags_CharacterId",
                schema: "Characters",
                table: "Bags",
                column: "CharacterId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItemTemplates_GameId",
                schema: "Items",
                table: "ItemTemplates",
                column: "GameId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Bags_BagId",
                schema: "Items",
                table: "Items",
                column: "BagId",
                principalSchema: "Characters",
                principalTable: "Bags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_ItemTemplates_ItemTemplateId",
                schema: "Items",
                table: "Items",
                column: "ItemTemplateId",
                principalSchema: "Items",
                principalTable: "ItemTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Weapons_Characters_EquippedByCharacterId",
                schema: "Items",
                table: "Weapons",
                column: "EquippedByCharacterId",
                principalSchema: "Characters",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Bags_BagId",
                schema: "Items",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_ItemTemplates_ItemTemplateId",
                schema: "Items",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Weapons_Characters_EquippedByCharacterId",
                schema: "Items",
                table: "Weapons");

            migrationBuilder.DropTable(
                name: "Bags",
                schema: "Characters");

            migrationBuilder.DropTable(
                name: "WeaponTeemplates_AppliedConditions",
                schema: "Items");

            migrationBuilder.DropTable(
                name: "WeaponTeemplates_DefensiveSkills",
                schema: "Items");

            migrationBuilder.DropTable(
                name: "WeaponTeemplates",
                schema: "Items");

            migrationBuilder.DropTable(
                name: "ItemTemplates",
                schema: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Weapons_EquippedByCharacterId",
                schema: "Items",
                table: "Weapons");

            migrationBuilder.DropIndex(
                name: "IX_Items_BagId",
                schema: "Items",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_ItemTemplateId",
                schema: "Items",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "CurrentDurability",
                schema: "Items",
                table: "Weapons");

            migrationBuilder.DropColumn(
                name: "EquippedByCharacterId",
                schema: "Items",
                table: "Weapons");

            migrationBuilder.DropColumn(
                name: "BagId",
                schema: "Items",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "ItemTemplateId",
                schema: "Items",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "BagId",
                schema: "Characters",
                table: "Characters");

            migrationBuilder.RenameColumn(
                name: "Turn_MuitiattackAttackFormulaId",
                schema: "Battles",
                table: "Creatures",
                newName: "Turn_MuitiattackAbilityId");

            migrationBuilder.AddColumn<int>(
                name: "Accuracy",
                schema: "Items",
                table: "Weapons",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "Точность атаки");

            migrationBuilder.AddColumn<int>(
                name: "AttackDiceQuantity",
                schema: "Items",
                table: "Weapons",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "Количество кубов атаки");

            migrationBuilder.AddColumn<string>(
                name: "AttackSkill",
                schema: "Items",
                table: "Weapons",
                type: "text",
                nullable: false,
                defaultValue: "",
                comment: "Навык атаки");

            migrationBuilder.AddColumn<int>(
                name: "DamageModifier",
                schema: "Items",
                table: "Weapons",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "Модификатор атаки");

            migrationBuilder.AddColumn<string>(
                name: "DamageType",
                schema: "Items",
                table: "Weapons",
                type: "text",
                nullable: false,
                defaultValue: "",
                comment: "Тип урона");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "Items",
                table: "Items",
                type: "text",
                nullable: true,
                comment: "Описание");

            migrationBuilder.AddColumn<Guid>(
                name: "GameId",
                schema: "Items",
                table: "Items",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "Айди игры");

            migrationBuilder.AddColumn<bool>(
                name: "IsStackable",
                schema: "Items",
                table: "Items",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "Items",
                table: "Items",
                type: "text",
                nullable: false,
                defaultValue: "",
                comment: "Название");

            migrationBuilder.AddColumn<int>(
                name: "Price",
                schema: "Items",
                table: "Items",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "Цена");

            migrationBuilder.AddColumn<int>(
                name: "Weight",
                schema: "Items",
                table: "Items",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "Вес");

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
                    WeaponId = table.Column<Guid>(type: "uuid", nullable: false),
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
                    WeaponId = table.Column<Guid>(type: "uuid", nullable: false),
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
                name: "IX_Items_GameId",
                schema: "Items",
                table: "Items",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterItem_ItemsId",
                table: "CharacterItem",
                column: "ItemsId");

            migrationBuilder.CreateIndex(
                name: "IX_EquippedWeapons_EquippedWeaponsId",
                schema: "Items",
                table: "EquippedWeapons",
                column: "EquippedWeaponsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Games_GameId",
                schema: "Items",
                table: "Items",
                column: "GameId",
                principalSchema: "BaseGame",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
