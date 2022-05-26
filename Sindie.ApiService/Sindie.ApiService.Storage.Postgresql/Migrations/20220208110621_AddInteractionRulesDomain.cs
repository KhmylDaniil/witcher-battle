using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sindie.ApiService.Storage.Postgresql.Migrations
{
    public partial class AddInteractionRulesDomain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CharacterCharacteristic_Characteristic_CharacteristicId",
                table: "CharacterCharacteristic");

            migrationBuilder.DropForeignKey(
                name: "FK_Characteristic_Games_GameId",
                table: "Characteristic");

            migrationBuilder.DropForeignKey(
                name: "FK_CharacteristicModifier_Characteristic_CharacteristicId",
                table: "CharacteristicModifier");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Roles_RoleId",
                schema: "System",
                table: "UserRoles");

            migrationBuilder.DropTable(
                name: "Roles",
                schema: "System");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Characteristic",
                table: "Characteristic");

            migrationBuilder.DropIndex(
                name: "IX_Characteristic_GameId",
                table: "Characteristic");

            migrationBuilder.DropColumn(
                name: "DateOfGame",
                schema: "BaseGame",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "StoryAboutRules",
                schema: "BaseGame",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "Characteristic");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Characteristic");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Characteristic");

            migrationBuilder.DropColumn(
                name: "GameId",
                table: "Characteristic");

            migrationBuilder.DropColumn(
                name: "ModifiedByUserId",
                table: "Characteristic");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "Characteristic");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Characteristic");

            migrationBuilder.DropColumn(
                name: "RoleCreatedUser",
                table: "Characteristic");

            migrationBuilder.DropColumn(
                name: "RoleModifiedUser",
                table: "Characteristic");

            migrationBuilder.EnsureSchema(
                name: "InteractionRules");

            migrationBuilder.RenameTable(
                name: "Characteristic",
                newName: "Characteristics",
                newSchema: "InteractionRules");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                schema: "System",
                table: "UserRoles",
                newName: "SystemRoleId");

            migrationBuilder.RenameIndex(
                name: "IX_UserRoles_RoleId",
                schema: "System",
                table: "UserRoles",
                newName: "IX_UserRoles_SystemRoleId");

            migrationBuilder.AlterTable(
                name: "Characteristics",
                schema: "InteractionRules",
                comment: "Характеристики");

            migrationBuilder.AddColumn<Guid>(
                name: "InteractionId",
                schema: "InteractionRules",
                table: "Characteristics",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "Айди взаимодействия");

            migrationBuilder.AddColumn<int>(
                name: "MaxCondition",
                schema: "InteractionRules",
                table: "Characteristics",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "Максимальное значение");

            migrationBuilder.AddColumn<int>(
                name: "MinCondition",
                schema: "InteractionRules",
                table: "Characteristics",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "Минимальное значение");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Characteristics",
                schema: "InteractionRules",
                table: "Characteristics",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ApplicationAreas",
                schema: "InteractionRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Название области применения"),
                    Description = table.Column<string>(type: "text", nullable: true, comment: "Описание описание области применения"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationAreas", x => x.Id);
                },
                comment: "Области применения");

            migrationBuilder.CreateTable(
                name: "Interactions",
                schema: "InteractionRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    GameId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди игры"),
                    ImgFileId = table.Column<Guid>(type: "uuid", nullable: true, comment: "Айди графического файла"),
                    TextFileId = table.Column<Guid>(type: "uuid", nullable: true, comment: "Айди текстового файла"),
                    ScenarioReturnId = table.Column<Guid>(type: "uuid", nullable: true, comment: "Айди сценария завершения взаимодействия"),
                    ScenarioVictoryId = table.Column<Guid>(type: "uuid", nullable: true, comment: "Айди сценария победы во взаимодействии"),
                    ScenarioLootId = table.Column<Guid>(type: "uuid", nullable: true, comment: "Айди сценария лута во взаимодействии"),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Название взаимодействия"),
                    Description = table.Column<string>(type: "text", nullable: true, comment: "Описание описание взаимодействия"),
                    CanGiveUp = table.Column<bool>(type: "boolean", nullable: false, comment: "Можно ли выйти из взаимодействия"),
                    RoundLimit = table.Column<int>(type: "integer", nullable: false, comment: "Максимальное количество раундов взаимодействия"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Interactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Interactions_Games_GameId",
                        column: x => x.GameId,
                        principalSchema: "BaseGame",
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Interactions_ImgFiles_ImgFileId",
                        column: x => x.ImgFileId,
                        principalSchema: "System",
                        principalTable: "ImgFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Interactions_Scripts_ScenarioLootId",
                        column: x => x.ScenarioLootId,
                        principalSchema: "GameRules",
                        principalTable: "Scripts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Interactions_Scripts_ScenarioReturnId",
                        column: x => x.ScenarioReturnId,
                        principalSchema: "GameRules",
                        principalTable: "Scripts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Interactions_Scripts_ScenarioVictoryId",
                        column: x => x.ScenarioVictoryId,
                        principalSchema: "GameRules",
                        principalTable: "Scripts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Interactions_TextFiles_TextFileId",
                        column: x => x.TextFileId,
                        principalSchema: "System",
                        principalTable: "TextFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                },
                comment: "Взаимодействия");

            migrationBuilder.CreateTable(
                name: "SystemRoles",
                schema: "System",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Роль в системе"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemRoles", x => x.Id);
                },
                comment: "Роли в системе");

            migrationBuilder.CreateTable(
                name: "Actions",
                schema: "InteractionRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    InteractionId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди взаимодействия для деятельности"),
                    ScenarioActionId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди сценария действия во взаимодействии"),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Название действия"),
                    Description = table.Column<string>(type: "text", nullable: true, comment: "Описание действия"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Actions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Actions_Interactions_InteractionId",
                        column: x => x.InteractionId,
                        principalSchema: "InteractionRules",
                        principalTable: "Interactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Actions_Scripts_ScenarioActionId",
                        column: x => x.ScenarioActionId,
                        principalSchema: "GameRules",
                        principalTable: "Scripts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Действия");

            migrationBuilder.CreateTable(
                name: "Activities",
                schema: "InteractionRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    InteractionId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди взаимодействия для деятельности"),
                    ImgFileId = table.Column<Guid>(type: "uuid", nullable: true, comment: "Айди графического файла для деятельности"),
                    ApplicationAreaId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди области применения для деятельности"),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Название деятельности"),
                    Description = table.Column<string>(type: "text", nullable: true, comment: "Описание деятельности"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Activities_ApplicationAreas_ApplicationAreaId",
                        column: x => x.ApplicationAreaId,
                        principalSchema: "InteractionRules",
                        principalTable: "ApplicationAreas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Activities_ImgFiles_ImgFileId",
                        column: x => x.ImgFileId,
                        principalSchema: "System",
                        principalTable: "ImgFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Activities_Interactions_InteractionId",
                        column: x => x.InteractionId,
                        principalSchema: "InteractionRules",
                        principalTable: "Interactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Деятельности");

            migrationBuilder.CreateTable(
                name: "InteractionsRoles",
                schema: "InteractionRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    InteractionId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди взаимодействия для роли"),
                    ImgFileId = table.Column<Guid>(type: "uuid", nullable: true, comment: "Айди графического файла для роли"),
                    TextFileId = table.Column<Guid>(type: "uuid", nullable: true, comment: "Айди текстового файла для роли"),
                    ScenarioReturnId = table.Column<Guid>(type: "uuid", nullable: true, comment: "Айди сценария завершения взаимодействия для роли"),
                    ScenarioPrerequisites = table.Column<Guid>(type: "uuid", nullable: false, comment: "Сценарий пререквизитов к роли"),
                    ScenarioCharacteristicsId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Сценарий характеристик роли"),
                    ScenarioInitiativeId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Сценарий инициативы роли"),
                    ScenarioVictoryId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди сценария победы во взаимодействии для роли"),
                    ScenarioLootId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди сценария лута во взаимодействии для роли"),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Название роли"),
                    Description = table.Column<string>(type: "text", nullable: true, comment: "Описание роли"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InteractionsRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InteractionsRoles_ImgFiles_ImgFileId",
                        column: x => x.ImgFileId,
                        principalSchema: "System",
                        principalTable: "ImgFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_InteractionsRoles_Interactions_InteractionId",
                        column: x => x.InteractionId,
                        principalSchema: "InteractionRules",
                        principalTable: "Interactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InteractionsRoles_Scripts_ScenarioCharacteristicsId",
                        column: x => x.ScenarioCharacteristicsId,
                        principalSchema: "GameRules",
                        principalTable: "Scripts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InteractionsRoles_Scripts_ScenarioInitiativeId",
                        column: x => x.ScenarioInitiativeId,
                        principalSchema: "GameRules",
                        principalTable: "Scripts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InteractionsRoles_Scripts_ScenarioLootId",
                        column: x => x.ScenarioLootId,
                        principalSchema: "GameRules",
                        principalTable: "Scripts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InteractionsRoles_Scripts_ScenarioPrerequisites",
                        column: x => x.ScenarioPrerequisites,
                        principalSchema: "GameRules",
                        principalTable: "Scripts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InteractionsRoles_Scripts_ScenarioReturnId",
                        column: x => x.ScenarioReturnId,
                        principalSchema: "GameRules",
                        principalTable: "Scripts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_InteractionsRoles_Scripts_ScenarioVictoryId",
                        column: x => x.ScenarioVictoryId,
                        principalSchema: "GameRules",
                        principalTable: "Scripts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InteractionsRoles_TextFiles_TextFileId",
                        column: x => x.TextFileId,
                        principalSchema: "System",
                        principalTable: "TextFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                },
                comment: "Роли во взаимодействии");

            migrationBuilder.CreateTable(
                name: "Parties",
                schema: "InteractionRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    InteractionId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди взаимодействия для стороны"),
                    ImgFileId = table.Column<Guid>(type: "uuid", nullable: true, comment: "Айди графического файла для стороны"),
                    TextFileId = table.Column<Guid>(type: "uuid", nullable: true, comment: "Айди текстового файла для стороны"),
                    ScenarioReturnId = table.Column<Guid>(type: "uuid", nullable: true, comment: "Айди сценария завершения взаимодействия для стороны"),
                    ScenarioVictoryId = table.Column<Guid>(type: "uuid", nullable: true, comment: "Айди сценария победы во взаимодействии для стороны"),
                    ScenarioLootId = table.Column<Guid>(type: "uuid", nullable: true, comment: "Айди сценария лута во взаимодействии для стороны"),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Название стороны"),
                    Description = table.Column<string>(type: "text", nullable: true, comment: "Описание стороны"),
                    MinQuantityCharacters = table.Column<int>(type: "integer", nullable: false, comment: "Минимальное количество персонажей"),
                    MaxQuantityCharacters = table.Column<int>(type: "integer", nullable: false, comment: "Максимальное количество персонажей"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Parties_ImgFiles_ImgFileId",
                        column: x => x.ImgFileId,
                        principalSchema: "System",
                        principalTable: "ImgFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Parties_Interactions_InteractionId",
                        column: x => x.InteractionId,
                        principalSchema: "InteractionRules",
                        principalTable: "Interactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Parties_Scripts_ScenarioLootId",
                        column: x => x.ScenarioLootId,
                        principalSchema: "GameRules",
                        principalTable: "Scripts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Parties_Scripts_ScenarioReturnId",
                        column: x => x.ScenarioReturnId,
                        principalSchema: "GameRules",
                        principalTable: "Scripts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Parties_Scripts_ScenarioVictoryId",
                        column: x => x.ScenarioVictoryId,
                        principalSchema: "GameRules",
                        principalTable: "Scripts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Parties_TextFiles_TextFileId",
                        column: x => x.TextFileId,
                        principalSchema: "System",
                        principalTable: "TextFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                },
                comment: "Стороны");

            migrationBuilder.CreateTable(
                name: "ActivityActions",
                schema: "InteractionRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    ActivityId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди деятельности"),
                    ActionId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди действия"),
                    Order = table.Column<int>(type: "integer", nullable: false, comment: "Порядок действий"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityActions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivityActions_Actions_ActionId",
                        column: x => x.ActionId,
                        principalSchema: "InteractionRules",
                        principalTable: "Actions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActivityActions_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalSchema: "InteractionRules",
                        principalTable: "Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Действия деятельности");

            migrationBuilder.CreateTable(
                name: "InteractionItems",
                schema: "InteractionRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    InteractionsRoleId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди роли взаимодействия"),
                    ActivityId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди деятельности во взаимодействии"),
                    ItemId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди предмета во взаимодействии"),
                    ExpendQuantity = table.Column<int>(type: "integer", nullable: false, comment: "Максимальное количество применений"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InteractionItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InteractionItems_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalSchema: "InteractionRules",
                        principalTable: "Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InteractionItems_InteractionsRoles_InteractionsRoleId",
                        column: x => x.InteractionsRoleId,
                        principalSchema: "InteractionRules",
                        principalTable: "InteractionsRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InteractionItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalSchema: "GameRules",
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Предметы во взаимодействии");

            migrationBuilder.CreateTable(
                name: "InteractionsRoleActivities",
                schema: "InteractionRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    InteractionsRoleId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди роли взаимодействия"),
                    ActivityId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди деятельности"),
                    Order = table.Column<int>(type: "integer", nullable: true, comment: "Порядок деятельности"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InteractionsRoleActivities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InteractionsRoleActivities_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalSchema: "InteractionRules",
                        principalTable: "Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InteractionsRoleActivities_InteractionsRoles_InteractionsRo~",
                        column: x => x.InteractionsRoleId,
                        principalSchema: "InteractionRules",
                        principalTable: "InteractionsRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Деятельности роли взаимодействия");

            migrationBuilder.CreateTable(
                name: "PartyInteractionsRoles",
                schema: "InteractionRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    InteractionsRoleId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди роли взаимодействия"),
                    PartyId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди стороны"),
                    MinQuantityCharacters = table.Column<int>(type: "integer", nullable: true, comment: "Минимальное количество персонажей"),
                    MaxQuantityCharacters = table.Column<int>(type: "integer", nullable: true, comment: "Максимальное количество персонажей"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartyInteractionsRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PartyInteractionsRoles_InteractionsRoles_InteractionsRoleId",
                        column: x => x.InteractionsRoleId,
                        principalSchema: "InteractionRules",
                        principalTable: "InteractionsRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PartyInteractionsRoles_Parties_PartyId",
                        column: x => x.PartyId,
                        principalSchema: "InteractionRules",
                        principalTable: "Parties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Роли стороны");

            migrationBuilder.InsertData(
                schema: "System",
                table: "SystemRoles",
                columns: new[] { "Id", "CreatedByUserId", "CreatedOn", "ModifiedByUserId", "ModifiedOn", "Name", "RoleCreatedUser", "RoleModifiedUser" },
                values: new object[,]
                {
                    { new Guid("8094e0d0-3147-4791-9053-9667cbe107d7"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "AndminRole", "Default", "Default" },
                    { new Guid("8094e0d0-3148-4791-9053-9667cbe107d8"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "UserRole", "Default", "Default" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Characteristics_InteractionId",
                schema: "InteractionRules",
                table: "Characteristics",
                column: "InteractionId");

            migrationBuilder.CreateIndex(
                name: "IX_Actions_InteractionId",
                schema: "InteractionRules",
                table: "Actions",
                column: "InteractionId");

            migrationBuilder.CreateIndex(
                name: "IX_Actions_Name",
                schema: "InteractionRules",
                table: "Actions",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Actions_ScenarioActionId",
                schema: "InteractionRules",
                table: "Actions",
                column: "ScenarioActionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Activities_ApplicationAreaId",
                schema: "InteractionRules",
                table: "Activities",
                column: "ApplicationAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_ImgFileId",
                schema: "InteractionRules",
                table: "Activities",
                column: "ImgFileId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Activities_InteractionId",
                schema: "InteractionRules",
                table: "Activities",
                column: "InteractionId");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_Name",
                schema: "InteractionRules",
                table: "Activities",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ActivityActions_ActionId",
                schema: "InteractionRules",
                table: "ActivityActions",
                column: "ActionId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityActions_ActivityId",
                schema: "InteractionRules",
                table: "ActivityActions",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationAreas_Name",
                schema: "InteractionRules",
                table: "ApplicationAreas",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InteractionItems_ActivityId",
                schema: "InteractionRules",
                table: "InteractionItems",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_InteractionItems_InteractionsRoleId",
                schema: "InteractionRules",
                table: "InteractionItems",
                column: "InteractionsRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_InteractionItems_ItemId",
                schema: "InteractionRules",
                table: "InteractionItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Interactions_GameId",
                schema: "InteractionRules",
                table: "Interactions",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Interactions_ImgFileId",
                schema: "InteractionRules",
                table: "Interactions",
                column: "ImgFileId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Interactions_Name",
                schema: "InteractionRules",
                table: "Interactions",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Interactions_ScenarioLootId",
                schema: "InteractionRules",
                table: "Interactions",
                column: "ScenarioLootId");

            migrationBuilder.CreateIndex(
                name: "IX_Interactions_ScenarioReturnId",
                schema: "InteractionRules",
                table: "Interactions",
                column: "ScenarioReturnId");

            migrationBuilder.CreateIndex(
                name: "IX_Interactions_ScenarioVictoryId",
                schema: "InteractionRules",
                table: "Interactions",
                column: "ScenarioVictoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Interactions_TextFileId",
                schema: "InteractionRules",
                table: "Interactions",
                column: "TextFileId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InteractionsRoleActivities_ActivityId",
                schema: "InteractionRules",
                table: "InteractionsRoleActivities",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_InteractionsRoleActivities_InteractionsRoleId",
                schema: "InteractionRules",
                table: "InteractionsRoleActivities",
                column: "InteractionsRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_InteractionsRoles_ImgFileId",
                schema: "InteractionRules",
                table: "InteractionsRoles",
                column: "ImgFileId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InteractionsRoles_InteractionId",
                schema: "InteractionRules",
                table: "InteractionsRoles",
                column: "InteractionId");

            migrationBuilder.CreateIndex(
                name: "IX_InteractionsRoles_Name",
                schema: "InteractionRules",
                table: "InteractionsRoles",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InteractionsRoles_ScenarioCharacteristicsId",
                schema: "InteractionRules",
                table: "InteractionsRoles",
                column: "ScenarioCharacteristicsId");

            migrationBuilder.CreateIndex(
                name: "IX_InteractionsRoles_ScenarioInitiativeId",
                schema: "InteractionRules",
                table: "InteractionsRoles",
                column: "ScenarioInitiativeId");

            migrationBuilder.CreateIndex(
                name: "IX_InteractionsRoles_ScenarioLootId",
                schema: "InteractionRules",
                table: "InteractionsRoles",
                column: "ScenarioLootId");

            migrationBuilder.CreateIndex(
                name: "IX_InteractionsRoles_ScenarioPrerequisites",
                schema: "InteractionRules",
                table: "InteractionsRoles",
                column: "ScenarioPrerequisites");

            migrationBuilder.CreateIndex(
                name: "IX_InteractionsRoles_ScenarioReturnId",
                schema: "InteractionRules",
                table: "InteractionsRoles",
                column: "ScenarioReturnId");

            migrationBuilder.CreateIndex(
                name: "IX_InteractionsRoles_ScenarioVictoryId",
                schema: "InteractionRules",
                table: "InteractionsRoles",
                column: "ScenarioVictoryId");

            migrationBuilder.CreateIndex(
                name: "IX_InteractionsRoles_TextFileId",
                schema: "InteractionRules",
                table: "InteractionsRoles",
                column: "TextFileId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Parties_ImgFileId",
                schema: "InteractionRules",
                table: "Parties",
                column: "ImgFileId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Parties_InteractionId",
                schema: "InteractionRules",
                table: "Parties",
                column: "InteractionId");

            migrationBuilder.CreateIndex(
                name: "IX_Parties_Name",
                schema: "InteractionRules",
                table: "Parties",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Parties_ScenarioLootId",
                schema: "InteractionRules",
                table: "Parties",
                column: "ScenarioLootId");

            migrationBuilder.CreateIndex(
                name: "IX_Parties_ScenarioReturnId",
                schema: "InteractionRules",
                table: "Parties",
                column: "ScenarioReturnId");

            migrationBuilder.CreateIndex(
                name: "IX_Parties_ScenarioVictoryId",
                schema: "InteractionRules",
                table: "Parties",
                column: "ScenarioVictoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Parties_TextFileId",
                schema: "InteractionRules",
                table: "Parties",
                column: "TextFileId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PartyInteractionsRoles_InteractionsRoleId",
                schema: "InteractionRules",
                table: "PartyInteractionsRoles",
                column: "InteractionsRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_PartyInteractionsRoles_PartyId",
                schema: "InteractionRules",
                table: "PartyInteractionsRoles",
                column: "PartyId");

            migrationBuilder.CreateIndex(
                name: "IX_SystemRoles_Name",
                schema: "System",
                table: "SystemRoles",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CharacterCharacteristic_Characteristics_CharacteristicId",
                table: "CharacterCharacteristic",
                column: "CharacteristicId",
                principalSchema: "InteractionRules",
                principalTable: "Characteristics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CharacteristicModifier_Characteristics_CharacteristicId",
                table: "CharacteristicModifier",
                column: "CharacteristicId",
                principalSchema: "InteractionRules",
                principalTable: "Characteristics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Characteristics_Interactions_InteractionId",
                schema: "InteractionRules",
                table: "Characteristics",
                column: "InteractionId",
                principalSchema: "InteractionRules",
                principalTable: "Interactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Characteristics_Prerequisites_Id",
                schema: "InteractionRules",
                table: "Characteristics",
                column: "Id",
                principalSchema: "GameRules",
                principalTable: "Prerequisites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_SystemRoles_SystemRoleId",
                schema: "System",
                table: "UserRoles",
                column: "SystemRoleId",
                principalSchema: "System",
                principalTable: "SystemRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CharacterCharacteristic_Characteristics_CharacteristicId",
                table: "CharacterCharacteristic");

            migrationBuilder.DropForeignKey(
                name: "FK_CharacteristicModifier_Characteristics_CharacteristicId",
                table: "CharacteristicModifier");

            migrationBuilder.DropForeignKey(
                name: "FK_Characteristics_Interactions_InteractionId",
                schema: "InteractionRules",
                table: "Characteristics");

            migrationBuilder.DropForeignKey(
                name: "FK_Characteristics_Prerequisites_Id",
                schema: "InteractionRules",
                table: "Characteristics");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_SystemRoles_SystemRoleId",
                schema: "System",
                table: "UserRoles");

            migrationBuilder.DropTable(
                name: "ActivityActions",
                schema: "InteractionRules");

            migrationBuilder.DropTable(
                name: "InteractionItems",
                schema: "InteractionRules");

            migrationBuilder.DropTable(
                name: "InteractionsRoleActivities",
                schema: "InteractionRules");

            migrationBuilder.DropTable(
                name: "PartyInteractionsRoles",
                schema: "InteractionRules");

            migrationBuilder.DropTable(
                name: "SystemRoles",
                schema: "System");

            migrationBuilder.DropTable(
                name: "Actions",
                schema: "InteractionRules");

            migrationBuilder.DropTable(
                name: "Activities",
                schema: "InteractionRules");

            migrationBuilder.DropTable(
                name: "InteractionsRoles",
                schema: "InteractionRules");

            migrationBuilder.DropTable(
                name: "Parties",
                schema: "InteractionRules");

            migrationBuilder.DropTable(
                name: "ApplicationAreas",
                schema: "InteractionRules");

            migrationBuilder.DropTable(
                name: "Interactions",
                schema: "InteractionRules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Characteristics",
                schema: "InteractionRules",
                table: "Characteristics");

            migrationBuilder.DropIndex(
                name: "IX_Characteristics_InteractionId",
                schema: "InteractionRules",
                table: "Characteristics");

            migrationBuilder.DropColumn(
                name: "InteractionId",
                schema: "InteractionRules",
                table: "Characteristics");

            migrationBuilder.DropColumn(
                name: "MaxCondition",
                schema: "InteractionRules",
                table: "Characteristics");

            migrationBuilder.DropColumn(
                name: "MinCondition",
                schema: "InteractionRules",
                table: "Characteristics");

            migrationBuilder.RenameTable(
                name: "Characteristics",
                schema: "InteractionRules",
                newName: "Characteristic");

            migrationBuilder.RenameColumn(
                name: "SystemRoleId",
                schema: "System",
                table: "UserRoles",
                newName: "RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_UserRoles_SystemRoleId",
                schema: "System",
                table: "UserRoles",
                newName: "IX_UserRoles_RoleId");

            migrationBuilder.AlterTable(
                name: "Characteristic",
                oldComment: "Характеристики");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfGame",
                schema: "BaseGame",
                table: "Games",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StoryAboutRules",
                schema: "BaseGame",
                table: "Games",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedByUserId",
                table: "Characteristic",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Characteristic",
                type: "timestamp without time zone",
                nullable: false,
                defaultValueSql: "now() at time zone 'utc'");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Characteristic",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "GameId",
                table: "Characteristic",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedByUserId",
                table: "Characteristic",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "Characteristic",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Characteristic",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RoleCreatedUser",
                table: "Characteristic",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoleModifiedUser",
                table: "Characteristic",
                type: "text",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Characteristic",
                table: "Characteristic",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Roles",
                schema: "System",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Роль"),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                },
                comment: "Роли");

            migrationBuilder.InsertData(
                schema: "System",
                table: "Roles",
                columns: new[] { "Id", "CreatedByUserId", "CreatedOn", "ModifiedByUserId", "ModifiedOn", "Name", "RoleCreatedUser", "RoleModifiedUser" },
                values: new object[,]
                {
                    { new Guid("8094e0d0-3147-4791-9053-9667cbe107d7"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "AndminRole", "Default", "Default" },
                    { new Guid("8094e0d0-3148-4791-9053-9667cbe107d8"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "UserRole", "Default", "Default" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Characteristic_GameId",
                table: "Characteristic",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Name",
                schema: "System",
                table: "Roles",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CharacterCharacteristic_Characteristic_CharacteristicId",
                table: "CharacterCharacteristic",
                column: "CharacteristicId",
                principalTable: "Characteristic",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Characteristic_Games_GameId",
                table: "Characteristic",
                column: "GameId",
                principalSchema: "BaseGame",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CharacteristicModifier_Characteristic_CharacteristicId",
                table: "CharacteristicModifier",
                column: "CharacteristicId",
                principalTable: "Characteristic",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Roles_RoleId",
                schema: "System",
                table: "UserRoles",
                column: "RoleId",
                principalSchema: "System",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
