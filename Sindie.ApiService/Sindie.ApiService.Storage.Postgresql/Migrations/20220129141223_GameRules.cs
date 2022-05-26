using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sindie.ApiService.Storage.Postgresql.Migrations
{
    public partial class GameRules : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "Slots");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Slots");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Slots");

            migrationBuilder.DropColumn(
                name: "ModifiedByUserId",
                table: "Slots");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "Slots");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Slots");

            migrationBuilder.DropColumn(
                name: "RoleCreatedUser",
                table: "Slots");

            migrationBuilder.DropColumn(
                name: "RoleModifiedUser",
                table: "Slots");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "Parameters");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Parameters");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Parameters");

            migrationBuilder.DropColumn(
                name: "ModifiedByUserId",
                table: "Parameters");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "Parameters");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Parameters");

            migrationBuilder.DropColumn(
                name: "RoleCreatedUser",
                table: "Parameters");

            migrationBuilder.DropColumn(
                name: "RoleModifiedUser",
                table: "Parameters");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "Modifiers");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Modifiers");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Modifiers");

            migrationBuilder.DropColumn(
                name: "ModifiedByUserId",
                table: "Modifiers");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "Modifiers");

            migrationBuilder.DropColumn(
                name: "ModifierActivity_ModifierActivation_ActivationFromStartGame",
                table: "Modifiers");

            migrationBuilder.DropColumn(
                name: "ModifierActivity_ModifierActivation_ActivationFromWearingInSlot",
                table: "Modifiers");

            migrationBuilder.DropColumn(
                name: "ModifierActivity_ModifierActivation_ActivationTime",
                table: "Modifiers");

            migrationBuilder.DropColumn(
                name: "ModifierActivity_Periodicity_NumberOfRepetitions",
                table: "Modifiers");

            migrationBuilder.DropColumn(
                name: "ModifierActivity_Periodicity_PeriodOfActivity",
                table: "Modifiers");

            migrationBuilder.DropColumn(
                name: "ModifierActivity_Periodicity_PeriodOfInactivity",
                table: "Modifiers");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Modifiers");

            migrationBuilder.DropColumn(
                name: "RoleCreatedUser",
                table: "Modifiers");

            migrationBuilder.DropColumn(
                name: "RoleModifiedUser",
                table: "Modifiers");

            migrationBuilder.DropColumn(
                name: "AutoWear",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "IsRemovable",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "MaxQuantityItem",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "ModifiedByUserId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "RoleCreatedUser",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "RoleModifiedUser",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "Items");

            migrationBuilder.EnsureSchema(
                name: "GameRules");

            migrationBuilder.RenameTable(
                name: "Slots",
                newName: "Slots",
                newSchema: "GameRules");

            migrationBuilder.RenameTable(
                name: "Parameters",
                newName: "Parameters",
                newSchema: "GameRules");

            migrationBuilder.RenameTable(
                name: "Modifiers",
                newName: "Modifiers",
                newSchema: "GameRules");

            migrationBuilder.RenameTable(
                name: "Items",
                newName: "Items",
                newSchema: "GameRules");

            migrationBuilder.RenameColumn(
                name: "ParameterBounds_MinValueParameter",
                schema: "GameRules",
                table: "Parameters",
                newName: "MinValueParameters");

            migrationBuilder.RenameColumn(
                name: "ParameterBounds_MaxValueParameter",
                schema: "GameRules",
                table: "Parameters",
                newName: "MaxValueParameters");

            migrationBuilder.AlterTable(
                name: "Slots",
                schema: "GameRules",
                comment: "Слоты");

            migrationBuilder.AlterTable(
                name: "Parameters",
                schema: "GameRules",
                comment: "Параметры");

            migrationBuilder.AlterTable(
                name: "Modifiers",
                schema: "GameRules",
                comment: "Модификаторы");

            migrationBuilder.AlterTable(
                name: "Items",
                schema: "GameRules",
                comment: "Предметы");

            migrationBuilder.AlterColumn<Guid>(
                name: "GameId",
                schema: "GameRules",
                table: "Slots",
                type: "uuid",
                nullable: false,
                comment: "Айди игры",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "GameId",
                schema: "GameRules",
                table: "Parameters",
                type: "uuid",
                nullable: false,
                comment: "Айди игры",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<double>(
                name: "MinValueParameters",
                schema: "GameRules",
                table: "Parameters",
                type: "double precision",
                nullable: true,
                comment: "Минимальные значения параметра",
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "MaxValueParameters",
                schema: "GameRules",
                table: "Parameters",
                type: "double precision",
                nullable: true,
                comment: "Максимальные значения параметра",
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "GameId",
                schema: "GameRules",
                table: "Modifiers",
                type: "uuid",
                nullable: false,
                comment: "Айди игры",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "SlotId",
                schema: "GameRules",
                table: "Items",
                type: "uuid",
                nullable: false,
                comment: "Айди слота",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "ItemTemplateId",
                schema: "GameRules",
                table: "Items",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "Айди шаблона предмета");

            migrationBuilder.AddColumn<Guid>(
                name: "ScriptId",
                schema: "GameRules",
                table: "Items",
                type: "uuid",
                nullable: true,
                comment: "Айди скрипта");

            migrationBuilder.CreateTable(
                name: "CharacterTemplates",
                schema: "GameRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    GameId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди игры"),
                    ImgFileId = table.Column<Guid>(type: "uuid", nullable: true, comment: "Айди графического файла(аватарки)"),
                    InterfaceId = table.Column<Guid>(type: "uuid", nullable: true, comment: "Айди интерфейса"),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Название шаблона"),
                    Description = table.Column<string>(type: "text", nullable: true, comment: "Описание шаблона"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CharacterTemplates_Games_GameId",
                        column: x => x.GameId,
                        principalSchema: "BaseGame",
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterTemplates_ImgFiles_ImgFileId",
                        column: x => x.ImgFileId,
                        principalSchema: "System",
                        principalTable: "ImgFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_CharacterTemplates_Interfaces_InterfaceId",
                        column: x => x.InterfaceId,
                        principalSchema: "System",
                        principalTable: "Interfaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Шаблоны персонажей");

            migrationBuilder.CreateTable(
                name: "ItemTemplates",
                schema: "GameRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    GameId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди игры"),
                    ImgFileId = table.Column<Guid>(type: "uuid", nullable: true, comment: "Айди графического файла"),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Название"),
                    Description = table.Column<string>(type: "text", nullable: true, comment: "Описание"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
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
                    table.ForeignKey(
                        name: "FK_ItemTemplates_ImgFiles_ImgFileId",
                        column: x => x.ImgFileId,
                        principalSchema: "System",
                        principalTable: "ImgFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                },
                comment: "Шаблоны предметов");

            migrationBuilder.CreateTable(
                name: "Prerequisites",
                schema: "GameRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    ImgFileId = table.Column<Guid>(type: "uuid", nullable: true, comment: "Айди графического файла пререквизита"),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Название"),
                    Description = table.Column<string>(type: "text", nullable: true, comment: "Описание"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prerequisites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Prerequisites_ImgFiles_ImgFileId",
                        column: x => x.ImgFileId,
                        principalSchema: "System",
                        principalTable: "ImgFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                },
                comment: "Пререквизиты");

            migrationBuilder.CreateTable(
                name: "Scripts",
                schema: "GameRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    GameId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди игры"),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Название"),
                    Description = table.Column<string>(type: "text", nullable: true, comment: "Описание"),
                    BodyScript = table.Column<string>(type: "text", nullable: false, comment: "Тело скрипта"),
                    IsValid = table.Column<bool>(type: "boolean", nullable: false, comment: "Валидность скрипта"),
                    ValidationText = table.Column<string>(type: "text", nullable: true, comment: "Валидационный текст"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scripts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Scripts_Games_GameId",
                        column: x => x.GameId,
                        principalSchema: "BaseGame",
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Скрипты");

            migrationBuilder.CreateTable(
                name: "CharacterTemplateModifiers",
                schema: "GameRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    ModifierId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди модификатора"),
                    CharacterTemplateId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди шаблона персонажа"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterTemplateModifiers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CharacterTemplateModifiers_CharacterTemplates_CharacterTemp~",
                        column: x => x.CharacterTemplateId,
                        principalSchema: "GameRules",
                        principalTable: "CharacterTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterTemplateModifiers_Modifiers_ModifierId",
                        column: x => x.ModifierId,
                        principalSchema: "GameRules",
                        principalTable: "Modifiers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Модификаторы шаблонов персонажей");

            migrationBuilder.CreateTable(
                name: "CharacterTemplateSlots",
                schema: "GameRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    CharacterTemplateId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди шаблона персонажа"),
                    SlotId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди слота"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterTemplateSlots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CharacterTemplateSlots_CharacterTemplates_CharacterTemplate~",
                        column: x => x.CharacterTemplateId,
                        principalSchema: "GameRules",
                        principalTable: "CharacterTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterTemplateSlots_Slots_SlotId",
                        column: x => x.SlotId,
                        principalSchema: "GameRules",
                        principalTable: "Slots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Слоты шаблона персонажа");

            migrationBuilder.CreateTable(
                name: "ItemTemplateModifiers",
                schema: "GameRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    ModifierId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди модификатора"),
                    ItemTemplateId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди шаблона предмета"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemTemplateModifiers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemTemplateModifiers_ItemTemplates_ItemTemplateId",
                        column: x => x.ItemTemplateId,
                        principalSchema: "GameRules",
                        principalTable: "ItemTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemTemplateModifiers_Modifiers_ModifierId",
                        column: x => x.ModifierId,
                        principalSchema: "GameRules",
                        principalTable: "Modifiers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Модификаторы шаблонов предметов");

            migrationBuilder.CreateTable(
                name: "Events",
                schema: "GameRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    GameId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди игры"),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, comment: "Событие активно")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Events_Games_GameId",
                        column: x => x.GameId,
                        principalSchema: "BaseGame",
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Events_Prerequisites_Id",
                        column: x => x.Id,
                        principalSchema: "GameRules",
                        principalTable: "Prerequisites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "События");

            migrationBuilder.CreateTable(
                name: "ScriptPrerequisites",
                schema: "GameRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    ScriptId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди скрипта"),
                    PrerequisiteId = table.Column<Guid>(type: "uuid", nullable: true, comment: "Айди пререквизита"),
                    IsValid = table.Column<bool>(type: "boolean", nullable: false, comment: "Валидность скрипта"),
                    ValidationText = table.Column<string>(type: "text", nullable: true, comment: "Валидационный текст"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScriptPrerequisites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScriptPrerequisites_Prerequisites_PrerequisiteId",
                        column: x => x.PrerequisiteId,
                        principalSchema: "GameRules",
                        principalTable: "Prerequisites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ScriptPrerequisites_Scripts_ScriptId",
                        column: x => x.ScriptId,
                        principalSchema: "GameRules",
                        principalTable: "Scripts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Пререквизиты скрипта");

            migrationBuilder.CreateTable(
                name: "ModifierScripts",
                schema: "GameRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    EventId = table.Column<Guid>(type: "uuid", nullable: true, comment: "Айди события"),
                    ModifierId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди модификатора"),
                    ScriptId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди скрипта"),
                    ActivationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, comment: "Время активации скрипта модификатора"),
                    PeriodOfActivity = table.Column<int>(type: "integer", nullable: true, comment: "Период активности скрипта модификатора"),
                    PeriodOfInactivity = table.Column<int>(type: "integer", nullable: true, comment: "Период неактивности скрипта модификатора"),
                    NumberOfRepetitions = table.Column<int>(type: "integer", nullable: true, comment: "Количество повторений скрипта модификатора"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModifierScripts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModifierScripts_Events_EventId",
                        column: x => x.EventId,
                        principalSchema: "GameRules",
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ModifierScripts_Modifiers_ModifierId",
                        column: x => x.ModifierId,
                        principalSchema: "GameRules",
                        principalTable: "Modifiers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ModifierScripts_Scripts_ScriptId",
                        column: x => x.ScriptId,
                        principalSchema: "GameRules",
                        principalTable: "Scripts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Скрипты модифкатора");

            migrationBuilder.CreateIndex(
                name: "IX_Items_ItemTemplateId",
                schema: "GameRules",
                table: "Items",
                column: "ItemTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_ScriptId",
                schema: "GameRules",
                table: "Items",
                column: "ScriptId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterTemplateModifiers_CharacterTemplateId",
                schema: "GameRules",
                table: "CharacterTemplateModifiers",
                column: "CharacterTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterTemplateModifiers_ModifierId",
                schema: "GameRules",
                table: "CharacterTemplateModifiers",
                column: "ModifierId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterTemplates_GameId",
                schema: "GameRules",
                table: "CharacterTemplates",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterTemplates_ImgFileId",
                schema: "GameRules",
                table: "CharacterTemplates",
                column: "ImgFileId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CharacterTemplates_InterfaceId",
                schema: "GameRules",
                table: "CharacterTemplates",
                column: "InterfaceId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterTemplates_Name",
                schema: "GameRules",
                table: "CharacterTemplates",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CharacterTemplateSlots_CharacterTemplateId",
                schema: "GameRules",
                table: "CharacterTemplateSlots",
                column: "CharacterTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterTemplateSlots_SlotId",
                schema: "GameRules",
                table: "CharacterTemplateSlots",
                column: "SlotId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_GameId",
                schema: "GameRules",
                table: "Events",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemTemplateModifiers_ItemTemplateId",
                schema: "GameRules",
                table: "ItemTemplateModifiers",
                column: "ItemTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemTemplateModifiers_ModifierId",
                schema: "GameRules",
                table: "ItemTemplateModifiers",
                column: "ModifierId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemTemplates_GameId",
                schema: "GameRules",
                table: "ItemTemplates",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemTemplates_ImgFileId",
                schema: "GameRules",
                table: "ItemTemplates",
                column: "ImgFileId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItemTemplates_Name",
                schema: "GameRules",
                table: "ItemTemplates",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ModifierScripts_EventId",
                schema: "GameRules",
                table: "ModifierScripts",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_ModifierScripts_ModifierId",
                schema: "GameRules",
                table: "ModifierScripts",
                column: "ModifierId");

            migrationBuilder.CreateIndex(
                name: "IX_ModifierScripts_ScriptId",
                schema: "GameRules",
                table: "ModifierScripts",
                column: "ScriptId");

            migrationBuilder.CreateIndex(
                name: "IX_Prerequisites_ImgFileId",
                schema: "GameRules",
                table: "Prerequisites",
                column: "ImgFileId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Prerequisites_Name",
                schema: "GameRules",
                table: "Prerequisites",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ScriptPrerequisites_PrerequisiteId",
                schema: "GameRules",
                table: "ScriptPrerequisites",
                column: "PrerequisiteId");

            migrationBuilder.CreateIndex(
                name: "IX_ScriptPrerequisites_ScriptId",
                schema: "GameRules",
                table: "ScriptPrerequisites",
                column: "ScriptId");

            migrationBuilder.CreateIndex(
                name: "IX_Scripts_GameId",
                schema: "GameRules",
                table: "Scripts",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Scripts_Name",
                schema: "GameRules",
                table: "Scripts",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_ItemTemplates_ItemTemplateId",
                schema: "GameRules",
                table: "Items",
                column: "ItemTemplateId",
                principalSchema: "GameRules",
                principalTable: "ItemTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Prerequisites_Id",
                schema: "GameRules",
                table: "Items",
                column: "Id",
                principalSchema: "GameRules",
                principalTable: "Prerequisites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Scripts_ScriptId",
                schema: "GameRules",
                table: "Items",
                column: "ScriptId",
                principalSchema: "GameRules",
                principalTable: "Scripts",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Modifiers_Prerequisites_Id",
                schema: "GameRules",
                table: "Modifiers",
                column: "Id",
                principalSchema: "GameRules",
                principalTable: "Prerequisites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Parameters_Prerequisites_Id",
                schema: "GameRules",
                table: "Parameters",
                column: "Id",
                principalSchema: "GameRules",
                principalTable: "Prerequisites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Slots_Prerequisites_Id",
                schema: "GameRules",
                table: "Slots",
                column: "Id",
                principalSchema: "GameRules",
                principalTable: "Prerequisites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_ItemTemplates_ItemTemplateId",
                schema: "GameRules",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_Prerequisites_Id",
                schema: "GameRules",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_Scripts_ScriptId",
                schema: "GameRules",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Modifiers_Prerequisites_Id",
                schema: "GameRules",
                table: "Modifiers");

            migrationBuilder.DropForeignKey(
                name: "FK_Parameters_Prerequisites_Id",
                schema: "GameRules",
                table: "Parameters");

            migrationBuilder.DropForeignKey(
                name: "FK_Slots_Prerequisites_Id",
                schema: "GameRules",
                table: "Slots");

            migrationBuilder.DropTable(
                name: "CharacterTemplateModifiers",
                schema: "GameRules");

            migrationBuilder.DropTable(
                name: "CharacterTemplateSlots",
                schema: "GameRules");

            migrationBuilder.DropTable(
                name: "ItemTemplateModifiers",
                schema: "GameRules");

            migrationBuilder.DropTable(
                name: "ModifierScripts",
                schema: "GameRules");

            migrationBuilder.DropTable(
                name: "ScriptPrerequisites",
                schema: "GameRules");

            migrationBuilder.DropTable(
                name: "CharacterTemplates",
                schema: "GameRules");

            migrationBuilder.DropTable(
                name: "ItemTemplates",
                schema: "GameRules");

            migrationBuilder.DropTable(
                name: "Events",
                schema: "GameRules");

            migrationBuilder.DropTable(
                name: "Scripts",
                schema: "GameRules");

            migrationBuilder.DropTable(
                name: "Prerequisites",
                schema: "GameRules");

            migrationBuilder.DropIndex(
                name: "IX_Items_ItemTemplateId",
                schema: "GameRules",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_ScriptId",
                schema: "GameRules",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "ItemTemplateId",
                schema: "GameRules",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "ScriptId",
                schema: "GameRules",
                table: "Items");

            migrationBuilder.RenameTable(
                name: "Slots",
                schema: "GameRules",
                newName: "Slots");

            migrationBuilder.RenameTable(
                name: "Parameters",
                schema: "GameRules",
                newName: "Parameters");

            migrationBuilder.RenameTable(
                name: "Modifiers",
                schema: "GameRules",
                newName: "Modifiers");

            migrationBuilder.RenameTable(
                name: "Items",
                schema: "GameRules",
                newName: "Items");

            migrationBuilder.RenameColumn(
                name: "MinValueParameters",
                table: "Parameters",
                newName: "ParameterBounds_MinValueParameter");

            migrationBuilder.RenameColumn(
                name: "MaxValueParameters",
                table: "Parameters",
                newName: "ParameterBounds_MaxValueParameter");

            migrationBuilder.AlterTable(
                name: "Slots",
                oldComment: "Слоты");

            migrationBuilder.AlterTable(
                name: "Parameters",
                oldComment: "Параметры");

            migrationBuilder.AlterTable(
                name: "Modifiers",
                oldComment: "Модификаторы");

            migrationBuilder.AlterTable(
                name: "Items",
                oldComment: "Предметы");

            migrationBuilder.AlterColumn<Guid>(
                name: "GameId",
                table: "Slots",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "Айди игры");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedByUserId",
                table: "Slots",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Slots",
                type: "timestamp without time zone",
                nullable: false,
                defaultValueSql: "now() at time zone 'utc'");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Slots",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedByUserId",
                table: "Slots",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "Slots",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Slots",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RoleCreatedUser",
                table: "Slots",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoleModifiedUser",
                table: "Slots",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "GameId",
                table: "Parameters",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "Айди игры");

            migrationBuilder.AlterColumn<double>(
                name: "ParameterBounds_MinValueParameter",
                table: "Parameters",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true,
                oldComment: "Минимальные значения параметра");

            migrationBuilder.AlterColumn<double>(
                name: "ParameterBounds_MaxValueParameter",
                table: "Parameters",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true,
                oldComment: "Максимальные значения параметра");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedByUserId",
                table: "Parameters",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Parameters",
                type: "timestamp without time zone",
                nullable: false,
                defaultValueSql: "now() at time zone 'utc'");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Parameters",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedByUserId",
                table: "Parameters",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "Parameters",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Parameters",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RoleCreatedUser",
                table: "Parameters",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoleModifiedUser",
                table: "Parameters",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "GameId",
                table: "Modifiers",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "Айди игры");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedByUserId",
                table: "Modifiers",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Modifiers",
                type: "timestamp without time zone",
                nullable: false,
                defaultValueSql: "now() at time zone 'utc'");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Modifiers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedByUserId",
                table: "Modifiers",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "Modifiers",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "ModifierActivity_ModifierActivation_ActivationFromStartGame",
                table: "Modifiers",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ModifierActivity_ModifierActivation_ActivationFromWearingInSlot",
                table: "Modifiers",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifierActivity_ModifierActivation_ActivationTime",
                table: "Modifiers",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifierActivity_Periodicity_NumberOfRepetitions",
                table: "Modifiers",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifierActivity_Periodicity_PeriodOfActivity",
                table: "Modifiers",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifierActivity_Periodicity_PeriodOfInactivity",
                table: "Modifiers",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Modifiers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoleCreatedUser",
                table: "Modifiers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoleModifiedUser",
                table: "Modifiers",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "SlotId",
                table: "Items",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "Айди слота");

            migrationBuilder.AddColumn<bool>(
                name: "AutoWear",
                table: "Items",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedByUserId",
                table: "Items",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Items",
                type: "timestamp without time zone",
                nullable: false,
                defaultValueSql: "now() at time zone 'utc'");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Items",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsRemovable",
                table: "Items",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaxQuantityItem",
                table: "Items",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedByUserId",
                table: "Items",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "Items",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Items",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RoleCreatedUser",
                table: "Items",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoleModifiedUser",
                table: "Items",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Weight",
                table: "Items",
                type: "double precision",
                nullable: true);
        }
    }
}
