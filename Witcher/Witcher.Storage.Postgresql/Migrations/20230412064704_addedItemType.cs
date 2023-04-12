using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Witcher.Storage.Postgresql.Migrations
{
    public partial class addedItemType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Range",
                schema: "Items",
                table: "WeaponTeemplates",
                type: "integer",
                nullable: true,
                comment: "Дальность");

            migrationBuilder.AddColumn<string>(
                name: "ItemType",
                schema: "Items",
                table: "ItemTemplates",
                type: "text",
                nullable: false,
                defaultValue: "",
                comment: "Тип предмета");

            migrationBuilder.AddColumn<string>(
                name: "ItemType",
                schema: "Items",
                table: "Items",
                type: "text",
                nullable: false,
                defaultValue: "",
                comment: "Тип предмета");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Range",
                schema: "Items",
                table: "WeaponTeemplates");

            migrationBuilder.DropColumn(
                name: "ItemType",
                schema: "Items",
                table: "ItemTemplates");

            migrationBuilder.DropColumn(
                name: "ItemType",
                schema: "Items",
                table: "Items");
        }
    }
}
