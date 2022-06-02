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

            migrationBuilder.DropIndex(
                name: "IX_NotificationTradeRequests_BagId",
                schema: "Notifications",
                table: "NotificationTradeRequests");

            migrationBuilder.DropColumn(
                name: "BagId",
                schema: "Notifications",
                table: "NotificationTradeRequests");

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
                name: "Creatures",
                schema: "GameRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    InstanceId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди Экземпляра"),
                    ImgFileId = table.Column<Guid>(type: "uuid", nullable: true, comment: "Айди графического файла"),
                    CreatureBodyId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди тела существа"),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Название существа"),
                    Description = table.Column<string>(type: "text", nullable: true, comment: "Описание шаблона"),
                    Type = table.Column<string>(type: "text", nullable: false, comment: "Тип шаблона существа"),
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
                    table.UniqueConstraint("AK_Creatures_CreatureBodyId", x => x.CreatureBodyId);
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
                    table.UniqueConstraint("AK_CreatureTemplates_BodyTemplateId", x => x.BodyTemplateId);
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
                name: "ConditionCreature",
                schema: "GameRules",
                columns: table => new
                {
                    ConditionsId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreaturesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConditionCreature", x => new { x.ConditionsId, x.CreaturesId });
                    table.ForeignKey(
                        name: "FK_ConditionCreature_Conditions_ConditionsId",
                        column: x => x.ConditionsId,
                        principalSchema: "GameRules",
                        principalTable: "Conditions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConditionCreature_Creatures_CreaturesId",
                        column: x => x.CreaturesId,
                        principalSchema: "GameRules",
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
                        principalSchema: "GameRules",
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
                name: "BodyTemplates",
                schema: "GameRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Название шаблона тела"),
                    GameId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди игры"),
                    CreatureTemplateId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди шаблона существа"),
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
                        name: "FK_BodyTemplates_CreatureTemplates_CreatureTemplateId",
                        column: x => x.CreatureTemplateId,
                        principalSchema: "GameRules",
                        principalTable: "CreatureTemplates",
                        principalColumn: "BodyTemplateId",
                        onDelete: ReferentialAction.Cascade);
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
                name: "AbilityCreature",
                schema: "GameRules",
                columns: table => new
                {
                    AbilitiesId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreaturesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbilityCreature", x => new { x.AbilitiesId, x.CreaturesId });
                    table.ForeignKey(
                        name: "FK_AbilityCreature_Abilities_AbilitiesId",
                        column: x => x.AbilitiesId,
                        principalSchema: "GameRules",
                        principalTable: "Abilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AbilityCreature_Creatures_CreaturesId",
                        column: x => x.CreaturesId,
                        principalSchema: "GameRules",
                        principalTable: "Creatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppliedCondition",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AbilityId = table.Column<Guid>(type: "uuid", nullable: false),
                    ApplyChance = table.Column<double>(type: "double precision", nullable: false),
                    ConditionId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppliedCondition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppliedCondition_Abilities_AbilityId",
                        column: x => x.AbilityId,
                        principalSchema: "GameRules",
                        principalTable: "Abilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppliedCondition_Conditions_ConditionId",
                        column: x => x.ConditionId,
                        principalSchema: "GameRules",
                        principalTable: "Conditions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BodyTemplates_BodyParts",
                schema: "GameRules",
                columns: table => new
                {
                    BodyTemplateId = table.Column<Guid>(type: "uuid", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Название"),
                    DamageModifer = table.Column<double>(type: "double precision", nullable: false, comment: "Модификатор урона"),
                    MinToHit = table.Column<int>(type: "integer", nullable: false, comment: "Минимальное значение попадания"),
                    MaxToHit = table.Column<int>(type: "integer", nullable: false, comment: "Максимальное значение попадания"),
                    StartingArmor = table.Column<int>(type: "integer", nullable: false, comment: "Начальная броня"),
                    CurrentArmor = table.Column<int>(type: "integer", nullable: false, comment: "Текущая броня")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BodyTemplates_BodyParts", x => new { x.BodyTemplateId, x.Id });
                    table.ForeignKey(
                        name: "FK_BodyTemplates_BodyParts_BodyTemplates_BodyTemplateId",
                        column: x => x.BodyTemplateId,
                        principalSchema: "GameRules",
                        principalTable: "BodyTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CreatureBodies",
                schema: "GameInstance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    InstanceId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди экземпляра"),
                    BodyTemplateId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди шаблона тела"),
                    CreatureId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди сущесттва"),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Название тела существа"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreatureBodies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CreatureBodies_BodyTemplates_BodyTemplateId",
                        column: x => x.BodyTemplateId,
                        principalSchema: "GameRules",
                        principalTable: "BodyTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CreatureBodies_Creatures_CreatureId",
                        column: x => x.CreatureId,
                        principalSchema: "GameRules",
                        principalTable: "Creatures",
                        principalColumn: "CreatureBodyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CreatureBodies_Instances_InstanceId",
                        column: x => x.InstanceId,
                        principalSchema: "GameInstance",
                        principalTable: "Instances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Шаблоны тел");

            migrationBuilder.CreateTable(
                name: "CreatureBodies_BodyParts",
                schema: "GameInstance",
                columns: table => new
                {
                    CreatureBodyId = table.Column<Guid>(type: "uuid", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Название"),
                    DamageModifer = table.Column<double>(type: "double precision", nullable: false, comment: "Модификатор урона"),
                    MinToHit = table.Column<int>(type: "integer", nullable: false, comment: "Минимальное значение попадания"),
                    MaxToHit = table.Column<int>(type: "integer", nullable: false, comment: "Максимальное значение попадания"),
                    StartingArmor = table.Column<int>(type: "integer", nullable: false, comment: "Начальная броня"),
                    CurrentArmor = table.Column<int>(type: "integer", nullable: false, comment: "Текущая броня")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreatureBodies_BodyParts", x => new { x.CreatureBodyId, x.Id });
                    table.ForeignKey(
                        name: "FK_CreatureBodies_BodyParts_CreatureBodies_CreatureBodyId",
                        column: x => x.CreatureBodyId,
                        principalSchema: "GameInstance",
                        principalTable: "CreatureBodies",
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
                name: "IX_AbilityCreature_CreaturesId",
                schema: "GameRules",
                table: "AbilityCreature",
                column: "CreaturesId");

            migrationBuilder.CreateIndex(
                name: "IX_AppliedCondition_AbilityId",
                table: "AppliedCondition",
                column: "AbilityId");

            migrationBuilder.CreateIndex(
                name: "IX_AppliedCondition_ConditionId",
                table: "AppliedCondition",
                column: "ConditionId");

            migrationBuilder.CreateIndex(
                name: "IX_BodyTemplates_CreatureTemplateId",
                schema: "GameRules",
                table: "BodyTemplates",
                column: "CreatureTemplateId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BodyTemplates_GameId",
                schema: "GameRules",
                table: "BodyTemplates",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_ConditionCreature_CreaturesId",
                schema: "GameRules",
                table: "ConditionCreature",
                column: "CreaturesId");

            migrationBuilder.CreateIndex(
                name: "IX_CreatureBodies_BodyTemplateId",
                schema: "GameInstance",
                table: "CreatureBodies",
                column: "BodyTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_CreatureBodies_CreatureId",
                schema: "GameInstance",
                table: "CreatureBodies",
                column: "CreatureId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CreatureBodies_InstanceId",
                schema: "GameInstance",
                table: "CreatureBodies",
                column: "InstanceId");

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
                name: "IX_Creatures_ImgFileId",
                schema: "GameRules",
                table: "Creatures",
                column: "ImgFileId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Creatures_InstanceId",
                schema: "GameRules",
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AbilityCreature",
                schema: "GameRules");

            migrationBuilder.DropTable(
                name: "AppliedCondition");

            migrationBuilder.DropTable(
                name: "BodyTemplates_BodyParts",
                schema: "GameRules");

            migrationBuilder.DropTable(
                name: "ConditionCreature",
                schema: "GameRules");

            migrationBuilder.DropTable(
                name: "CreatureBodies_BodyParts",
                schema: "GameInstance");

            migrationBuilder.DropTable(
                name: "CreatureParameters",
                schema: "GameInstance");

            migrationBuilder.DropTable(
                name: "CreatureTemplateParameters",
                schema: "GameInstance");

            migrationBuilder.DropTable(
                name: "Abilities",
                schema: "GameRules");

            migrationBuilder.DropTable(
                name: "Conditions",
                schema: "GameRules");

            migrationBuilder.DropTable(
                name: "CreatureBodies",
                schema: "GameInstance");

            migrationBuilder.DropTable(
                name: "BodyTemplates",
                schema: "GameRules");

            migrationBuilder.DropTable(
                name: "Creatures",
                schema: "GameRules");

            migrationBuilder.DropTable(
                name: "CreatureTemplates",
                schema: "GameRules");

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
        }
    }
}
