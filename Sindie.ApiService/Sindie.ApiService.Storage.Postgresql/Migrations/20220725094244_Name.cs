using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sindie.ApiService.Storage.Postgresql.Migrations
{
    public partial class Name : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PoisonEffects",
                schema: "Battles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    Toxicity = table.Column<string>(type: "text", nullable: false, comment: "Тяжесть")
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PoisonEffects",
                schema: "Battles");
        }
    }
}
