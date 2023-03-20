using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Witcher.Storage.Postgresql.Migrations
{
    public partial class addTurnBeginngingIsTriggered : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "TurnBeginningEffectsAreTriggered",
                schema: "Battles",
                table: "Creatures",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TurnBeginningEffectsAreTriggered",
                schema: "Battles",
                table: "Creatures");
        }
    }
}
