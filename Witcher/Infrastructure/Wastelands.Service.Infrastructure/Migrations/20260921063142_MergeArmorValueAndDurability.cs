using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wastelands.Service.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MergeArmorValueAndDurability : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArmorValue",
                table: "ItemTemplateArmorPart");

            migrationBuilder.DropColumn(
                name: "ArmorValue",
                table: "ItemArmorPart");

            migrationBuilder.RenameColumn(
                name: "MaxDurability",
                table: "ItemTemplateArmorPart",
                newName: "Armor");

            migrationBuilder.RenameColumn(
                name: "MaxDurability",
                table: "ItemArmorPart",
                newName: "Armor");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Armor",
                table: "ItemTemplateArmorPart",
                newName: "MaxDurability");

            migrationBuilder.RenameColumn(
                name: "Armor",
                table: "ItemArmorPart",
                newName: "MaxDurability");

            migrationBuilder.AddColumn<int>(
                name: "ArmorValue",
                table: "ItemTemplateArmorPart",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ArmorValue",
                table: "ItemArmorPart",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
