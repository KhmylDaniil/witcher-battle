using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Wastelands.Service.Domain.Enums;

#nullable disable

namespace Wastelands.Service.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddArmorTemplatesAndCombat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Dictionary<DamageType, DamageTypeModifier>>(
                name: "DamageTypeModifiers",
                table: "ItemTemplate",
                type: "jsonb",
                nullable: false,
                defaultValueSql: "'{}'::jsonb",
                comment: "DamageTypeModifiers");

            migrationBuilder.AddColumn<Dictionary<DamageType, DamageTypeModifier>>(
                name: "DamageTypeModifiers",
                table: "Item",
                type: "jsonb",
                nullable: false,
                defaultValueSql: "'{}'::jsonb",
                comment: "DamageTypeModifiers");

            migrationBuilder.AddColumn<int>(
                name: "ResolvedHumanBodyPart",
                table: "BattleAttack",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ItemArmorPart",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ItemId = table.Column<long>(type: "bigint", nullable: false),
                    Part = table.Column<int>(type: "integer", nullable: false),
                    ArmorValue = table.Column<int>(type: "integer", nullable: false),
                    MaxDurability = table.Column<int>(type: "integer", nullable: false),
                    CurrentDurability = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemArmorPart", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemArmorPart_Item_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Item",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemTemplateArmorPart",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ItemTemplateId = table.Column<long>(type: "bigint", nullable: false),
                    Part = table.Column<int>(type: "integer", nullable: false),
                    ArmorValue = table.Column<int>(type: "integer", nullable: false),
                    MaxDurability = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemTemplateArmorPart", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemTemplateArmorPart_ItemTemplate_ItemTemplateId",
                        column: x => x.ItemTemplateId,
                        principalTable: "ItemTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItemArmorPart_ItemId",
                table: "ItemArmorPart",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemTemplateArmorPart_ItemTemplateId",
                table: "ItemTemplateArmorPart",
                column: "ItemTemplateId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemArmorPart");

            migrationBuilder.DropTable(
                name: "ItemTemplateArmorPart");

            migrationBuilder.DropColumn(
                name: "DamageTypeModifiers",
                table: "ItemTemplate");

            migrationBuilder.DropColumn(
                name: "DamageTypeModifiers",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "ResolvedHumanBodyPart",
                table: "BattleAttack");
        }
    }
}
