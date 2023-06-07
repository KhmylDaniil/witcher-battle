using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Witcher.Storage.Postgresql.Migrations
{
    public partial class ArmorParts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CreatureParts_BodyParts_Id",
                schema: "Battles",
                table: "CreatureParts");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                schema: "Battles",
                table: "CreatureParts",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)");

            migrationBuilder.AddColumn<int>(
                name: "BodyPartType",
                schema: "Battles",
                table: "CreatureParts",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedByUserId",
                schema: "Battles",
                table: "CreatureParts",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                schema: "Battles",
                table: "CreatureParts",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now() at time zone 'utc'");

            migrationBuilder.AddColumn<double>(
                name: "DamageModifier",
                schema: "Battles",
                table: "CreatureParts",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "HitPenalty",
                schema: "Battles",
                table: "CreatureParts",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaxToHit",
                schema: "Battles",
                table: "CreatureParts",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MinToHit",
                schema: "Battles",
                table: "CreatureParts",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedByUserId",
                schema: "Battles",
                table: "CreatureParts",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                schema: "Battles",
                table: "CreatureParts",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "Battles",
                table: "CreatureParts",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoleCreatedUser",
                schema: "Battles",
                table: "CreatureParts",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoleModifiedUser",
                schema: "Battles",
                table: "CreatureParts",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Armors",
                schema: "Items",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Armors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Armors_Items_Id",
                        column: x => x.Id,
                        principalSchema: "Items",
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Броня");

            migrationBuilder.CreateTable(
                name: "ArmorParts",
                schema: "Items",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    ArmorId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreaturePartId = table.Column<Guid>(type: "uuid", nullable: false),
                    CurrentArmor = table.Column<int>(type: "integer", nullable: false, comment: "Текущая броня"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArmorParts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArmorParts_Armors_ArmorId",
                        column: x => x.ArmorId,
                        principalSchema: "Items",
                        principalTable: "Armors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArmorParts_CreatureParts_CreaturePartId",
                        column: x => x.CreaturePartId,
                        principalSchema: "Battles",
                        principalTable: "CreatureParts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Части брони");

            migrationBuilder.CreateIndex(
                name: "IX_ArmorParts_ArmorId",
                schema: "Items",
                table: "ArmorParts",
                column: "ArmorId");

            migrationBuilder.CreateIndex(
                name: "IX_ArmorParts_CreaturePartId",
                schema: "Items",
                table: "ArmorParts",
                column: "CreaturePartId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArmorParts",
                schema: "Items");

            migrationBuilder.DropTable(
                name: "Armors",
                schema: "Items");

            migrationBuilder.DropColumn(
                name: "BodyPartType",
                schema: "Battles",
                table: "CreatureParts");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                schema: "Battles",
                table: "CreatureParts");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                schema: "Battles",
                table: "CreatureParts");

            migrationBuilder.DropColumn(
                name: "DamageModifier",
                schema: "Battles",
                table: "CreatureParts");

            migrationBuilder.DropColumn(
                name: "HitPenalty",
                schema: "Battles",
                table: "CreatureParts");

            migrationBuilder.DropColumn(
                name: "MaxToHit",
                schema: "Battles",
                table: "CreatureParts");

            migrationBuilder.DropColumn(
                name: "MinToHit",
                schema: "Battles",
                table: "CreatureParts");

            migrationBuilder.DropColumn(
                name: "ModifiedByUserId",
                schema: "Battles",
                table: "CreatureParts");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                schema: "Battles",
                table: "CreatureParts");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "Battles",
                table: "CreatureParts");

            migrationBuilder.DropColumn(
                name: "RoleCreatedUser",
                schema: "Battles",
                table: "CreatureParts");

            migrationBuilder.DropColumn(
                name: "RoleModifiedUser",
                schema: "Battles",
                table: "CreatureParts");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                schema: "Battles",
                table: "CreatureParts",
                type: "uuid",
                nullable: false,
                defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_CreatureParts_BodyParts_Id",
                schema: "Battles",
                table: "CreatureParts",
                column: "Id",
                principalSchema: "GameRules",
                principalTable: "BodyParts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
