using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Sindie.ApiService.Storage.Postgresql.Migrations
{
    public partial class Creature : Migration
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
                name: "BodyTemplatePart",
                schema: "GameRules",
                columns: table => new
                {
                    BodyTemplateId = table.Column<Guid>(type: "uuid", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Название"),
                    DamageModifer = table.Column<double>(type: "double precision", nullable: false, comment: "Модификатор урона"),
                    MinToHit = table.Column<int>(type: "integer", nullable: false, comment: "Минимальное значение попадания"),
                    MaxToHit = table.Column<int>(type: "integer", nullable: false, comment: "Максимальное значение попадания")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BodyTemplatePart", x => new { x.BodyTemplateId, x.Id });
                    table.ForeignKey(
                        name: "FK_BodyTemplatePart_BodyTemplates_BodyTemplateId",
                        column: x => x.BodyTemplateId,
                        principalSchema: "GameRules",
                        principalTable: "BodyTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CreatureTemplates",
                schema: "GameRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    GameId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди игры"),
                    ImgFileId = table.Column<Guid>(type: "uuid", nullable: true, comment: "Айди графического файла"),
                    BodyTemplateId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди шаблона тела"),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Название шаблона"),
                    Description = table.Column<string>(type: "text", nullable: true, comment: "Описание шаблона"),
                    Type = table.Column<string>(type: "text", nullable: false, comment: "Тип шаблона существа"),
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
                name: "Abilities",
                schema: "GameRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    CreatureTemplateId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди шаблона существа"),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Название способности"),
                    Description = table.Column<string>(type: "text", nullable: true, comment: "Описание способности"),
                    AttackParameterId = table.Column<Guid>(type: "uuid", nullable: false),
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
                        name: "FK_Abilities_CreatureTemplates_CreatureTemplateId",
                        column: x => x.CreatureTemplateId,
                        principalSchema: "GameRules",
                        principalTable: "CreatureTemplates",
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
                    Type = table.Column<string>(type: "text", nullable: false, comment: "Тип шаблона существа"),
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
                name: "CreatureTemplateParameters",
                schema: "GameInstance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    CreatureId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди шаблона существа"),
                    ParameterId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди параметра"),
                    ParametrValue = table.Column<double>(type: "double precision", nullable: false, comment: "Значение параметра"),
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
                name: "CreatureTemplates_BodyParts",
                schema: "GameRules",
                columns: table => new
                {
                    CreatureTemplateId = table.Column<Guid>(type: "uuid", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StartingArmor = table.Column<int>(type: "integer", nullable: false, comment: "Начальная броня"),
                    CurrentArmor = table.Column<int>(type: "integer", nullable: false, comment: "Текущая броня"),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Название"),
                    DamageModifer = table.Column<double>(type: "double precision", nullable: false, comment: "Модификатор урона"),
                    MinToHit = table.Column<int>(type: "integer", nullable: false, comment: "Минимальное значение попадания"),
                    MaxToHit = table.Column<int>(type: "integer", nullable: false, comment: "Максимальное значение попадания")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreatureTemplates_BodyParts", x => new { x.CreatureTemplateId, x.Id });
                    table.ForeignKey(
                        name: "FK_CreatureTemplates_BodyParts_CreatureTemplates_CreatureTempl~",
                        column: x => x.CreatureTemplateId,
                        principalSchema: "GameRules",
                        principalTable: "CreatureTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppliedConditions",
                schema: "GameRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    AbilityId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди способнности"),
                    ApplyChance = table.Column<double>(type: "double precision", nullable: false, comment: "Шанс применения"),
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
                name: "CreatureParameters",
                schema: "GameInstance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    CreatureId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди существа"),
                    ParameterId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди параметра"),
                    ParametrValue = table.Column<double>(type: "double precision", nullable: false, comment: "Значение параметра"),
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
                name: "Creatures_BodyParts",
                schema: "GameInstance",
                columns: table => new
                {
                    CreatureId = table.Column<Guid>(type: "uuid", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StartingArmor = table.Column<int>(type: "integer", nullable: false, comment: "Начальная броня"),
                    CurrentArmor = table.Column<int>(type: "integer", nullable: false, comment: "Текущая броня"),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Название"),
                    DamageModifer = table.Column<double>(type: "double precision", nullable: false, comment: "Модификатор урона"),
                    MinToHit = table.Column<int>(type: "integer", nullable: false, comment: "Минимальное значение попадания"),
                    MaxToHit = table.Column<int>(type: "integer", nullable: false, comment: "Максимальное значение попадания")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Creatures_BodyParts", x => new { x.CreatureId, x.Id });
                    table.ForeignKey(
                        name: "FK_Creatures_BodyParts_Creatures_CreatureId",
                        column: x => x.CreatureId,
                        principalSchema: "GameInstance",
                        principalTable: "Creatures",
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

            migrationBuilder.CreateIndex(
                name: "IX_Abilities_AttackParameterId",
                schema: "GameRules",
                table: "Abilities",
                column: "AttackParameterId");

            migrationBuilder.CreateIndex(
                name: "IX_Abilities_CreatureTemplateId",
                schema: "GameRules",
                table: "Abilities",
                column: "CreatureTemplateId");

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
                name: "IX_CreatureTemplates_BodyTemplateId",
                schema: "GameRules",
                table: "CreatureTemplates",
                column: "BodyTemplateId");

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
                name: "IX_CurrentConditions_CreaturesId",
                schema: "GameInstance",
                table: "CurrentConditions",
                column: "CreaturesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppliedConditions",
                schema: "GameRules");

            migrationBuilder.DropTable(
                name: "BodyTemplatePart",
                schema: "GameRules");

            migrationBuilder.DropTable(
                name: "CreatureAbilities",
                schema: "GameInstance");

            migrationBuilder.DropTable(
                name: "CreatureParameters",
                schema: "GameInstance");

            migrationBuilder.DropTable(
                name: "Creatures_BodyParts",
                schema: "GameInstance");

            migrationBuilder.DropTable(
                name: "CreatureTemplateParameters",
                schema: "GameInstance");

            migrationBuilder.DropTable(
                name: "CreatureTemplates_BodyParts",
                schema: "GameRules");

            migrationBuilder.DropTable(
                name: "CurrentConditions",
                schema: "GameInstance");

            migrationBuilder.DropTable(
                name: "Abilities",
                schema: "GameRules");

            migrationBuilder.DropTable(
                name: "Conditions",
                schema: "GameRules");

            migrationBuilder.DropTable(
                name: "Creatures",
                schema: "GameInstance");

            migrationBuilder.DropTable(
                name: "CreatureTemplates",
                schema: "GameRules");

            migrationBuilder.DropTable(
                name: "BodyTemplates",
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
