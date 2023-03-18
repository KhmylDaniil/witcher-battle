using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Witcher.Storage.Postgresql.Migrations
{
    public partial class battleLogProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BattleLog",
                schema: "Battles",
                table: "Battles",
                type: "text",
                nullable: true,
                comment: "Журнал боя");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BattleLog",
                schema: "Battles",
                table: "Battles");
        }
    }
}
