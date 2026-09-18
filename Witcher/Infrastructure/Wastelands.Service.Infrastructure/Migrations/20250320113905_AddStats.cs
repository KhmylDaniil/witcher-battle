using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wastelands.Service.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddStats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Login",
                table: "Character",
                newName: "Name");

            migrationBuilder.AddColumn<int>(
                name: "Cra",
                table: "Character",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "Craft");

            migrationBuilder.AddColumn<int>(
                name: "Dex",
                table: "Character",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "Dexterity");

            migrationBuilder.AddColumn<int>(
                name: "Emp",
                table: "Character",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "Empathy");

            migrationBuilder.AddColumn<int>(
                name: "Int",
                table: "Character",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "Intellect");

            migrationBuilder.AddColumn<int>(
                name: "Rea",
                table: "Character",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "Reaction");

            migrationBuilder.AddColumn<int>(
                name: "Str",
                table: "Character",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "Strength");

            migrationBuilder.AddColumn<int>(
                name: "Wil",
                table: "Character",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "Willpower");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cra",
                table: "Character");

            migrationBuilder.DropColumn(
                name: "Dex",
                table: "Character");

            migrationBuilder.DropColumn(
                name: "Emp",
                table: "Character");

            migrationBuilder.DropColumn(
                name: "Int",
                table: "Character");

            migrationBuilder.DropColumn(
                name: "Rea",
                table: "Character");

            migrationBuilder.DropColumn(
                name: "Str",
                table: "Character");

            migrationBuilder.DropColumn(
                name: "Wil",
                table: "Character");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Character",
                newName: "Login");
        }
    }
}
