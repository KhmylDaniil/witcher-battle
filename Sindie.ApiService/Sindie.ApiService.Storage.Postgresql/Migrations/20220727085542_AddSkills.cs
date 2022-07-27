using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sindie.ApiService.Storage.Postgresql.Migrations
{
    public partial class AddSkills : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Skills_Games_GameId",
                schema: "GameRules",
                table: "Skills");

            migrationBuilder.DropTable(
                name: "AbilityDamageTypes",
                schema: "GameRules");

            migrationBuilder.DropIndex(
                name: "IX_Skills_GameId",
                schema: "GameRules",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "Description",
                schema: "GameRules",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "GameId",
                schema: "GameRules",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "MaxValueSkills",
                schema: "GameRules",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "MinValueSkills",
                schema: "GameRules",
                table: "Skills");

            migrationBuilder.AddColumn<Guid>(
                name: "DamageTypeId",
                schema: "GameRules",
                table: "Abilities",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.InsertData(
                schema: "GameRules",
                table: "Skills",
                columns: new[] { "Id", "CreatedByUserId", "CreatedOn", "ModifiedByUserId", "ModifiedOn", "Name", "RoleCreatedUser", "RoleModifiedUser", "StatName" },
                values: new object[,]
                {
                    { new Guid("c5f99eea-10d5-026e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "FirstAid", "Default", "Default", "Cra" },
                    { new Guid("c5f99eea-10d5-420e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Staff/Spear", "Default", "Default", "Ref" },
                    { new Guid("c5f99eea-10d5-426e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Brawling", "Default", "Default", "Ref" },
                    { new Guid("c5f99eea-10d5-427e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Dodge", "Default", "Default", "Ref" },
                    { new Guid("c5f99eea-10d5-428e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Melee", "Default", "Default", "Ref" },
                    { new Guid("c5f99eea-10d5-429e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "SmallBlades", "Default", "Default", "Ref" },
                    { new Guid("c5f99eea-10d5-436e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "SpellCasting", "Default", "Default", "Will" },
                    { new Guid("c5f99eea-10d5-446e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "ResistMagic", "Default", "Default", "Will" },
                    { new Guid("c5f99eea-10d5-456e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "ResistCoercion", "Default", "Default", "Will" },
                    { new Guid("c5f99eea-10d5-466e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Needling", "Default", "Default", "Emp" },
                    { new Guid("c5f99eea-10d5-476e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "EyeGouge", "Default", "Default", "Dex" },
                    { new Guid("c5f99eea-10d5-486e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "BleedingWound", "Default", "Default", "Int" },
                    { new Guid("c5f99eea-10d5-496e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "HealingHands", "Default", "Default", "Cra" },
                    { new Guid("c5f99eea-10d5-526e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Swordsmanship", "Default", "Default", "Ref" },
                    { new Guid("c5f99eea-10d5-626e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Archery", "Default", "Default", "Dex" },
                    { new Guid("c5f99eea-10d5-726e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Athletics", "Default", "Default", "Dex" },
                    { new Guid("c5f99eea-10d5-826e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Crossbow", "Default", "Default", "Dex" },
                    { new Guid("c5f99eea-10d5-926e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Endurance", "Default", "Default", "Body" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Abilities_DamageTypeId",
                schema: "GameRules",
                table: "Abilities",
                column: "DamageTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Abilities_DamageTypes_DamageTypeId",
                schema: "GameRules",
                table: "Abilities",
                column: "DamageTypeId",
                principalSchema: "GameRules",
                principalTable: "DamageTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Abilities_DamageTypes_DamageTypeId",
                schema: "GameRules",
                table: "Abilities");

            migrationBuilder.DropIndex(
                name: "IX_Abilities_DamageTypeId",
                schema: "GameRules",
                table: "Abilities");

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f99eea-10d5-026e-87a6-f6b8046c47da"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f99eea-10d5-420e-87a6-f6b8046c47da"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f99eea-10d5-426e-87a6-f6b8046c47da"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f99eea-10d5-427e-87a6-f6b8046c47da"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f99eea-10d5-428e-87a6-f6b8046c47da"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f99eea-10d5-429e-87a6-f6b8046c47da"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f99eea-10d5-436e-87a6-f6b8046c47da"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f99eea-10d5-446e-87a6-f6b8046c47da"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f99eea-10d5-456e-87a6-f6b8046c47da"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f99eea-10d5-466e-87a6-f6b8046c47da"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f99eea-10d5-476e-87a6-f6b8046c47da"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f99eea-10d5-486e-87a6-f6b8046c47da"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f99eea-10d5-496e-87a6-f6b8046c47da"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f99eea-10d5-526e-87a6-f6b8046c47da"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f99eea-10d5-626e-87a6-f6b8046c47da"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f99eea-10d5-726e-87a6-f6b8046c47da"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f99eea-10d5-826e-87a6-f6b8046c47da"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f99eea-10d5-926e-87a6-f6b8046c47da"));

            migrationBuilder.DropColumn(
                name: "DamageTypeId",
                schema: "GameRules",
                table: "Abilities");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "GameRules",
                table: "Skills",
                type: "text",
                nullable: true,
                comment: "Описание навыка");

            migrationBuilder.AddColumn<Guid>(
                name: "GameId",
                schema: "GameRules",
                table: "Skills",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "Айди игры");

            migrationBuilder.AddColumn<int>(
                name: "MaxValueSkills",
                schema: "GameRules",
                table: "Skills",
                type: "integer",
                nullable: true,
                comment: "Максимальное значение навыка");

            migrationBuilder.AddColumn<int>(
                name: "MinValueSkills",
                schema: "GameRules",
                table: "Skills",
                type: "integer",
                nullable: true,
                comment: "Минимальное значение навыка");

            migrationBuilder.CreateTable(
                name: "AbilityDamageTypes",
                schema: "GameRules",
                columns: table => new
                {
                    AbilitiesId = table.Column<Guid>(type: "uuid", nullable: false),
                    DamageTypesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbilityDamageTypes", x => new { x.AbilitiesId, x.DamageTypesId });
                    table.ForeignKey(
                        name: "FK_AbilityDamageTypes_Abilities_AbilitiesId",
                        column: x => x.AbilitiesId,
                        principalSchema: "GameRules",
                        principalTable: "Abilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AbilityDamageTypes_DamageTypes_DamageTypesId",
                        column: x => x.DamageTypesId,
                        principalSchema: "GameRules",
                        principalTable: "DamageTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Skills_GameId",
                schema: "GameRules",
                table: "Skills",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_AbilityDamageTypes_DamageTypesId",
                schema: "GameRules",
                table: "AbilityDamageTypes",
                column: "DamageTypesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_Games_GameId",
                schema: "GameRules",
                table: "Skills",
                column: "GameId",
                principalSchema: "BaseGame",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
