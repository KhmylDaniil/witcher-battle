using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sindie.ApiService.Storage.Postgresql.Migrations
{
    public partial class AddSomeEffects : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Toxicity",
                schema: "Battles",
                table: "PoisonEffects");

            migrationBuilder.DropColumn(
                name: "Severity",
                schema: "Battles",
                table: "BleedEffects");

            migrationBuilder.EnsureSchema(
                name: "Effects");

            migrationBuilder.RenameTable(
                name: "PoisonEffects",
                schema: "Battles",
                newName: "PoisonEffects",
                newSchema: "Effects");

            migrationBuilder.RenameTable(
                name: "BleedEffects",
                schema: "Battles",
                newName: "BleedEffects",
                newSchema: "Effects");

            migrationBuilder.CreateTable(
                name: "BleedingWoundEffects",
                schema: "Battles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    Severity = table.Column<int>(type: "integer", nullable: false, comment: "Тяжесть"),
                    Damage = table.Column<int>(type: "integer", nullable: false, comment: "Урон")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BleedingWoundEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BleedingWoundEffects_Effects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "Effects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты кровавой раны");

            migrationBuilder.CreateTable(
                name: "FireEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FireEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FireEffects_Effects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "Effects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты горения");

            migrationBuilder.CreateTable(
                name: "FreezeEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    Int = table.Column<int>(type: "integer", nullable: false),
                    Ref = table.Column<int>(type: "integer", nullable: false),
                    Dex = table.Column<int>(type: "integer", nullable: false),
                    Body = table.Column<int>(type: "integer", nullable: false),
                    Emp = table.Column<int>(type: "integer", nullable: false),
                    Cra = table.Column<int>(type: "integer", nullable: false),
                    Will = table.Column<int>(type: "integer", nullable: false),
                    Speed = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FreezeEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FreezeEffects_Effects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "Effects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты заморозки");

            migrationBuilder.InsertData(
                schema: "GameRules",
                table: "Conditions",
                columns: new[] { "Id", "CreatedByUserId", "CreatedOn", "ModifiedByUserId", "ModifiedOn", "Name", "RoleCreatedUser", "RoleModifiedUser" },
                values: new object[,]
                {
                    { new Guid("7794e0d0-3147-4791-9053-9667cbe127d7"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "BleedingWound", "Default", "Default" },
                    { new Guid("8895e0d0-3147-4791-9053-9667cbe127d7"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Fire", "Default", "Default" },
                    { new Guid("8895e0d1-3147-4791-9053-9667cbe127d7"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Freeze", "Default", "Default" }
                });

            migrationBuilder.InsertData(
                schema: "GameRules",
                table: "Skills",
                columns: new[] { "Id", "CreatedByUserId", "CreatedOn", "ModifiedByUserId", "ModifiedOn", "Name", "RoleCreatedUser", "RoleModifiedUser", "StatName" },
                values: new object[] { new Guid("c5f99eea-10d5-506e-87a6-f6b8046c47da"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Physique", "Default", "Default", "Body" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BleedingWoundEffects",
                schema: "Battles");

            migrationBuilder.DropTable(
                name: "FireEffects",
                schema: "Effects");

            migrationBuilder.DropTable(
                name: "FreezeEffects",
                schema: "Effects");

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Conditions",
                keyColumn: "Id",
                keyValue: new Guid("7794e0d0-3147-4791-9053-9667cbe127d7"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Conditions",
                keyColumn: "Id",
                keyValue: new Guid("8895e0d0-3147-4791-9053-9667cbe127d7"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Conditions",
                keyColumn: "Id",
                keyValue: new Guid("8895e0d1-3147-4791-9053-9667cbe127d7"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("c5f99eea-10d5-506e-87a6-f6b8046c47da"));

            migrationBuilder.RenameTable(
                name: "PoisonEffects",
                schema: "Effects",
                newName: "PoisonEffects",
                newSchema: "Battles");

            migrationBuilder.RenameTable(
                name: "BleedEffects",
                schema: "Effects",
                newName: "BleedEffects",
                newSchema: "Battles");

            migrationBuilder.AddColumn<string>(
                name: "Toxicity",
                schema: "Battles",
                table: "PoisonEffects",
                type: "text",
                nullable: false,
                defaultValue: "",
                comment: "Тяжесть");

            migrationBuilder.AddColumn<int>(
                name: "Severity",
                schema: "Battles",
                table: "BleedEffects",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "Тяжесть");
        }
    }
}
