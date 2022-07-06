using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sindie.ApiService.Storage.Postgresql.Migrations
{
    public partial class Witcher : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NotificationTradeRequests_Bags_BagId",
                schema: "Notifications",
                table: "NotificationTradeRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_Parameters_Prerequisites_Id",
                schema: "GameRules",
                table: "Parameters");

            migrationBuilder.DropIndex(
                name: "IX_NotificationTradeRequests_BagId",
                schema: "Notifications",
                table: "NotificationTradeRequests");

            migrationBuilder.DropColumn(
                name: "BagId",
                schema: "Notifications",
                table: "NotificationTradeRequests");

            migrationBuilder.AlterColumn<int>(
                name: "MinValueParameters",
                schema: "GameRules",
                table: "Parameters",
                type: "integer",
                nullable: true,
                comment: "Минимальные значения параметра",
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true,
                oldComment: "Минимальные значения параметра");

            migrationBuilder.AlterColumn<int>(
                name: "MaxValueParameters",
                schema: "GameRules",
                table: "Parameters",
                type: "integer",
                nullable: true,
                comment: "Максимальные значения параметра",
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true,
                oldComment: "Максимальные значения параметра");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedByUserId",
                schema: "GameRules",
                table: "Parameters",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                schema: "GameRules",
                table: "Parameters",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now() at time zone 'utc'");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "GameRules",
                table: "Parameters",
                type: "text",
                nullable: true,
                comment: "Описание параметра");

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedByUserId",
                schema: "GameRules",
                table: "Parameters",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                schema: "GameRules",
                table: "Parameters",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "GameRules",
                table: "Parameters",
                type: "text",
                nullable: false,
                defaultValue: "",
                comment: "Название параметра");

            migrationBuilder.AddColumn<string>(
                name: "RoleCreatedUser",
                schema: "GameRules",
                table: "Parameters",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoleModifiedUser",
                schema: "GameRules",
                table: "Parameters",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StatName",
                schema: "GameRules",
                table: "Parameters",
                type: "text",
                nullable: true,
                comment: "Название корреспондирующей характеристики");

            migrationBuilder.CreateTable(
                name: "Abilities",
                schema: "GameRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    GameId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди игры"),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Название способности"),
                    Description = table.Column<string>(type: "text", nullable: true, comment: "Описание способности"),
                    AttackParameterId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Параметр атаки"),
                    AttackDiceQuantity = table.Column<int>(type: "integer", nullable: false, comment: "Количество кубов атаки"),
                    DamageModifier = table.Column<int>(type: "integer", nullable: false, comment: "Модификатор атаки"),
                    AttackSpeed = table.Column<int>(type: "integer", nullable: false, comment: "Скорость атаки"),
                    Accuracy = table.Column<int>(type: "integer", nullable: false, comment: "Точность атаки"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Abilities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Abilities_Games_GameId",
                        column: x => x.GameId,
                        principalSchema: "BaseGame",
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Abilities_Parameters_AttackParameterId",
                        column: x => x.AttackParameterId,
                        principalSchema: "GameRules",
                        principalTable: "Parameters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Способности");

            migrationBuilder.CreateTable(
                name: "BodyPartTypes",
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
                    table.PrimaryKey("PK_BodyPartTypes", x => x.Id);
                },
                comment: "Типы частей тела");

            migrationBuilder.CreateTable(
                name: "BodyTemplates",
                schema: "GameRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Название шаблона тела"),
                    Description = table.Column<string>(type: "text", nullable: true, comment: "Описание"),
                    GameId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди игры"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BodyTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BodyTemplates_Games_GameId",
                        column: x => x.GameId,
                        principalSchema: "BaseGame",
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Шаблоны тел");

            migrationBuilder.CreateTable(
                name: "Conditions",
                schema: "GameRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Название состояния"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conditions", x => x.Id);
                },
                comment: "Состояния");

            migrationBuilder.CreateTable(
                name: "CreatureTypes",
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
                    table.PrimaryKey("PK_CreatureTypes", x => x.Id);
                },
                comment: "Типы существ");

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
                name: "DefensiveParameters",
                schema: "GameRules",
                columns: table => new
                {
                    AbilitiesForDefenseId = table.Column<Guid>(type: "uuid", nullable: false),
                    DefensiveParametersId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DefensiveParameters", x => new { x.AbilitiesForDefenseId, x.DefensiveParametersId });
                    table.ForeignKey(
                        name: "FK_DefensiveParameters_Abilities_AbilitiesForDefenseId",
                        column: x => x.AbilitiesForDefenseId,
                        principalSchema: "GameRules",
                        principalTable: "Abilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DefensiveParameters_Parameters_DefensiveParametersId",
                        column: x => x.DefensiveParametersId,
                        principalSchema: "GameRules",
                        principalTable: "Parameters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BodyParts",
                schema: "GameRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    BodyPartTypeId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди типа части тела"),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Название"),
                    HitPenalty = table.Column<int>(type: "integer", nullable: false, comment: "Пенальти за прицеливание"),
                    DamageModifer = table.Column<double>(type: "double precision", nullable: false, comment: "Модификатор урона"),
                    MinToHit = table.Column<int>(type: "integer", nullable: false, comment: "Минимальное значение попадания"),
                    MaxToHit = table.Column<int>(type: "integer", nullable: false, comment: "Максимальное значение попадания"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BodyParts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BodyParts_BodyPartTypes_BodyPartTypeId",
                        column: x => x.BodyPartTypeId,
                        principalSchema: "GameRules",
                        principalTable: "BodyPartTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Части тела");

            migrationBuilder.CreateTable(
                name: "AppliedConditions",
                schema: "GameRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    AbilityId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди способнности"),
                    ApplyChance = table.Column<int>(type: "integer", nullable: false, comment: "Шанс применения"),
                    ConditionId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди состояния"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppliedConditions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppliedConditions_Abilities_AbilityId",
                        column: x => x.AbilityId,
                        principalSchema: "GameRules",
                        principalTable: "Abilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppliedConditions_Conditions_ConditionId",
                        column: x => x.ConditionId,
                        principalSchema: "GameRules",
                        principalTable: "Conditions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Применяемые состояния");

            migrationBuilder.CreateTable(
                name: "CreatureTemplates",
                schema: "GameRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    GameId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди игры"),
                    ImgFileId = table.Column<Guid>(type: "uuid", nullable: true, comment: "Айди графического файла"),
                    BodyTemplateId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди шаблона тела"),
                    CreatureTypeId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди типа существа"),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Название шаблона"),
                    Description = table.Column<string>(type: "text", nullable: true, comment: "Описание шаблона"),
                    HP = table.Column<int>(type: "integer", nullable: false, comment: "Хиты"),
                    Sta = table.Column<int>(type: "integer", nullable: false, comment: "Стамина"),
                    Int = table.Column<int>(type: "integer", nullable: false, comment: "Интеллект"),
                    Ref = table.Column<int>(type: "integer", nullable: false, comment: "Рефлексы"),
                    Dex = table.Column<int>(type: "integer", nullable: false, comment: "Ловкость"),
                    Body = table.Column<int>(type: "integer", nullable: false, comment: "Телосложение"),
                    Emp = table.Column<int>(type: "integer", nullable: false, comment: "Эмпатия"),
                    Cra = table.Column<int>(type: "integer", nullable: false, comment: "Крафт"),
                    Will = table.Column<int>(type: "integer", nullable: false, comment: "Воля"),
                    Luck = table.Column<int>(type: "integer", nullable: false, comment: "Удача"),
                    Speed = table.Column<int>(type: "integer", nullable: false, comment: "Скорость"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreatureTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CreatureTemplates_BodyTemplates_BodyTemplateId",
                        column: x => x.BodyTemplateId,
                        principalSchema: "GameRules",
                        principalTable: "BodyTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CreatureTemplates_CreatureTypes_CreatureTypeId",
                        column: x => x.CreatureTypeId,
                        principalSchema: "GameRules",
                        principalTable: "CreatureTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CreatureTemplates_Games_GameId",
                        column: x => x.GameId,
                        principalSchema: "BaseGame",
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CreatureTemplates_ImgFiles_ImgFileId",
                        column: x => x.ImgFileId,
                        principalSchema: "System",
                        principalTable: "ImgFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                },
                comment: "Шаблоны существ");

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
                name: "BodyTemplateParts",
                schema: "GameRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    BodyTemplateId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди шаблона тела")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BodyTemplateParts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BodyTemplateParts_BodyParts_Id",
                        column: x => x.Id,
                        principalSchema: "GameRules",
                        principalTable: "BodyParts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BodyTemplateParts_BodyTemplates_BodyTemplateId",
                        column: x => x.BodyTemplateId,
                        principalSchema: "GameRules",
                        principalTable: "BodyTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Части шаблона тела");

            migrationBuilder.CreateTable(
                name: "Creatures",
                schema: "GameInstance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    InstanceId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди экземпляра"),
                    ImgFileId = table.Column<Guid>(type: "uuid", nullable: true, comment: "Айди графического файла"),
                    CreatureTemplateId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди шаблона существа"),
                    BodyTemplateId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди шаблона тела"),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Название существа"),
                    Description = table.Column<string>(type: "text", nullable: true, comment: "Описание шаблона"),
                    CreatureTypeId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди типа существа"),
                    HP = table.Column<int>(type: "integer", nullable: false, comment: "Хиты"),
                    Sta = table.Column<int>(type: "integer", nullable: false, comment: "Стамина"),
                    Int = table.Column<int>(type: "integer", nullable: false, comment: "Интеллект"),
                    Ref = table.Column<int>(type: "integer", nullable: false, comment: "Рефлексы"),
                    Dex = table.Column<int>(type: "integer", nullable: false, comment: "Ловкость"),
                    Body = table.Column<int>(type: "integer", nullable: false, comment: "Телосложение"),
                    Emp = table.Column<int>(type: "integer", nullable: false, comment: "Эмпатия"),
                    Cra = table.Column<int>(type: "integer", nullable: false, comment: "Крафт"),
                    Will = table.Column<int>(type: "integer", nullable: false, comment: "Воля"),
                    Luck = table.Column<int>(type: "integer", nullable: false, comment: "Удача"),
                    Speed = table.Column<int>(type: "integer", nullable: false, comment: "Скорость"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Creatures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Creatures_BodyTemplates_BodyTemplateId",
                        column: x => x.BodyTemplateId,
                        principalSchema: "GameRules",
                        principalTable: "BodyTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Creatures_CreatureTemplates_CreatureTemplateId",
                        column: x => x.CreatureTemplateId,
                        principalSchema: "GameRules",
                        principalTable: "CreatureTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Creatures_CreatureTypes_CreatureTypeId",
                        column: x => x.CreatureTypeId,
                        principalSchema: "GameRules",
                        principalTable: "CreatureTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Creatures_ImgFiles_ImgFileId",
                        column: x => x.ImgFileId,
                        principalSchema: "System",
                        principalTable: "ImgFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Creatures_Instances_InstanceId",
                        column: x => x.InstanceId,
                        principalSchema: "GameInstance",
                        principalTable: "Instances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Существа");

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
                name: "CreatureTemplateParameters",
                schema: "GameInstance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    CreatureId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди шаблона существа"),
                    ParameterId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди параметра"),
                    StatName = table.Column<string>(type: "text", nullable: true, comment: "Название корреспондирующей характеристики"),
                    ParametrValue = table.Column<int>(type: "integer", nullable: false, comment: "Значение параметра"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreatureTemplateParameters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CreatureTemplateParameters_CreatureTemplates_CreatureId",
                        column: x => x.CreatureId,
                        principalSchema: "GameRules",
                        principalTable: "CreatureTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CreatureTemplateParameters_Parameters_ParameterId",
                        column: x => x.ParameterId,
                        principalSchema: "GameRules",
                        principalTable: "Parameters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Параметры шаблона существа");

            migrationBuilder.CreateTable(
                name: "CreatureTemplateParts",
                schema: "GameRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    CreatureTemplateId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди шаблона существа"),
                    Armor = table.Column<int>(type: "integer", nullable: false, comment: "Броня")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreatureTemplateParts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CreatureTemplateParts_BodyParts_Id",
                        column: x => x.Id,
                        principalSchema: "GameRules",
                        principalTable: "BodyParts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CreatureTemplateParts_CreatureTemplates_CreatureTemplateId",
                        column: x => x.CreatureTemplateId,
                        principalSchema: "GameRules",
                        principalTable: "CreatureTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Части шаблона существа");

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
                name: "CreatureAbilities",
                schema: "GameInstance",
                columns: table => new
                {
                    AbilitiesId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreaturesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreatureAbilities", x => new { x.AbilitiesId, x.CreaturesId });
                    table.ForeignKey(
                        name: "FK_CreatureAbilities_Abilities_AbilitiesId",
                        column: x => x.AbilitiesId,
                        principalSchema: "GameRules",
                        principalTable: "Abilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CreatureAbilities_Creatures_CreaturesId",
                        column: x => x.CreaturesId,
                        principalSchema: "GameInstance",
                        principalTable: "Creatures",
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
                name: "CreatureParameters",
                schema: "GameInstance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    CreatureId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди существа"),
                    ParameterId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди параметра"),
                    StatName = table.Column<string>(type: "text", nullable: true, comment: "Название корреспондирующей характеристики"),
                    ParametrValue = table.Column<int>(type: "integer", nullable: false, comment: "Значение параметра"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreatureParameters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CreatureParameters_Creatures_CreatureId",
                        column: x => x.CreatureId,
                        principalSchema: "GameInstance",
                        principalTable: "Creatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CreatureParameters_Parameters_ParameterId",
                        column: x => x.ParameterId,
                        principalSchema: "GameRules",
                        principalTable: "Parameters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Параметры существа");

            migrationBuilder.CreateTable(
                name: "CreatureParts",
                schema: "GameInstance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    CreatureId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди существа"),
                    StartingArmor = table.Column<int>(type: "integer", nullable: false, comment: "Стартовая броня"),
                    CurrentArmor = table.Column<int>(type: "integer", nullable: false, comment: "Текущая броня")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreatureParts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CreatureParts_BodyParts_Id",
                        column: x => x.Id,
                        principalSchema: "GameRules",
                        principalTable: "BodyParts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CreatureParts_Creatures_CreatureId",
                        column: x => x.CreatureId,
                        principalSchema: "GameInstance",
                        principalTable: "Creatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Части существа");

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

            migrationBuilder.CreateTable(
                name: "CurrentConditions",
                schema: "GameInstance",
                columns: table => new
                {
                    ConditionsId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreaturesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrentConditions", x => new { x.ConditionsId, x.CreaturesId });
                    table.ForeignKey(
                        name: "FK_CurrentConditions_Conditions_ConditionsId",
                        column: x => x.ConditionsId,
                        principalSchema: "GameRules",
                        principalTable: "Conditions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CurrentConditions_Creatures_CreaturesId",
                        column: x => x.CreaturesId,
                        principalSchema: "GameInstance",
                        principalTable: "Creatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "GameRules",
                table: "BodyPartTypes",
                columns: new[] { "Id", "CreatedByUserId", "CreatedOn", "ModifiedByUserId", "ModifiedOn", "Name", "RoleCreatedUser", "RoleModifiedUser" },
                values: new object[,]
                {
                    { new Guid("8894e0d0-3147-4791-1153-9667cbe127d7"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Tail", "Default", "Default" },
                    { new Guid("8894e0d0-3147-4791-1353-9667cbe127d7"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Void", "Default", "Default" },
                    { new Guid("8894e0d0-3147-4791-9153-9667cbe127d7"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Head", "Default", "Default" },
                    { new Guid("8894e0d0-3147-4791-9353-9667cbe127d7"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Torso", "Default", "Default" },
                    { new Guid("8894e0d0-3147-4791-9553-9667cbe127d7"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Arm", "Default", "Default" },
                    { new Guid("8894e0d0-3147-4791-9753-9667cbe127d7"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Leg", "Default", "Default" },
                    { new Guid("8894e0d0-3147-4791-9953-9667cbe127d7"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Wing", "Default", "Default" }
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
                table: "CreatureTypes",
                columns: new[] { "Id", "CreatedByUserId", "CreatedOn", "ModifiedByUserId", "ModifiedOn", "Name", "RoleCreatedUser", "RoleModifiedUser" },
                values: new object[,]
                {
                    { new Guid("03ca5eb6-6534-4eea-9616-78e3ef0d572c"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Human", "Default", "Default" },
                    { new Guid("04ca5eb6-6534-4eea-9616-78e3ef0d572c"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Necrophage", "Default", "Default" },
                    { new Guid("05ca5eb6-6534-4eea-9616-78e3ef0d572c"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Specter", "Default", "Default" },
                    { new Guid("06ca5eb6-6534-4eea-9616-78e3ef0d572c"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Beast", "Default", "Default" },
                    { new Guid("07ca5eb6-6534-4eea-9616-78e3ef0d572c"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Cursed", "Default", "Default" },
                    { new Guid("08ca5eb6-6534-4eea-9616-78e3ef0d572c"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Hybrid", "Default", "Default" },
                    { new Guid("09ca5eb6-6534-4eea-9616-78e3ef0d572c"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Insectoid", "Default", "Default" },
                    { new Guid("13ca5eb6-6534-4eea-9616-78e3ef0d572c"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Elementa", "Default", "Default" },
                    { new Guid("23ca5eb6-6534-4eea-9616-78e3ef0d572c"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Relict", "Default", "Default" },
                    { new Guid("33ca5eb6-6534-4eea-9616-78e3ef0d572c"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ogroid", "Default", "Default" },
                    { new Guid("43ca5eb6-6534-4eea-9616-78e3ef0d572c"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Draconid", "Default", "Default" },
                    { new Guid("53ca5eb6-6534-4eea-9616-78e3ef0d572c"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Vampire", "Default", "Default" }
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
                name: "IX_Abilities_AttackParameterId",
                schema: "GameRules",
                table: "Abilities",
                column: "AttackParameterId");

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
                name: "IX_AppliedConditions_AbilityId",
                schema: "GameRules",
                table: "AppliedConditions",
                column: "AbilityId");

            migrationBuilder.CreateIndex(
                name: "IX_AppliedConditions_ConditionId",
                schema: "GameRules",
                table: "AppliedConditions",
                column: "ConditionId");

            migrationBuilder.CreateIndex(
                name: "IX_BodyParts_BodyPartTypeId",
                schema: "GameRules",
                table: "BodyParts",
                column: "BodyPartTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_BodyTemplateParts_BodyTemplateId",
                schema: "GameRules",
                table: "BodyTemplateParts",
                column: "BodyTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_BodyTemplates_GameId",
                schema: "GameRules",
                table: "BodyTemplates",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_CreatureAbilities_CreaturesId",
                schema: "GameInstance",
                table: "CreatureAbilities",
                column: "CreaturesId");

            migrationBuilder.CreateIndex(
                name: "IX_CreatureImmunities_ImmunitiesId",
                schema: "GameInstance",
                table: "CreatureImmunities",
                column: "ImmunitiesId");

            migrationBuilder.CreateIndex(
                name: "IX_CreatureParameters_CreatureId",
                schema: "GameInstance",
                table: "CreatureParameters",
                column: "CreatureId");

            migrationBuilder.CreateIndex(
                name: "IX_CreatureParameters_ParameterId",
                schema: "GameInstance",
                table: "CreatureParameters",
                column: "ParameterId");

            migrationBuilder.CreateIndex(
                name: "IX_CreatureParts_CreatureId",
                schema: "GameInstance",
                table: "CreatureParts",
                column: "CreatureId");

            migrationBuilder.CreateIndex(
                name: "IX_CreatureResistances_ResistantCreaturesId",
                schema: "GameInstance",
                table: "CreatureResistances",
                column: "ResistantCreaturesId");

            migrationBuilder.CreateIndex(
                name: "IX_Creatures_BodyTemplateId",
                schema: "GameInstance",
                table: "Creatures",
                column: "BodyTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Creatures_CreatureTemplateId",
                schema: "GameInstance",
                table: "Creatures",
                column: "CreatureTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Creatures_CreatureTypeId",
                schema: "GameInstance",
                table: "Creatures",
                column: "CreatureTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Creatures_ImgFileId",
                schema: "GameInstance",
                table: "Creatures",
                column: "ImgFileId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Creatures_InstanceId",
                schema: "GameInstance",
                table: "Creatures",
                column: "InstanceId");

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
                name: "IX_CreatureTemplateParameters_CreatureId",
                schema: "GameInstance",
                table: "CreatureTemplateParameters",
                column: "CreatureId");

            migrationBuilder.CreateIndex(
                name: "IX_CreatureTemplateParameters_ParameterId",
                schema: "GameInstance",
                table: "CreatureTemplateParameters",
                column: "ParameterId");

            migrationBuilder.CreateIndex(
                name: "IX_CreatureTemplateParts_CreatureTemplateId",
                schema: "GameRules",
                table: "CreatureTemplateParts",
                column: "CreatureTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_CreatureTemplateResistances_ResistantCreatureTemplatesId",
                schema: "GameRules",
                table: "CreatureTemplateResistances",
                column: "ResistantCreatureTemplatesId");

            migrationBuilder.CreateIndex(
                name: "IX_CreatureTemplates_BodyTemplateId",
                schema: "GameRules",
                table: "CreatureTemplates",
                column: "BodyTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_CreatureTemplates_CreatureTypeId",
                schema: "GameRules",
                table: "CreatureTemplates",
                column: "CreatureTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CreatureTemplates_GameId",
                schema: "GameRules",
                table: "CreatureTemplates",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_CreatureTemplates_ImgFileId",
                schema: "GameRules",
                table: "CreatureTemplates",
                column: "ImgFileId",
                unique: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_CurrentConditions_CreaturesId",
                schema: "GameInstance",
                table: "CurrentConditions",
                column: "CreaturesId");

            migrationBuilder.CreateIndex(
                name: "IX_DefensiveParameters_DefensiveParametersId",
                schema: "GameRules",
                table: "DefensiveParameters",
                column: "DefensiveParametersId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AbilityDamageTypes",
                schema: "GameRules");

            migrationBuilder.DropTable(
                name: "AppliedConditions",
                schema: "GameRules");

            migrationBuilder.DropTable(
                name: "BodyTemplateParts",
                schema: "GameRules");

            migrationBuilder.DropTable(
                name: "CreatureAbilities",
                schema: "GameInstance");

            migrationBuilder.DropTable(
                name: "CreatureImmunities",
                schema: "GameInstance");

            migrationBuilder.DropTable(
                name: "CreatureParameters",
                schema: "GameInstance");

            migrationBuilder.DropTable(
                name: "CreatureParts",
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
                name: "CreatureTemplateParameters",
                schema: "GameInstance");

            migrationBuilder.DropTable(
                name: "CreatureTemplateParts",
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
                name: "CurrentConditions",
                schema: "GameInstance");

            migrationBuilder.DropTable(
                name: "DefensiveParameters",
                schema: "GameRules");

            migrationBuilder.DropTable(
                name: "BodyParts",
                schema: "GameRules");

            migrationBuilder.DropTable(
                name: "DamageTypes",
                schema: "GameRules");

            migrationBuilder.DropTable(
                name: "Conditions",
                schema: "GameRules");

            migrationBuilder.DropTable(
                name: "Creatures",
                schema: "GameInstance");

            migrationBuilder.DropTable(
                name: "Abilities",
                schema: "GameRules");

            migrationBuilder.DropTable(
                name: "BodyPartTypes",
                schema: "GameRules");

            migrationBuilder.DropTable(
                name: "CreatureTemplates",
                schema: "GameRules");

            migrationBuilder.DropTable(
                name: "BodyTemplates",
                schema: "GameRules");

            migrationBuilder.DropTable(
                name: "CreatureTypes",
                schema: "GameRules");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                schema: "GameRules",
                table: "Parameters");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                schema: "GameRules",
                table: "Parameters");

            migrationBuilder.DropColumn(
                name: "Description",
                schema: "GameRules",
                table: "Parameters");

            migrationBuilder.DropColumn(
                name: "ModifiedByUserId",
                schema: "GameRules",
                table: "Parameters");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                schema: "GameRules",
                table: "Parameters");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "GameRules",
                table: "Parameters");

            migrationBuilder.DropColumn(
                name: "RoleCreatedUser",
                schema: "GameRules",
                table: "Parameters");

            migrationBuilder.DropColumn(
                name: "RoleModifiedUser",
                schema: "GameRules",
                table: "Parameters");

            migrationBuilder.DropColumn(
                name: "StatName",
                schema: "GameRules",
                table: "Parameters");

            migrationBuilder.AlterColumn<double>(
                name: "MinValueParameters",
                schema: "GameRules",
                table: "Parameters",
                type: "double precision",
                nullable: true,
                comment: "Минимальные значения параметра",
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true,
                oldComment: "Минимальные значения параметра");

            migrationBuilder.AlterColumn<double>(
                name: "MaxValueParameters",
                schema: "GameRules",
                table: "Parameters",
                type: "double precision",
                nullable: true,
                comment: "Максимальные значения параметра",
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true,
                oldComment: "Максимальные значения параметра");

            migrationBuilder.AddColumn<Guid>(
                name: "BagId",
                schema: "Notifications",
                table: "NotificationTradeRequests",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_NotificationTradeRequests_BagId",
                schema: "Notifications",
                table: "NotificationTradeRequests",
                column: "BagId");

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationTradeRequests_Bags_BagId",
                schema: "Notifications",
                table: "NotificationTradeRequests",
                column: "BagId",
                principalSchema: "GameInstance",
                principalTable: "Bags",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Parameters_Prerequisites_Id",
                schema: "GameRules",
                table: "Parameters",
                column: "Id",
                principalSchema: "GameRules",
                principalTable: "Prerequisites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
