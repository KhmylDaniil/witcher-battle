using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sindie.ApiService.Storage.Postgresql.Migrations
{
    public partial class addActiveCycle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActivationTime",
                schema: "GameRules",
                table: "ModifierScripts");

            migrationBuilder.DropColumn(
                name: "NumberOfRepetitions",
                schema: "GameRules",
                table: "ModifierScripts");

            migrationBuilder.DropColumn(
                name: "PeriodOfActivity",
                schema: "GameRules",
                table: "ModifierScripts");

            migrationBuilder.DropColumn(
                name: "PeriodOfInactivity",
                schema: "GameRules",
                table: "ModifierScripts");

            migrationBuilder.CreateTable(
                name: "ActiveCycles",
                schema: "GameRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    ActivationBeginning = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Начало цикла активности"),
                    ActivationEnd = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Конец цикла активности"),
                    ModifierScriptId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди скрипта модификатора"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActiveCycles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActiveCycles_ModifierScripts_ModifierScriptId",
                        column: x => x.ModifierScriptId,
                        principalSchema: "GameRules",
                        principalTable: "ModifierScripts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Скрипты модифкатора");

            migrationBuilder.CreateIndex(
                name: "IX_ActiveCycles_ModifierScriptId",
                schema: "GameRules",
                table: "ActiveCycles",
                column: "ModifierScriptId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActiveCycles",
                schema: "GameRules");

            migrationBuilder.AddColumn<DateTime>(
                name: "ActivationTime",
                schema: "GameRules",
                table: "ModifierScripts",
                type: "timestamp with time zone",
                nullable: true,
                comment: "Время активации скрипта модификатора");

            migrationBuilder.AddColumn<int>(
                name: "NumberOfRepetitions",
                schema: "GameRules",
                table: "ModifierScripts",
                type: "integer",
                nullable: true,
                comment: "Количество повторений скрипта модификатора");

            migrationBuilder.AddColumn<int>(
                name: "PeriodOfActivity",
                schema: "GameRules",
                table: "ModifierScripts",
                type: "integer",
                nullable: true,
                comment: "Период активности скрипта модификатора");

            migrationBuilder.AddColumn<int>(
                name: "PeriodOfInactivity",
                schema: "GameRules",
                table: "ModifierScripts",
                type: "integer",
                nullable: true,
                comment: "Период неактивности скрипта модификатора");
        }
    }
}
