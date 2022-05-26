using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sindie.ApiService.Storage.Postgresql.Migrations
{
    public partial class Mod : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Modifiers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    GameId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    ModifierActivity_ModifierActivation_ActivationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ModifierActivity_ModifierActivation_ActivationFromStartGame = table.Column<bool>(type: "boolean", nullable: true),
                    ModifierActivity_ModifierActivation_ActivationFromWearingInSlot = table.Column<bool>(type: "boolean", nullable: true),
                    ModifierActivity_Periodicity_PeriodOfActivity = table.Column<int>(type: "integer", nullable: true),
                    ModifierActivity_Periodicity_PeriodOfInactivity = table.Column<int>(type: "integer", nullable: true),
                    ModifierActivity_Periodicity_NumberOfRepetitions = table.Column<int>(type: "integer", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modifiers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Modifiers_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Modifiers_GameId",
                table: "Modifiers",
                column: "GameId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Modifiers");
        }
    }
}
