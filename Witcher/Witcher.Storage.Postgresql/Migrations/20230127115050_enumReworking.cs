using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Witcher.Storage.Postgresql.Migrations
{
    public partial class enumReworking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BodyParts_BodyPartTypes_BodyPartTypeId",
                schema: "GameRules",
                table: "BodyParts");

            migrationBuilder.DropForeignKey(
                name: "FK_Creatures_CreatureTypes_CreatureTypeId",
                schema: "Battles",
                table: "Creatures");

            migrationBuilder.DropForeignKey(
                name: "FK_CreatureTemplates_CreatureTypes_CreatureTypeId",
                schema: "GameRules",
                table: "CreatureTemplates");

            migrationBuilder.DropTable(
                name: "BodyPartTypes",
                schema: "GameRules");

            migrationBuilder.DropTable(
                name: "CreatureTypes",
                schema: "GameRules");

            migrationBuilder.DropIndex(
                name: "IX_CreatureTemplates_CreatureTypeId",
                schema: "GameRules",
                table: "CreatureTemplates");

            migrationBuilder.DropIndex(
                name: "IX_Creatures_CreatureTypeId",
                schema: "Battles",
                table: "Creatures");

            migrationBuilder.DropIndex(
                name: "IX_BodyParts_BodyPartTypeId",
                schema: "GameRules",
                table: "BodyParts");

            migrationBuilder.DropColumn(
                name: "CreatureTypeId",
                schema: "GameRules",
                table: "CreatureTemplates");

            migrationBuilder.DropColumn(
                name: "CreatureTypeId",
                schema: "Battles",
                table: "Creatures");

            migrationBuilder.DropColumn(
                name: "BodyPartTypeId",
                schema: "GameRules",
                table: "BodyParts");

            migrationBuilder.AddColumn<int>(
                name: "CreatureType",
                schema: "GameRules",
                table: "CreatureTemplates",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "Тип существа");

            migrationBuilder.AddColumn<int>(
                name: "CreatureType",
                schema: "Battles",
                table: "Creatures",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "Тип существа");

            migrationBuilder.AddColumn<int>(
                name: "BodyPartType",
                schema: "GameRules",
                table: "BodyParts",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "Айди типа части тела");

            migrationBuilder.UpdateData(
                schema: "System",
                table: "SystemRoles",
                keyColumn: "Id",
                keyValue: new Guid("8094e0d0-3147-4791-9053-9667cbe107d7"),
                column: "Name",
                value: "AdminRole");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatureType",
                schema: "GameRules",
                table: "CreatureTemplates");

            migrationBuilder.DropColumn(
                name: "CreatureType",
                schema: "Battles",
                table: "Creatures");

            migrationBuilder.DropColumn(
                name: "BodyPartType",
                schema: "GameRules",
                table: "BodyParts");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatureTypeId",
                schema: "GameRules",
                table: "CreatureTemplates",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "Айди типа существа");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatureTypeId",
                schema: "Battles",
                table: "Creatures",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "Айди типа существа");

            migrationBuilder.AddColumn<Guid>(
                name: "BodyPartTypeId",
                schema: "GameRules",
                table: "BodyParts",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "Айди типа части тела");

            migrationBuilder.CreateTable(
                name: "BodyPartTypes",
                schema: "GameRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Название"),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BodyPartTypes", x => x.Id);
                },
                comment: "Типы частей тела");

            migrationBuilder.CreateTable(
                name: "CreatureTypes",
                schema: "GameRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Название"),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreatureTypes", x => x.Id);
                },
                comment: "Типы существ");

            migrationBuilder.InsertData(
                schema: "GameRules",
                table: "BodyPartTypes",
                columns: new[] { "Id", "CreatedByUserId", "CreatedOn", "ModifiedByUserId", "ModifiedOn", "Name", "RoleCreatedUser", "RoleModifiedUser" },
                values: new object[,]
                {
                    { new Guid("8894e0d0-3147-4791-1153-9667cbe127d7"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Tail", "Default", "Default" },
                    { new Guid("8894e0d0-3147-4791-1353-9667cbe127d7"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Void", "Default", "Default" },
                    { new Guid("8894e0d0-3147-4791-9153-9667cbe127d7"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Head", "Default", "Default" },
                    { new Guid("8894e0d0-3147-4791-9353-9667cbe127d7"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Torso", "Default", "Default" },
                    { new Guid("8894e0d0-3147-4791-9553-9667cbe127d7"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Arm", "Default", "Default" },
                    { new Guid("8894e0d0-3147-4791-9753-9667cbe127d7"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Leg", "Default", "Default" },
                    { new Guid("8894e0d0-3147-4791-9953-9667cbe127d7"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Wing", "Default", "Default" }
                });

            migrationBuilder.InsertData(
                schema: "GameRules",
                table: "CreatureTypes",
                columns: new[] { "Id", "CreatedByUserId", "CreatedOn", "ModifiedByUserId", "ModifiedOn", "Name", "RoleCreatedUser", "RoleModifiedUser" },
                values: new object[,]
                {
                    { new Guid("03ca5eb6-6534-4eea-9616-78e3ef0d572c"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Human", "Default", "Default" },
                    { new Guid("04ca5eb6-6534-4eea-9616-78e3ef0d572c"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Necrophage", "Default", "Default" },
                    { new Guid("05ca5eb6-6534-4eea-9616-78e3ef0d572c"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Specter", "Default", "Default" },
                    { new Guid("06ca5eb6-6534-4eea-9616-78e3ef0d572c"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Beast", "Default", "Default" },
                    { new Guid("07ca5eb6-6534-4eea-9616-78e3ef0d572c"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Cursed", "Default", "Default" },
                    { new Guid("08ca5eb6-6534-4eea-9616-78e3ef0d572c"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Hybrid", "Default", "Default" },
                    { new Guid("09ca5eb6-6534-4eea-9616-78e3ef0d572c"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Insectoid", "Default", "Default" },
                    { new Guid("13ca5eb6-6534-4eea-9616-78e3ef0d572c"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Elementa", "Default", "Default" },
                    { new Guid("23ca5eb6-6534-4eea-9616-78e3ef0d572c"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Relict", "Default", "Default" },
                    { new Guid("33ca5eb6-6534-4eea-9616-78e3ef0d572c"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ogroid", "Default", "Default" },
                    { new Guid("43ca5eb6-6534-4eea-9616-78e3ef0d572c"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Draconid", "Default", "Default" },
                    { new Guid("53ca5eb6-6534-4eea-9616-78e3ef0d572c"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Vampire", "Default", "Default" }
                });

            migrationBuilder.UpdateData(
                schema: "System",
                table: "SystemRoles",
                keyColumn: "Id",
                keyValue: new Guid("8094e0d0-3147-4791-9053-9667cbe107d7"),
                column: "Name",
                value: "AndminRole");

            migrationBuilder.CreateIndex(
                name: "IX_CreatureTemplates_CreatureTypeId",
                schema: "GameRules",
                table: "CreatureTemplates",
                column: "CreatureTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Creatures_CreatureTypeId",
                schema: "Battles",
                table: "Creatures",
                column: "CreatureTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_BodyParts_BodyPartTypeId",
                schema: "GameRules",
                table: "BodyParts",
                column: "BodyPartTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_BodyParts_BodyPartTypes_BodyPartTypeId",
                schema: "GameRules",
                table: "BodyParts",
                column: "BodyPartTypeId",
                principalSchema: "GameRules",
                principalTable: "BodyPartTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Creatures_CreatureTypes_CreatureTypeId",
                schema: "Battles",
                table: "Creatures",
                column: "CreatureTypeId",
                principalSchema: "GameRules",
                principalTable: "CreatureTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CreatureTemplates_CreatureTypes_CreatureTypeId",
                schema: "GameRules",
                table: "CreatureTemplates",
                column: "CreatureTypeId",
                principalSchema: "GameRules",
                principalTable: "CreatureTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
