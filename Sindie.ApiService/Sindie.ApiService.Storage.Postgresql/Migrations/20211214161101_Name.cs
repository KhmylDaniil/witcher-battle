using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sindie.ApiService.Storage.Postgresql.Migrations
{
    public partial class Name : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OuantityItem",
                table: "BodyItem");

            migrationBuilder.RenameColumn(
                name: "MinValueParameter",
                table: "Parameters",
                newName: "ParameterBounds_MinValueParameter");

            migrationBuilder.RenameColumn(
                name: "MaxValueParameter",
                table: "Parameters",
                newName: "ParameterBounds_MaxValueParameter");

            migrationBuilder.RenameColumn(
                name: "OccupiedBagSize",
                table: "Items",
                newName: "Weight");

            migrationBuilder.RenameColumn(
                name: "OuantityItem",
                table: "BagItems",
                newName: "QuantityItem");

            migrationBuilder.AddColumn<int>(
                name: "CounterWearing_QuantityItem",
                table: "BodyItem",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CounterWearing_TimeWear",
                table: "BodyItem",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BodySlotActivity_CounterWearing",
                table: "Body",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "BodySlotActivity_InActivationTime",
                table: "Body",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BodySlotActivity_MaxQuantityWearing",
                table: "Body",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BodySlotActivity_PeriodOfInactivity",
                table: "Body",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Characteristic",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    GameId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characteristic", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Characteristic_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CharacterCharacteristic",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    CharacteristicId = table.Column<Guid>(type: "uuid", nullable: false),
                    CharacterId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterCharacteristic", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CharacterCharacteristic_Characteristic_CharacteristicId",
                        column: x => x.CharacteristicId,
                        principalTable: "Characteristic",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterCharacteristic_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CharacteristicModifier",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    CharacteristicId = table.Column<Guid>(type: "uuid", nullable: false),
                    ModifierId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacteristicModifier", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CharacteristicModifier_Characteristic_CharacteristicId",
                        column: x => x.CharacteristicId,
                        principalTable: "Characteristic",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacteristicModifier_Modifiers_ModifierId",
                        column: x => x.ModifierId,
                        principalTable: "Modifiers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CharacterCharacteristic_CharacterId",
                table: "CharacterCharacteristic",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterCharacteristic_CharacteristicId",
                table: "CharacterCharacteristic",
                column: "CharacteristicId");

            migrationBuilder.CreateIndex(
                name: "IX_Characteristic_GameId",
                table: "Characteristic",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacteristicModifier_CharacteristicId",
                table: "CharacteristicModifier",
                column: "CharacteristicId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacteristicModifier_ModifierId",
                table: "CharacteristicModifier",
                column: "ModifierId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CharacterCharacteristic");

            migrationBuilder.DropTable(
                name: "CharacteristicModifier");

            migrationBuilder.DropTable(
                name: "Characteristic");

            migrationBuilder.DropColumn(
                name: "CounterWearing_QuantityItem",
                table: "BodyItem");

            migrationBuilder.DropColumn(
                name: "CounterWearing_TimeWear",
                table: "BodyItem");

            migrationBuilder.DropColumn(
                name: "BodySlotActivity_CounterWearing",
                table: "Body");

            migrationBuilder.DropColumn(
                name: "BodySlotActivity_InActivationTime",
                table: "Body");

            migrationBuilder.DropColumn(
                name: "BodySlotActivity_MaxQuantityWearing",
                table: "Body");

            migrationBuilder.DropColumn(
                name: "BodySlotActivity_PeriodOfInactivity",
                table: "Body");

            migrationBuilder.RenameColumn(
                name: "ParameterBounds_MinValueParameter",
                table: "Parameters",
                newName: "MinValueParameter");

            migrationBuilder.RenameColumn(
                name: "ParameterBounds_MaxValueParameter",
                table: "Parameters",
                newName: "MaxValueParameter");

            migrationBuilder.RenameColumn(
                name: "Weight",
                table: "Items",
                newName: "OccupiedBagSize");

            migrationBuilder.RenameColumn(
                name: "QuantityItem",
                table: "BagItems",
                newName: "OuantityItem");

            migrationBuilder.AddColumn<int>(
                name: "OuantityItem",
                table: "BodyItem",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
