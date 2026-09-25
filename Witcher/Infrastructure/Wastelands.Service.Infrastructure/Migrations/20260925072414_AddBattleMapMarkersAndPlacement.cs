using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wastelands.Service.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBattleMapMarkersAndPlacement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Creature_BattleId",
                table: "Creature");

            migrationBuilder.DropIndex(
                name: "IX_BattleCharacter_BattleId",
                table: "BattleCharacter");

            migrationBuilder.AddColumn<int>(
                name: "MapColumn",
                table: "Creature",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MapRow",
                table: "Creature",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MarkerText",
                table: "BattleMapHex",
                type: "varchar(500)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MapColumn",
                table: "BattleCharacter",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MapRow",
                table: "BattleCharacter",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "BattleMapId",
                table: "Battle",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Creature_BattleId_MapColumn_MapRow",
                table: "Creature",
                columns: new[] { "BattleId", "MapColumn", "MapRow" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BattleCharacter_BattleId_MapColumn_MapRow",
                table: "BattleCharacter",
                columns: new[] { "BattleId", "MapColumn", "MapRow" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Battle_BattleMapId",
                table: "Battle",
                column: "BattleMapId");

            migrationBuilder.AddForeignKey(
                name: "FK_Battle_BattleMap_BattleMapId",
                table: "Battle",
                column: "BattleMapId",
                principalTable: "BattleMap",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Battle_BattleMap_BattleMapId",
                table: "Battle");

            migrationBuilder.DropIndex(
                name: "IX_Creature_BattleId_MapColumn_MapRow",
                table: "Creature");

            migrationBuilder.DropIndex(
                name: "IX_BattleCharacter_BattleId_MapColumn_MapRow",
                table: "BattleCharacter");

            migrationBuilder.DropIndex(
                name: "IX_Battle_BattleMapId",
                table: "Battle");

            migrationBuilder.DropColumn(
                name: "MapColumn",
                table: "Creature");

            migrationBuilder.DropColumn(
                name: "MapRow",
                table: "Creature");

            migrationBuilder.DropColumn(
                name: "MarkerText",
                table: "BattleMapHex");

            migrationBuilder.DropColumn(
                name: "MapColumn",
                table: "BattleCharacter");

            migrationBuilder.DropColumn(
                name: "MapRow",
                table: "BattleCharacter");

            migrationBuilder.DropColumn(
                name: "BattleMapId",
                table: "Battle");

            migrationBuilder.CreateIndex(
                name: "IX_Creature_BattleId",
                table: "Creature",
                column: "BattleId");

            migrationBuilder.CreateIndex(
                name: "IX_BattleCharacter_BattleId",
                table: "BattleCharacter",
                column: "BattleId");
        }
    }
}
