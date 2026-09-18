using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Wastelands.Service.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddItemsAndItemTemplates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "EquippedItemId",
                table: "Ability",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Item",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CharacterId = table.Column<long>(type: "bigint", nullable: false),
                    ItemTemplateId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", nullable: false),
                    Description = table.Column<string>(type: "varchar(500)", nullable: true),
                    ItemType = table.Column<int>(type: "integer", nullable: false),
                    Weight = table.Column<double>(type: "double precision", nullable: false),
                    Cost = table.Column<int>(type: "integer", nullable: false),
                    AttackSkill = table.Column<int>(type: "integer", nullable: true),
                    AttacksPerTurn = table.Column<int>(type: "integer", nullable: true),
                    DamageDiceCount = table.Column<int>(type: "integer", nullable: true),
                    DamageModifier = table.Column<int>(type: "integer", nullable: true),
                    DamageType = table.Column<int>(type: "integer", nullable: true),
                    WeaponKind = table.Column<int>(type: "integer", nullable: true),
                    AttackRange = table.Column<int>(type: "integer", nullable: true),
                    HandsRequired = table.Column<int>(type: "integer", nullable: true),
                    Durability = table.Column<int>(type: "integer", nullable: true),
                    IsEquipped = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Item", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Item_Character_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Character",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemTemplate",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GameId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", nullable: false),
                    Description = table.Column<string>(type: "varchar(500)", nullable: true),
                    ItemType = table.Column<int>(type: "integer", nullable: false),
                    Weight = table.Column<double>(type: "double precision", nullable: false),
                    Cost = table.Column<int>(type: "integer", nullable: false),
                    AttackSkill = table.Column<int>(type: "integer", nullable: true),
                    AttacksPerTurn = table.Column<int>(type: "integer", nullable: true),
                    DamageDiceCount = table.Column<int>(type: "integer", nullable: true),
                    DamageModifier = table.Column<int>(type: "integer", nullable: true),
                    DamageType = table.Column<int>(type: "integer", nullable: true),
                    WeaponKind = table.Column<int>(type: "integer", nullable: true),
                    AttackRange = table.Column<int>(type: "integer", nullable: true),
                    HandsRequired = table.Column<int>(type: "integer", nullable: true),
                    Durability = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemTemplate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ItemAppliedCondition",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ItemId = table.Column<long>(type: "bigint", nullable: false),
                    Condition = table.Column<int>(type: "integer", nullable: false),
                    ApplyChance = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemAppliedCondition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemAppliedCondition_Item_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Item",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemTemplateAppliedCondition",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ItemTemplateId = table.Column<long>(type: "bigint", nullable: false),
                    Condition = table.Column<int>(type: "integer", nullable: false),
                    ApplyChance = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemTemplateAppliedCondition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemTemplateAppliedCondition_ItemTemplate_ItemTemplateId",
                        column: x => x.ItemTemplateId,
                        principalTable: "ItemTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Item_CharacterId",
                table: "Item",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemAppliedCondition_ItemId",
                table: "ItemAppliedCondition",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemTemplateAppliedCondition_ItemTemplateId",
                table: "ItemTemplateAppliedCondition",
                column: "ItemTemplateId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemAppliedCondition");

            migrationBuilder.DropTable(
                name: "ItemTemplateAppliedCondition");

            migrationBuilder.DropTable(
                name: "Item");

            migrationBuilder.DropTable(
                name: "ItemTemplate");

            migrationBuilder.DropColumn(
                name: "EquippedItemId",
                table: "Ability");
        }
    }
}
