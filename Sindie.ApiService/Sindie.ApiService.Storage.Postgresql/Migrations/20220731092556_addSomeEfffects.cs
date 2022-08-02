using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sindie.ApiService.Storage.Postgresql.Migrations
{
    public partial class addSomeEfffects : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Body",
                schema: "Effects",
                table: "FreezeEffects");

            migrationBuilder.DropColumn(
                name: "Cra",
                schema: "Effects",
                table: "FreezeEffects");

            migrationBuilder.DropColumn(
                name: "Dex",
                schema: "Effects",
                table: "FreezeEffects");

            migrationBuilder.DropColumn(
                name: "Emp",
                schema: "Effects",
                table: "FreezeEffects");

            migrationBuilder.DropColumn(
                name: "Int",
                schema: "Effects",
                table: "FreezeEffects");

            migrationBuilder.DropColumn(
                name: "Ref",
                schema: "Effects",
                table: "FreezeEffects");

            migrationBuilder.DropColumn(
                name: "Speed",
                schema: "Effects",
                table: "FreezeEffects");

            migrationBuilder.DropColumn(
                name: "Will",
                schema: "Effects",
                table: "FreezeEffects");

            migrationBuilder.AddColumn<int>(
                name: "MaxHP",
                schema: "Battles",
                table: "Creatures",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "Максимальные хиты");

            migrationBuilder.AddColumn<int>(
                name: "Stun",
                schema: "Battles",
                table: "Creatures",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "StunEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StunEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StunEffects_Effects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "Effects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты дезориентации");

            migrationBuilder.InsertData(
                schema: "GameRules",
                table: "Conditions",
                columns: new[] { "Id", "CreatedByUserId", "CreatedOn", "ModifiedByUserId", "ModifiedOn", "Name", "RoleCreatedUser", "RoleModifiedUser" },
                values: new object[,]
                {
                    { new Guid("afb1c2ac-f6ab-035e-aedd-011da6f5ea9a"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Blinded", "Default", "Default" },
                    { new Guid("afb1c2ac-f6ab-435e-aedd-011da6f5ea9a"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Stun", "Default", "Default" },
                    { new Guid("afb1c2ac-f6ab-535e-aedd-011da6f5ea9a"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Staggered", "Default", "Default" },
                    { new Guid("afb1c2ac-f6ab-635e-aedd-011da6f5ea9a"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Intoxication", "Default", "Default" },
                    { new Guid("afb1c2ac-f6ab-735e-aedd-011da6f5ea9a"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Hallutination", "Default", "Default" },
                    { new Guid("afb1c2ac-f6ab-835e-aedd-011da6f5ea9a"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Nausea", "Default", "Default" },
                    { new Guid("afb1c2ac-f6ab-935e-aedd-011da6f5ea9a"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Sufflocation", "Default", "Default" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StunEffects",
                schema: "Effects");

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Conditions",
                keyColumn: "Id",
                keyValue: new Guid("afb1c2ac-f6ab-035e-aedd-011da6f5ea9a"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Conditions",
                keyColumn: "Id",
                keyValue: new Guid("afb1c2ac-f6ab-435e-aedd-011da6f5ea9a"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Conditions",
                keyColumn: "Id",
                keyValue: new Guid("afb1c2ac-f6ab-535e-aedd-011da6f5ea9a"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Conditions",
                keyColumn: "Id",
                keyValue: new Guid("afb1c2ac-f6ab-635e-aedd-011da6f5ea9a"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Conditions",
                keyColumn: "Id",
                keyValue: new Guid("afb1c2ac-f6ab-735e-aedd-011da6f5ea9a"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Conditions",
                keyColumn: "Id",
                keyValue: new Guid("afb1c2ac-f6ab-835e-aedd-011da6f5ea9a"));

            migrationBuilder.DeleteData(
                schema: "GameRules",
                table: "Conditions",
                keyColumn: "Id",
                keyValue: new Guid("afb1c2ac-f6ab-935e-aedd-011da6f5ea9a"));

            migrationBuilder.DropColumn(
                name: "MaxHP",
                schema: "Battles",
                table: "Creatures");

            migrationBuilder.DropColumn(
                name: "Stun",
                schema: "Battles",
                table: "Creatures");

            migrationBuilder.AddColumn<int>(
                name: "Body",
                schema: "Effects",
                table: "FreezeEffects",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Cra",
                schema: "Effects",
                table: "FreezeEffects",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Dex",
                schema: "Effects",
                table: "FreezeEffects",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Emp",
                schema: "Effects",
                table: "FreezeEffects",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Int",
                schema: "Effects",
                table: "FreezeEffects",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Ref",
                schema: "Effects",
                table: "FreezeEffects",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Speed",
                schema: "Effects",
                table: "FreezeEffects",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Will",
                schema: "Effects",
                table: "FreezeEffects",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
