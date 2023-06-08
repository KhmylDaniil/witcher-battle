using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Witcher.Storage.Postgresql.Migrations
{
    public partial class MovedIsEquippedProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEquipped",
                schema: "Items",
                table: "Weapons");

            migrationBuilder.AddColumn<bool>(
                name: "IsEquipped",
                schema: "Items",
                table: "Items",
                type: "boolean",
                nullable: true,
                comment: "Экипировано");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEquipped",
                schema: "Items",
                table: "Items");

            migrationBuilder.AddColumn<bool>(
                name: "IsEquipped",
                schema: "Items",
                table: "Weapons",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "Экипировано");
        }
    }
}
