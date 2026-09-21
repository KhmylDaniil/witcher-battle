using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Wastelands.Service.Domain.Enums;

#nullable disable

namespace Wastelands.Service.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCriticalWounds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Dictionary<string, Condition>>(
                name: "CriticalWounds",
                table: "Creature",
                type: "jsonb",
                nullable: false,
                defaultValueSql: "'{}'",
                comment: "Критические ранения по слотам (часть тела + тип урона), см. CriticalWoundCatalog");

            migrationBuilder.AddColumn<Dictionary<string, Condition>>(
                name: "CriticalWounds",
                table: "BattleCharacter",
                type: "jsonb",
                nullable: false,
                defaultValueSql: "'{}'",
                comment: "Критические ранения по слотам (часть тела + тип урона), см. CriticalWoundCatalog");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CriticalWounds",
                table: "Creature");

            migrationBuilder.DropColumn(
                name: "CriticalWounds",
                table: "BattleCharacter");
        }
    }
}
