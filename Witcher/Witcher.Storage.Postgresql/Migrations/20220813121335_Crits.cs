using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Witcher.Storage.Postgresql.Migrations
{
    public partial class Crits : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Skills_Games_GameId",
                schema: "GameRules",
                table: "Skills");

            migrationBuilder.DropTable(
                name: "AbilityDamageTypes",
                schema: "GameRules");

            migrationBuilder.DropTable(
                name: "CurrentConditions",
                schema: "Battles");

            migrationBuilder.DropIndex(
                name: "IX_Skills_GameId",
                schema: "GameRules",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "Description",
                schema: "GameRules",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "GameId",
                schema: "GameRules",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "MaxValueSkills",
                schema: "GameRules",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "MinValueSkills",
                schema: "GameRules",
                table: "Skills");

            migrationBuilder.EnsureSchema(
                name: "Effects");

            migrationBuilder.AlterColumn<string>(
                name: "StatName",
                schema: "GameRules",
                table: "Skills",
                type: "text",
                nullable: false,
                defaultValue: "",
                comment: "Название корреспондирующей характеристики",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "Название корреспондирующей характеристики");

            migrationBuilder.AlterColumn<string>(
                name: "StatName",
                schema: "GameRules",
                table: "CreatureTemplateParameters",
                type: "text",
                nullable: false,
                defaultValue: "",
                comment: "Название корреспондирующей характеристики",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "Название корреспондирующей характеристики");

            migrationBuilder.AddColumn<Guid>(
                name: "LeadingArmId",
                schema: "Battles",
                table: "Creatures",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "MaxBody",
                schema: "Battles",
                table: "Creatures",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "Максимальное телосложение");

            migrationBuilder.AddColumn<int>(
                name: "MaxCra",
                schema: "Battles",
                table: "Creatures",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "Максимальный крафт");

            migrationBuilder.AddColumn<int>(
                name: "MaxDex",
                schema: "Battles",
                table: "Creatures",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "Максимальна ловкость");

            migrationBuilder.AddColumn<int>(
                name: "MaxEmp",
                schema: "Battles",
                table: "Creatures",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "Максимальная эмпатия");

            migrationBuilder.AddColumn<int>(
                name: "MaxHP",
                schema: "Battles",
                table: "Creatures",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "Максимальные хиты");

            migrationBuilder.AddColumn<int>(
                name: "MaxInt",
                schema: "Battles",
                table: "Creatures",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "Максимальный интеллект");

            migrationBuilder.AddColumn<int>(
                name: "MaxLuck",
                schema: "Battles",
                table: "Creatures",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "Максимальная удача");

            migrationBuilder.AddColumn<int>(
                name: "MaxRef",
                schema: "Battles",
                table: "Creatures",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "Максимальные рефлексы");

            migrationBuilder.AddColumn<int>(
                name: "MaxSpeed",
                schema: "Battles",
                table: "Creatures",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "Максимальная скорость");

            migrationBuilder.AddColumn<int>(
                name: "MaxSta",
                schema: "Battles",
                table: "Creatures",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "Максимальная стамина");

            migrationBuilder.AddColumn<int>(
                name: "MaxWill",
                schema: "Battles",
                table: "Creatures",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "Максимальнпя воля");

            migrationBuilder.AddColumn<int>(
                name: "Stun",
                schema: "Battles",
                table: "Creatures",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "StatName",
                schema: "Battles",
                table: "CreatureParameters",
                type: "text",
                nullable: false,
                defaultValue: "",
                comment: "Название корреспондирующей характеристики",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "Название корреспондирующей характеристики");

            migrationBuilder.AddColumn<int>(
                name: "MaxValue",
                schema: "Battles",
                table: "CreatureParameters",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "Макксимальное значение навыка");

            migrationBuilder.AddColumn<int>(
                name: "Round",
                schema: "Battles",
                table: "Battles",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "DamageTypeId",
                schema: "GameRules",
                table: "Abilities",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Effects",
                schema: "Battles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Название эффекта"),
                    CreatureId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди существа"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Effects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Effects_Creatures_CreatureId",
                        column: x => x.CreatureId,
                        principalSchema: "Battles",
                        principalTable: "Creatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты");

            migrationBuilder.CreateTable(
                name: "BleedEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BleedEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BleedEffects_Effects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "Effects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты кровотечения");

            migrationBuilder.CreateTable(
                name: "BleedingWoundEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    Severity = table.Column<int>(type: "integer", nullable: false, comment: "Тяжесть"),
                    Damage = table.Column<int>(type: "integer", nullable: false, comment: "Урон")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BleedingWoundEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BleedingWoundEffects_Effects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "Effects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты кровавой раны");

            migrationBuilder.CreateTable(
                name: "CritEffects",
                schema: "Battles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    CreaturePartId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди части тела")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CritEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CritEffects_CreatureParts_CreaturePartId",
                        column: x => x.CreaturePartId,
                        principalSchema: "Battles",
                        principalTable: "CreatureParts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CritEffects_Effects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "Effects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Критические эффекты");

            migrationBuilder.CreateTable(
                name: "DeadEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeadEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeadEffects_Effects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "Effects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты смерти");

            migrationBuilder.CreateTable(
                name: "DyingEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    Counter = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DyingEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DyingEffects_Effects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "Effects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты при смерти");

            migrationBuilder.CreateTable(
                name: "FireEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FireEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FireEffects_Effects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "Effects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты горения");

            migrationBuilder.CreateTable(
                name: "FreezeEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FreezeEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FreezeEffects_Effects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "Effects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты заморозки");

            migrationBuilder.CreateTable(
                name: "PoisonEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PoisonEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PoisonEffects_Effects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "Effects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты отравления");

            migrationBuilder.CreateTable(
                name: "StaggeredEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaggeredEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StaggeredEffects_Effects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "Effects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты ошеломления");

            migrationBuilder.CreateTable(
                name: "StunEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StunEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StunEffects_Effects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "Effects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты дезориентации");

            migrationBuilder.CreateTable(
                name: "SufflocationEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SufflocationEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SufflocationEffects_Effects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "Effects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты удушья");

            migrationBuilder.CreateTable(
                name: "ComplexArmCritEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    Severity = table.Column<int>(type: "integer", nullable: false),
                    PenaltyApplied = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComplexArmCritEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComplexArmCritEffects_CritEffects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "CritEffects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты перелома руки");

            migrationBuilder.CreateTable(
                name: "ComplexHead1CritEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    Severity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComplexHead1CritEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComplexHead1CritEffects_CritEffects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "CritEffects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты выбитых зубов");

            migrationBuilder.CreateTable(
                name: "ComplexHead2CritEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    Severity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComplexHead2CritEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComplexHead2CritEffects_CritEffects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "CritEffects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты небольшой травмы головы");

            migrationBuilder.CreateTable(
                name: "ComplexLegCritEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    Severity = table.Column<int>(type: "integer", nullable: false),
                    PenaltyApplied = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComplexLegCritEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComplexLegCritEffects_CritEffects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "CritEffects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты перелома ноги");

            migrationBuilder.CreateTable(
                name: "ComplexTailCritEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    Severity = table.Column<int>(type: "integer", nullable: false),
                    PenaltyApplied = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComplexTailCritEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComplexTailCritEffects_CritEffects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "CritEffects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты перелома хвоста");

            migrationBuilder.CreateTable(
                name: "ComplexTorso1CritEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    Severity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComplexTorso1CritEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComplexTorso1CritEffects_CritEffects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "CritEffects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты сломанных ребер");

            migrationBuilder.CreateTable(
                name: "ComplexTorso2CritEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    RoundCounter = table.Column<int>(type: "integer", nullable: false, comment: "Счетчик раундов"),
                    Severity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComplexTorso2CritEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComplexTorso2CritEffects_CritEffects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "CritEffects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты разрыва селезенки");

            migrationBuilder.CreateTable(
                name: "ComplexWingCritEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    Severity = table.Column<int>(type: "integer", nullable: false),
                    PenaltyApplied = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComplexWingCritEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComplexWingCritEffects_CritEffects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "CritEffects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты перелома крыла");

            migrationBuilder.CreateTable(
                name: "DeadlyArmCritEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    Severity = table.Column<int>(type: "integer", nullable: false),
                    PenaltyApplied = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeadlyArmCritEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeadlyArmCritEffects_CritEffects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "CritEffects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты потери руки");

            migrationBuilder.CreateTable(
                name: "DeadlyHead1CritEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    Severity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeadlyHead1CritEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeadlyHead1CritEffects_CritEffects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "CritEffects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты повреждения глаза");

            migrationBuilder.CreateTable(
                name: "DeadlyHead2CritEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    Severity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeadlyHead2CritEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeadlyHead2CritEffects_CritEffects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "CritEffects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты отсечения головы");

            migrationBuilder.CreateTable(
                name: "DeadlyLegCritEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    Severity = table.Column<int>(type: "integer", nullable: false),
                    PenaltyApplied = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeadlyLegCritEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeadlyLegCritEffects_CritEffects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "CritEffects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты потери ноги");

            migrationBuilder.CreateTable(
                name: "DeadlyTailCritEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    Severity = table.Column<int>(type: "integer", nullable: false),
                    PenaltyApplied = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeadlyTailCritEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeadlyTailCritEffects_CritEffects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "CritEffects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты потери хвоста");

            migrationBuilder.CreateTable(
                name: "DeadlyTorso1CritEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    Severity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeadlyTorso1CritEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeadlyTorso1CritEffects_CritEffects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "CritEffects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты септического шока");

            migrationBuilder.CreateTable(
                name: "DeadlyTorso2CritEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    Severity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeadlyTorso2CritEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeadlyTorso2CritEffects_CritEffects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "CritEffects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты травмы сердца");

            migrationBuilder.CreateTable(
                name: "DeadlyWingCritEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    Severity = table.Column<int>(type: "integer", nullable: false),
                    PenaltyApplied = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeadlyWingCritEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeadlyWingCritEffects_CritEffects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "CritEffects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты потери крыла");

            migrationBuilder.CreateTable(
                name: "DifficultArmCritEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    Severity = table.Column<int>(type: "integer", nullable: false),
                    PenaltyApplied = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DifficultArmCritEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DifficultArmCritEffects_CritEffects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "CritEffects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты открытого перелома руки");

            migrationBuilder.CreateTable(
                name: "DifficultHead1CritEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    RoundCounter = table.Column<int>(type: "integer", nullable: false),
                    NextCheck = table.Column<int>(type: "integer", nullable: false),
                    Severity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DifficultHead1CritEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DifficultHead1CritEffects_CritEffects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "CritEffects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты контузии");

            migrationBuilder.CreateTable(
                name: "DifficultHead2CritEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    Severity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DifficultHead2CritEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DifficultHead2CritEffects_CritEffects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "CritEffects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты проломленного черепа");

            migrationBuilder.CreateTable(
                name: "DifficultLegCritEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    Severity = table.Column<int>(type: "integer", nullable: false),
                    PenaltyApplied = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DifficultLegCritEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DifficultLegCritEffects_CritEffects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "CritEffects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты открытого перелома ноги");

            migrationBuilder.CreateTable(
                name: "DifficultTailCritEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    Severity = table.Column<int>(type: "integer", nullable: false),
                    PenaltyApplied = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DifficultTailCritEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DifficultTailCritEffects_CritEffects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "CritEffects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты открытого перелома хвоста");

            migrationBuilder.CreateTable(
                name: "DifficultTorso1CritEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    Severity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DifficultTorso1CritEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DifficultTorso1CritEffects_CritEffects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "CritEffects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты сосущей раны грудной клетки");

            migrationBuilder.CreateTable(
                name: "DifficultTorso2CritEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    Severity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DifficultTorso2CritEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DifficultTorso2CritEffects_CritEffects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "CritEffects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты раны в живот");

            migrationBuilder.CreateTable(
                name: "DifficultWingCritEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    Severity = table.Column<int>(type: "integer", nullable: false),
                    PenaltyApplied = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DifficultWingCritEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DifficultWingCritEffects_CritEffects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "CritEffects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты открытого перелома крыла");

            migrationBuilder.CreateTable(
                name: "SimpleArmCritEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    Severity = table.Column<int>(type: "integer", nullable: false),
                    PenaltyApplied = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SimpleArmCritEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SimpleArmCritEffects_CritEffects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "CritEffects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты вывиха руки");

            migrationBuilder.CreateTable(
                name: "SimpleHead1CritEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    Severity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SimpleHead1CritEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SimpleHead1CritEffects_CritEffects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "CritEffects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты уродующего шрама");

            migrationBuilder.CreateTable(
                name: "SimpleHead2CritEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    Severity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SimpleHead2CritEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SimpleHead2CritEffects_CritEffects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "CritEffects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты треснувшей челюсти");

            migrationBuilder.CreateTable(
                name: "SimpleLegCritEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    Severity = table.Column<int>(type: "integer", nullable: false),
                    PenaltyApplied = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SimpleLegCritEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SimpleLegCritEffects_CritEffects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "CritEffects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты вывиха ноги");

            migrationBuilder.CreateTable(
                name: "SimpleTailCritEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    Severity = table.Column<int>(type: "integer", nullable: false),
                    PenaltyApplied = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SimpleTailCritEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SimpleTailCritEffects_CritEffects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "CritEffects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты вывиха крыла");

            migrationBuilder.CreateTable(
                name: "SimpleTorso1CritEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    Severity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SimpleTorso1CritEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SimpleTorso1CritEffects_CritEffects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "CritEffects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты инородного объекта");

            migrationBuilder.CreateTable(
                name: "SimpleTorso2CritEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    Severity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SimpleTorso2CritEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SimpleTorso2CritEffects_CritEffects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "CritEffects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты треснувших ребер");

            migrationBuilder.CreateTable(
                name: "SimpleWingCritEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    Severity = table.Column<int>(type: "integer", nullable: false),
                    PenaltyApplied = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SimpleWingCritEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SimpleWingCritEffects_CritEffects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "CritEffects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты вывиха крыла");

            migrationBuilder.InsertData(
                schema: "GameRules",
                table: "Conditions",
                columns: new[] { "Id", "CreatedByUserId", "CreatedOn", "ModifiedByUserId", "ModifiedOn", "Name", "RoleCreatedUser", "RoleModifiedUser" },
                values: new object[,]
                {
                    { new Guid("208ed04e-73aa-4e57-bb58-26c807fcf558"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "DeadlyTail", "Default", "Default" },
                    { new Guid("208ed04e-73aa-4e57-bb58-26c807fcf559"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "DeadlyWing", "Default", "Default" },
                    { new Guid("208ed04e-73aa-4e57-bb58-26c807fcf560"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "DeadlyLeg", "Default", "Default" },
                    { new Guid("208ed04e-73aa-4e57-bb58-26c807fcf561"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "DeadlyArm", "Default", "Default" },
                    { new Guid("208ed04e-73aa-4e57-bb58-26c807fcf562"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "DeadlyTorso2", "Default", "Default" },
                    { new Guid("208ed04e-73aa-4e57-bb58-26c807fcf563"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "DeadlyTorso1", "Default", "Default" },
                    { new Guid("208ed04e-73aa-4e57-bb58-26c807fcf564"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "DeadlyHead2", "Default", "Default" },
                    { new Guid("208ed04e-73aa-4e57-bb58-26c807fcf565"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "DeadlyHead1", "Default", "Default" },
                    { new Guid("208ed04e-73aa-4e57-bb58-26c807fcf566"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "DifficultTail", "Default", "Default" },
                    { new Guid("208ed04e-73aa-4e57-bb58-26c807fcf567"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "DifficultWing", "Default", "Default" },
                    { new Guid("208ed04e-73aa-4e57-bb58-26c807fcf568"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "DifficultLeg", "Default", "Default" },
                    { new Guid("208ed04e-73aa-4e57-bb58-26c807fcf569"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "DifficultArm", "Default", "Default" },
                    { new Guid("208ed04e-73aa-4e57-bb58-26c807fcf570"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "DifficultTorso2", "Default", "Default" },
                    { new Guid("208ed04e-73aa-4e57-bb58-26c807fcf571"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "DifficultTorso1", "Default", "Default" },
                    { new Guid("208ed04e-73aa-4e57-bb58-26c807fcf572"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "DifficultHead2", "Default", "Default" },
                    { new Guid("208ed04e-73aa-4e57-bb58-26c807fcf573"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "DifficultHead1", "Default", "Default" },
                    { new Guid("208ed04e-73aa-4e57-bb58-26c807fcf574"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "ComplexTail", "Default", "Default" },
                    { new Guid("208ed04e-73aa-4e57-bb58-26c807fcf575"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "ComplexWing", "Default", "Default" },
                    { new Guid("208ed04e-73aa-4e57-bb58-26c807fcf576"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "ComplexLeg", "Default", "Default" },
                    { new Guid("208ed04e-73aa-4e57-bb58-26c807fcf577"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "ComplexArm", "Default", "Default" },
                    { new Guid("208ed04e-73aa-4e57-bb58-26c807fcf578"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "ComplexTorso2", "Default", "Default" },
                    { new Guid("208ed04e-73aa-4e57-bb58-26c807fcf579"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "ComplexTorso1", "Default", "Default" },
                    { new Guid("208ed04e-73aa-4e57-bb58-26c807fcf580"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "ComplexHead2", "Default", "Default" },
                    { new Guid("208ed04e-73aa-4e57-bb58-26c807fcf581"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "ComplexHead1", "Default", "Default" },
                    { new Guid("208ed04e-73aa-4e57-bb58-26c807fcf582"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "SimpleTail", "Default", "Default" },
                    { new Guid("208ed04e-73aa-4e57-bb58-26c807fcf583"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "SimpleWing", "Default", "Default" },
                    { new Guid("208ed04e-73aa-4e57-bb58-26c807fcf584"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "SimpleLeg", "Default", "Default" },
                    { new Guid("208ed04e-73aa-4e57-bb58-26c807fcf585"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "SimpleArm", "Default", "Default" },
                    { new Guid("208ed04e-73aa-4e57-bb58-26c807fcf586"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "SimpleTorso2", "Default", "Default" },
                    { new Guid("208ed04e-73aa-4e57-bb58-26c807fcf587"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "SimpleTorso1", "Default", "Default" },
                    { new Guid("208ed04e-73aa-4e57-bb58-26c807fcf588"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "SimpleHead2", "Default", "Default" },
                    { new Guid("208ed04e-73aa-4e57-bb58-26c807fcf589"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "SimpleHead1", "Default", "Default" },
                    { new Guid("7794e0d0-3147-4791-9053-9667cbe127d7"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "BleedingWound", "Default", "Default" },
                    { new Guid("8895e0d0-3147-4791-9053-9667cbe127d7"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Fire", "Default", "Default" },
                    { new Guid("8895e0d1-3147-4791-9053-9667cbe127d7"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Freeze", "Default", "Default" },
                    { new Guid("afb1c2ac-f6ab-035e-aedd-011da6f5ea9a"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Blinded", "Default", "Default" },
                    { new Guid("afb1c2ac-f6ab-435e-aedd-011da6f5ea9a"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Stun", "Default", "Default" },
                    { new Guid("afb1c2ac-f6ab-535e-aedd-011da6f5ea9a"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Staggered", "Default", "Default" },
                    { new Guid("afb1c2ac-f6ab-635e-aedd-011da6f5ea9a"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Intoxication", "Default", "Default" },
                    { new Guid("afb1c2ac-f6ab-735e-aedd-011da6f5ea9a"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Hallutination", "Default", "Default" },
                    { new Guid("afb1c2ac-f6ab-835e-aedd-011da6f5ea9a"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Nausea", "Default", "Default" },
                    { new Guid("afb1c2ac-f6ab-935e-aedd-011da6f5ea9a"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Sufflocation", "Default", "Default" }
                });

            migrationBuilder.InsertData(
                schema: "GameRules",
                table: "Skills",
                columns: new[] { "Id", "CreatedByUserId", "CreatedOn", "ModifiedByUserId", "ModifiedOn", "Name", "RoleCreatedUser", "RoleModifiedUser", "StatName" },
                values: new object[,]
                {
                    { new Guid("32ee830e-7bee-4924-9ddf-1070ceffecdd"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Education", "Default", "Default", "Int" },
                    { new Guid("4fcbd3d6-fde0-47c1-899d-a8c82c291751"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "CommonLanguage", "Default", "Default", "Int" },
                    { new Guid("754ef5e9-8960-4c38-a1be-a3c43c92b1cd"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Deduction", "Default", "Default", "Int" },
                    { new Guid("c5f00eea-10d5-426e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Awareness", "Default", "Default", "Int" },
                    { new Guid("c5f01eea-10d5-426e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Business", "Default", "Default", "Int" },
                    { new Guid("c5f03eea-10d5-426e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "ElderLanguage", "Default", "Default", "Int" },
                    { new Guid("c5f04eea-10d5-426e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "DwarfenLanguage", "Default", "Default", "Int" },
                    { new Guid("c5f05eea-10d5-426e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "MonsterLore", "Default", "Default", "Int" },
                    { new Guid("c5f06eea-10d5-426e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "SocialEtiquette", "Default", "Default", "Int" },
                    { new Guid("c5f07eea-10d5-426e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Streetwise", "Default", "Default", "Int" },
                    { new Guid("c5f08eea-10d5-426e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Tactics", "Default", "Default", "Int" },
                    { new Guid("c5f09eea-10d5-426e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Teaching", "Default", "Default", "Int" },
                    { new Guid("c5f10eea-10d5-426e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "WildernessSurvival", "Default", "Default", "Int" },
                    { new Guid("c5f11eea-10d5-428e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Riding", "Default", "Default", "Ref" },
                    { new Guid("c5f12eea-10d5-428e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Sailing", "Default", "Default", "Ref" },
                    { new Guid("c5f13eea-10d5-726e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "SleightOfHand", "Default", "Default", "Dex" },
                    { new Guid("c5f14eea-10d5-826e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Stealth", "Default", "Default", "Dex" },
                    { new Guid("c5f15eea-10d5-826e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Charisma", "Default", "Default", "Emp" },
                    { new Guid("c5f16eea-10d5-826e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Deceit", "Default", "Default", "Emp" },
                    { new Guid("c5f17eea-10d5-826e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "FineArts", "Default", "Default", "Emp" },
                    { new Guid("c5f18eea-10d5-826e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Gambling", "Default", "Default", "Emp" },
                    { new Guid("c5f19eea-10d5-826e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "GroomingAndStyle", "Default", "Default", "Emp" },
                    { new Guid("c5f20eea-10d5-826e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "HumanPerception", "Default", "Default", "Emp" },
                    { new Guid("c5f21eea-10d5-826e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Leadership", "Default", "Default", "Emp" },
                    { new Guid("c5f22eea-10d5-826e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Persuasion", "Default", "Default", "Emp" },
                    { new Guid("c5f23eea-10d5-826e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Perfomance", "Default", "Default", "Emp" },
                    { new Guid("c5f24eea-10d5-826e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Seduction", "Default", "Default", "Emp" },
                    { new Guid("c5f25eea-10d5-026e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Alchemy", "Default", "Default", "Cra" },
                    { new Guid("c5f26eea-10d5-026e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Crafting", "Default", "Default", "Cra" },
                    { new Guid("c5f27eea-10d5-026e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Diguise", "Default", "Default", "Cra" },
                    { new Guid("c5f28eea-10d5-026e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Forgery", "Default", "Default", "Cra" },
                    { new Guid("c5f29eea-10d5-026e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "PickLock", "Default", "Default", "Cra" },
                    { new Guid("c5f30eea-10d5-026e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "TrapCrafting", "Default", "Default", "Cra" },
                    { new Guid("c5f31eea-10d5-026e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Courage", "Default", "Default", "Will" },
                    { new Guid("c5f32eea-10d5-026e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "HexWeaving", "Default", "Default", "Will" },
                    { new Guid("c5f33eea-10d5-026e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Intimidation", "Default", "Default", "Will" },
                    { new Guid("c5f34eea-10d5-026e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "RitualCrafting", "Default", "Default", "Will" },
                    { new Guid("c5f99eea-10d5-026e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "FirstAid", "Default", "Default", "Cra" },
                    { new Guid("c5f99eea-10d5-420e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Staff/Spear", "Default", "Default", "Ref" },
                    { new Guid("c5f99eea-10d5-426e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Brawling", "Default", "Default", "Ref" },
                    { new Guid("c5f99eea-10d5-427e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Dodge", "Default", "Default", "Ref" },
                    { new Guid("c5f99eea-10d5-428e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Melee", "Default", "Default", "Ref" },
                    { new Guid("c5f99eea-10d5-429e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "SmallBlades", "Default", "Default", "Ref" },
                    { new Guid("c5f99eea-10d5-436e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "SpellCasting", "Default", "Default", "Will" },
                    { new Guid("c5f99eea-10d5-446e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "ResistMagic", "Default", "Default", "Will" },
                    { new Guid("c5f99eea-10d5-456e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "ResistCoercion", "Default", "Default", "Will" },
                    { new Guid("c5f99eea-10d5-466e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Needling", "Default", "Default", "Emp" },
                    { new Guid("c5f99eea-10d5-476e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "EyeGouge", "Default", "Default", "Dex" },
                    { new Guid("c5f99eea-10d5-486e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "BleedingWound", "Default", "Default", "Int" },
                    { new Guid("c5f99eea-10d5-496e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "HealingHands", "Default", "Default", "Cra" },
                    { new Guid("c5f99eea-10d5-506e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Physique", "Default", "Default", "Body" },
                    { new Guid("c5f99eea-10d5-526e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Swordsmanship", "Default", "Default", "Ref" },
                    { new Guid("c5f99eea-10d5-626e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Archery", "Default", "Default", "Dex" },
                    { new Guid("c5f99eea-10d5-726e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Athletics", "Default", "Default", "Dex" },
                    { new Guid("c5f99eea-10d5-826e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Crossbow", "Default", "Default", "Dex" },
                    { new Guid("c5f99eea-10d5-926e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Endurance", "Default", "Default", "Body" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Abilities_DamageTypeId",
                schema: "GameRules",
                table: "Abilities",
                column: "DamageTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CritEffects_CreaturePartId",
                schema: "Battles",
                table: "CritEffects",
                column: "CreaturePartId");

            migrationBuilder.CreateIndex(
                name: "IX_Effects_CreatureId",
                schema: "Battles",
                table: "Effects",
                column: "CreatureId");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Abilities_DamageTypes_DamageTypeId",
                schema: "GameRules",
                table: "Abilities");

            migrationBuilder.DropTable(
                name: "BleedEffects",
                schema: "Effects");

            migrationBuilder.DropTable(
                name: "BleedingWoundEffects",
                schema: "Effects");

            migrationBuilder.DropTable(
                name: "ComplexArmCritEffects",
                schema: "Effects");

            migrationBuilder.DropTable(
                name: "ComplexHead1CritEffects",
                schema: "Effects");

            migrationBuilder.DropTable(
                name: "ComplexHead2CritEffects",
                schema: "Effects");

            migrationBuilder.DropTable(
                name: "ComplexLegCritEffects",
                schema: "Effects");

            migrationBuilder.DropTable(
                name: "ComplexTailCritEffects",
                schema: "Effects");

            migrationBuilder.DropTable(
                name: "ComplexTorso1CritEffects",
                schema: "Effects");

            migrationBuilder.DropTable(
                name: "ComplexTorso2CritEffects",
                schema: "Effects");

            migrationBuilder.DropTable(
                name: "ComplexWingCritEffects",
                schema: "Effects");

            migrationBuilder.DropTable(
                name: "DeadEffects",
                schema: "Effects");

            migrationBuilder.DropTable(
                name: "DeadlyArmCritEffects",
                schema: "Effects");

            migrationBuilder.DropTable(
                name: "DeadlyHead1CritEffects",
                schema: "Effects");

            migrationBuilder.DropTable(
                name: "DeadlyHead2CritEffects",
                schema: "Effects");

            migrationBuilder.DropTable(
                name: "DeadlyLegCritEffects",
                schema: "Effects");

            migrationBuilder.DropTable(
                name: "DeadlyTailCritEffects",
                schema: "Effects");

            migrationBuilder.DropTable(
                name: "DeadlyTorso1CritEffects",
                schema: "Effects");

            migrationBuilder.DropTable(
                name: "DeadlyTorso2CritEffects",
                schema: "Effects");

            migrationBuilder.DropTable(
                name: "DeadlyWingCritEffects",
                schema: "Effects");

            migrationBuilder.DropTable(
                name: "DifficultArmCritEffects",
                schema: "Effects");

            migrationBuilder.DropTable(
                name: "DifficultHead1CritEffects",
                schema: "Effects");

            migrationBuilder.DropTable(
                name: "DifficultHead2CritEffects",
                schema: "Effects");

            migrationBuilder.DropTable(
                name: "DifficultLegCritEffects",
                schema: "Effects");

            migrationBuilder.DropTable(
                name: "DifficultTailCritEffects",
                schema: "Effects");

            migrationBuilder.DropTable(
                name: "DifficultTorso1CritEffects",
                schema: "Effects");

            migrationBuilder.DropTable(
                name: "DifficultTorso2CritEffects",
                schema: "Effects");

            migrationBuilder.DropTable(
                name: "DifficultWingCritEffects",
                schema: "Effects");

            migrationBuilder.DropTable(
                name: "DyingEffects",
                schema: "Effects");

            migrationBuilder.DropTable(
                name: "FireEffects",
                schema: "Effects");

            migrationBuilder.DropTable(
                name: "FreezeEffects",
                schema: "Effects");

            migrationBuilder.DropTable(
                name: "PoisonEffects",
                schema: "Effects");

            migrationBuilder.DropTable(
                name: "SimpleArmCritEffects",
                schema: "Effects");

            migrationBuilder.DropTable(
                name: "SimpleHead1CritEffects",
                schema: "Effects");

            migrationBuilder.DropTable(
                name: "SimpleHead2CritEffects",
                schema: "Effects");

            migrationBuilder.DropTable(
                name: "SimpleLegCritEffects",
                schema: "Effects");

            migrationBuilder.DropTable(
                name: "SimpleTailCritEffects",
                schema: "Effects");

            migrationBuilder.DropTable(
                name: "SimpleTorso1CritEffects",
                schema: "Effects");

            migrationBuilder.DropTable(
                name: "SimpleTorso2CritEffects",
                schema: "Effects");

            migrationBuilder.DropTable(
                name: "SimpleWingCritEffects",
                schema: "Effects");

            migrationBuilder.DropTable(
                name: "StaggeredEffects",
                schema: "Effects");

            migrationBuilder.DropTable(
                name: "StunEffects",
                schema: "Effects");

            migrationBuilder.DropTable(
                name: "SufflocationEffects",
                schema: "Effects");

            migrationBuilder.DropTable(
                name: "CritEffects",
                schema: "Battles");

            migrationBuilder.DropTable(
                name: "Effects",
                schema: "Battles");

            migrationBuilder.DropIndex(
                name: "IX_Abilities_DamageTypeId",
                schema: "GameRules",
                table: "Abilities");

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Conditions",
                keyColumn: "Id",
                keyValue: new Guid("208ed04e-73aa-4e57-bb58-26c807fcf558"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Conditions",
                keyColumn: "Id",
                keyValue: new Guid("208ed04e-73aa-4e57-bb58-26c807fcf559"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Conditions",
                keyColumn: "Id",
                keyValue: new Guid("208ed04e-73aa-4e57-bb58-26c807fcf560"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Conditions",
                keyColumn: "Id",
                keyValue: new Guid("208ed04e-73aa-4e57-bb58-26c807fcf561"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Conditions",
                keyColumn: "Id",
                keyValue: new Guid("208ed04e-73aa-4e57-bb58-26c807fcf562"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Conditions",
                keyColumn: "Id",
                keyValue: new Guid("208ed04e-73aa-4e57-bb58-26c807fcf563"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Conditions",
                keyColumn: "Id",
                keyValue: new Guid("208ed04e-73aa-4e57-bb58-26c807fcf564"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Conditions",
                keyColumn: "Id",
                keyValue: new Guid("208ed04e-73aa-4e57-bb58-26c807fcf565"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Conditions",
                keyColumn: "Id",
                keyValue: new Guid("208ed04e-73aa-4e57-bb58-26c807fcf566"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Conditions",
                keyColumn: "Id",
                keyValue: new Guid("208ed04e-73aa-4e57-bb58-26c807fcf567"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Conditions",
                keyColumn: "Id",
                keyValue: new Guid("208ed04e-73aa-4e57-bb58-26c807fcf568"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Conditions",
                keyColumn: "Id",
                keyValue: new Guid("208ed04e-73aa-4e57-bb58-26c807fcf569"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Conditions",
                keyColumn: "Id",
                keyValue: new Guid("208ed04e-73aa-4e57-bb58-26c807fcf570"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Conditions",
                keyColumn: "Id",
                keyValue: new Guid("208ed04e-73aa-4e57-bb58-26c807fcf571"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Conditions",
                keyColumn: "Id",
                keyValue: new Guid("208ed04e-73aa-4e57-bb58-26c807fcf572"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Conditions",
                keyColumn: "Id",
                keyValue: new Guid("208ed04e-73aa-4e57-bb58-26c807fcf573"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Conditions",
                keyColumn: "Id",
                keyValue: new Guid("208ed04e-73aa-4e57-bb58-26c807fcf574"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Conditions",
                keyColumn: "Id",
                keyValue: new Guid("208ed04e-73aa-4e57-bb58-26c807fcf575"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Conditions",
                keyColumn: "Id",
                keyValue: new Guid("208ed04e-73aa-4e57-bb58-26c807fcf576"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Conditions",
                keyColumn: "Id",
                keyValue: new Guid("208ed04e-73aa-4e57-bb58-26c807fcf577"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Conditions",
                keyColumn: "Id",
                keyValue: new Guid("208ed04e-73aa-4e57-bb58-26c807fcf578"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Conditions",
                keyColumn: "Id",
                keyValue: new Guid("208ed04e-73aa-4e57-bb58-26c807fcf579"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Conditions",
                keyColumn: "Id",
                keyValue: new Guid("208ed04e-73aa-4e57-bb58-26c807fcf580"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Conditions",
                keyColumn: "Id",
                keyValue: new Guid("208ed04e-73aa-4e57-bb58-26c807fcf581"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Conditions",
                keyColumn: "Id",
                keyValue: new Guid("208ed04e-73aa-4e57-bb58-26c807fcf582"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Conditions",
                keyColumn: "Id",
                keyValue: new Guid("208ed04e-73aa-4e57-bb58-26c807fcf583"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Conditions",
                keyColumn: "Id",
                keyValue: new Guid("208ed04e-73aa-4e57-bb58-26c807fcf584"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Conditions",
                keyColumn: "Id",
                keyValue: new Guid("208ed04e-73aa-4e57-bb58-26c807fcf585"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Conditions",
                keyColumn: "Id",
                keyValue: new Guid("208ed04e-73aa-4e57-bb58-26c807fcf586"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Conditions",
                keyColumn: "Id",
                keyValue: new Guid("208ed04e-73aa-4e57-bb58-26c807fcf587"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Conditions",
                keyColumn: "Id",
                keyValue: new Guid("208ed04e-73aa-4e57-bb58-26c807fcf588"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Conditions",
                keyColumn: "Id",
                keyValue: new Guid("208ed04e-73aa-4e57-bb58-26c807fcf589"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Conditions",
                keyColumn: "Id",
                keyValue: new Guid("7794e0d0-3147-4791-9053-9667cbe127d7"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Conditions",
                keyColumn: "Id",
                keyValue: new Guid("8895e0d0-3147-4791-9053-9667cbe127d7"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Conditions",
                keyColumn: "Id",
                keyValue: new Guid("8895e0d1-3147-4791-9053-9667cbe127d7"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Conditions",
                keyColumn: "Id",
                keyValue: new Guid("afb1c2ac-f6ab-035e-aedd-011da6f5ea9a"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Conditions",
                keyColumn: "Id",
                keyValue: new Guid("afb1c2ac-f6ab-435e-aedd-011da6f5ea9a"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Conditions",
                keyColumn: "Id",
                keyValue: new Guid("afb1c2ac-f6ab-535e-aedd-011da6f5ea9a"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Conditions",
                keyColumn: "Id",
                keyValue: new Guid("afb1c2ac-f6ab-635e-aedd-011da6f5ea9a"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Conditions",
                keyColumn: "Id",
                keyValue: new Guid("afb1c2ac-f6ab-735e-aedd-011da6f5ea9a"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Conditions",
                keyColumn: "Id",
                keyValue: new Guid("afb1c2ac-f6ab-835e-aedd-011da6f5ea9a"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Conditions",
                keyColumn: "Id",
                keyValue: new Guid("afb1c2ac-f6ab-935e-aedd-011da6f5ea9a"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("32ee830e-7bee-4924-9ddf-1070ceffecdd"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("4fcbd3d6-fde0-47c1-899d-a8c82c291751"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("754ef5e9-8960-4c38-a1be-a3c43c92b1cd"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f00eea-10d5-426e-87a6-f6b8046c47da"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f01eea-10d5-426e-87a6-f6b8046c47da"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f03eea-10d5-426e-87a6-f6b8046c47da"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f04eea-10d5-426e-87a6-f6b8046c47da"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f05eea-10d5-426e-87a6-f6b8046c47da"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f06eea-10d5-426e-87a6-f6b8046c47da"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f07eea-10d5-426e-87a6-f6b8046c47da"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f08eea-10d5-426e-87a6-f6b8046c47da"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f09eea-10d5-426e-87a6-f6b8046c47da"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f10eea-10d5-426e-87a6-f6b8046c47da"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f11eea-10d5-428e-87a6-f6b8046c47da"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f12eea-10d5-428e-87a6-f6b8046c47da"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f13eea-10d5-726e-87a6-f6b8046c47da"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f14eea-10d5-826e-87a6-f6b8046c47da"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f15eea-10d5-826e-87a6-f6b8046c47da"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f16eea-10d5-826e-87a6-f6b8046c47da"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f17eea-10d5-826e-87a6-f6b8046c47da"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f18eea-10d5-826e-87a6-f6b8046c47da"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f19eea-10d5-826e-87a6-f6b8046c47da"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f20eea-10d5-826e-87a6-f6b8046c47da"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f21eea-10d5-826e-87a6-f6b8046c47da"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f22eea-10d5-826e-87a6-f6b8046c47da"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f23eea-10d5-826e-87a6-f6b8046c47da"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f24eea-10d5-826e-87a6-f6b8046c47da"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f25eea-10d5-026e-87a6-f6b8046c47da"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f26eea-10d5-026e-87a6-f6b8046c47da"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f27eea-10d5-026e-87a6-f6b8046c47da"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f28eea-10d5-026e-87a6-f6b8046c47da"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f29eea-10d5-026e-87a6-f6b8046c47da"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f30eea-10d5-026e-87a6-f6b8046c47da"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f31eea-10d5-026e-87a6-f6b8046c47da"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f32eea-10d5-026e-87a6-f6b8046c47da"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f33eea-10d5-026e-87a6-f6b8046c47da"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f34eea-10d5-026e-87a6-f6b8046c47da"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f99eea-10d5-026e-87a6-f6b8046c47da"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f99eea-10d5-420e-87a6-f6b8046c47da"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f99eea-10d5-426e-87a6-f6b8046c47da"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f99eea-10d5-427e-87a6-f6b8046c47da"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f99eea-10d5-428e-87a6-f6b8046c47da"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f99eea-10d5-429e-87a6-f6b8046c47da"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f99eea-10d5-436e-87a6-f6b8046c47da"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f99eea-10d5-446e-87a6-f6b8046c47da"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f99eea-10d5-456e-87a6-f6b8046c47da"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f99eea-10d5-466e-87a6-f6b8046c47da"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f99eea-10d5-476e-87a6-f6b8046c47da"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f99eea-10d5-486e-87a6-f6b8046c47da"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f99eea-10d5-496e-87a6-f6b8046c47da"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f99eea-10d5-506e-87a6-f6b8046c47da"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f99eea-10d5-526e-87a6-f6b8046c47da"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f99eea-10d5-626e-87a6-f6b8046c47da"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f99eea-10d5-726e-87a6-f6b8046c47da"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f99eea-10d5-826e-87a6-f6b8046c47da"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f99eea-10d5-926e-87a6-f6b8046c47da"));

            migrationBuilder.DropColumn(
                name: "LeadingArmId",
                schema: "Battles",
                table: "Creatures");

            migrationBuilder.DropColumn(
                name: "MaxBody",
                schema: "Battles",
                table: "Creatures");

            migrationBuilder.DropColumn(
                name: "MaxCra",
                schema: "Battles",
                table: "Creatures");

            migrationBuilder.DropColumn(
                name: "MaxDex",
                schema: "Battles",
                table: "Creatures");

            migrationBuilder.DropColumn(
                name: "MaxEmp",
                schema: "Battles",
                table: "Creatures");

            migrationBuilder.DropColumn(
                name: "MaxHP",
                schema: "Battles",
                table: "Creatures");

            migrationBuilder.DropColumn(
                name: "MaxInt",
                schema: "Battles",
                table: "Creatures");

            migrationBuilder.DropColumn(
                name: "MaxLuck",
                schema: "Battles",
                table: "Creatures");

            migrationBuilder.DropColumn(
                name: "MaxRef",
                schema: "Battles",
                table: "Creatures");

            migrationBuilder.DropColumn(
                name: "MaxSpeed",
                schema: "Battles",
                table: "Creatures");

            migrationBuilder.DropColumn(
                name: "MaxSta",
                schema: "Battles",
                table: "Creatures");

            migrationBuilder.DropColumn(
                name: "MaxWill",
                schema: "Battles",
                table: "Creatures");

            migrationBuilder.DropColumn(
                name: "Stun",
                schema: "Battles",
                table: "Creatures");

            migrationBuilder.DropColumn(
                name: "MaxValue",
                schema: "Battles",
                table: "CreatureParameters");

            migrationBuilder.DropColumn(
                name: "Round",
                schema: "Battles",
                table: "Battles");

            migrationBuilder.DropColumn(
                name: "DamageTypeId",
                schema: "GameRules",
                table: "Abilities");

            migrationBuilder.AlterColumn<string>(
                name: "StatName",
                schema: "GameRules",
                table: "Skills",
                type: "text",
                nullable: true,
                comment: "Название корреспондирующей характеристики",
                oldClrType: typeof(string),
                oldType: "text",
                oldComment: "Название корреспондирующей характеристики");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "GameRules",
                table: "Skills",
                type: "text",
                nullable: true,
                comment: "Описание навыка");

            migrationBuilder.AddColumn<Guid>(
                name: "GameId",
                schema: "GameRules",
                table: "Skills",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "Айди игры");

            migrationBuilder.AddColumn<int>(
                name: "MaxValueSkills",
                schema: "GameRules",
                table: "Skills",
                type: "integer",
                nullable: true,
                comment: "Максимальное значение навыка");

            migrationBuilder.AddColumn<int>(
                name: "MinValueSkills",
                schema: "GameRules",
                table: "Skills",
                type: "integer",
                nullable: true,
                comment: "Минимальное значение навыка");

            migrationBuilder.AlterColumn<string>(
                name: "StatName",
                schema: "GameRules",
                table: "CreatureTemplateParameters",
                type: "text",
                nullable: true,
                comment: "Название корреспондирующей характеристики",
                oldClrType: typeof(string),
                oldType: "text",
                oldComment: "Название корреспондирующей характеристики");

            migrationBuilder.AlterColumn<string>(
                name: "StatName",
                schema: "Battles",
                table: "CreatureParameters",
                type: "text",
                nullable: true,
                comment: "Название корреспондирующей характеристики",
                oldClrType: typeof(string),
                oldType: "text",
                oldComment: "Название корреспондирующей характеристики");

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
                name: "CurrentConditions",
                schema: "Battles",
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
                        principalSchema: "Battles",
                        principalTable: "Creatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Skills_GameId",
                schema: "GameRules",
                table: "Skills",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_AbilityDamageTypes_DamageTypesId",
                schema: "GameRules",
                table: "AbilityDamageTypes",
                column: "DamageTypesId");

            migrationBuilder.CreateIndex(
                name: "IX_CurrentConditions_CreaturesId",
                schema: "Battles",
                table: "CurrentConditions",
                column: "CreaturesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_Games_GameId",
                schema: "GameRules",
                table: "Skills",
                column: "GameId",
                principalSchema: "BaseGame",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
