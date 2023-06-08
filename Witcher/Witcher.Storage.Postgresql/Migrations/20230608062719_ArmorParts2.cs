using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Witcher.Storage.Postgresql.Migrations
{
    public partial class ArmorParts2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BodyTemplateParts_BodyParts_Id",
                schema: "GameRules",
                table: "BodyTemplateParts");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                schema: "GameRules",
                table: "BodyTemplateParts",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)");

            migrationBuilder.AddColumn<int>(
                name: "BodyPartType",
                schema: "GameRules",
                table: "BodyTemplateParts",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedByUserId",
                schema: "GameRules",
                table: "BodyTemplateParts",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                schema: "GameRules",
                table: "BodyTemplateParts",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now() at time zone 'utc'");

            migrationBuilder.AddColumn<double>(
                name: "DamageModifier",
                schema: "GameRules",
                table: "BodyTemplateParts",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "HitPenalty",
                schema: "GameRules",
                table: "BodyTemplateParts",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaxToHit",
                schema: "GameRules",
                table: "BodyTemplateParts",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MinToHit",
                schema: "GameRules",
                table: "BodyTemplateParts",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedByUserId",
                schema: "GameRules",
                table: "BodyTemplateParts",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                schema: "GameRules",
                table: "BodyTemplateParts",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "GameRules",
                table: "BodyTemplateParts",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoleCreatedUser",
                schema: "GameRules",
                table: "BodyTemplateParts",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoleModifiedUser",
                schema: "GameRules",
                table: "BodyTemplateParts",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreaturePartId",
                schema: "Items",
                table: "ArmorParts",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "BodyTemplatePartId",
                schema: "Items",
                table: "ArmorParts",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "Items",
                table: "ArmorParts",
                type: "text",
                nullable: false,
                defaultValue: "",
                comment: "Название");

            migrationBuilder.CreateIndex(
                name: "IX_ArmorParts_BodyTemplatePartId",
                schema: "Items",
                table: "ArmorParts",
                column: "BodyTemplatePartId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArmorParts_BodyTemplateParts_BodyTemplatePartId",
                schema: "Items",
                table: "ArmorParts",
                column: "BodyTemplatePartId",
                principalSchema: "GameRules",
                principalTable: "BodyTemplateParts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArmorParts_BodyTemplateParts_BodyTemplatePartId",
                schema: "Items",
                table: "ArmorParts");

            migrationBuilder.DropIndex(
                name: "IX_ArmorParts_BodyTemplatePartId",
                schema: "Items",
                table: "ArmorParts");

            migrationBuilder.DropColumn(
                name: "BodyPartType",
                schema: "GameRules",
                table: "BodyTemplateParts");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                schema: "GameRules",
                table: "BodyTemplateParts");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                schema: "GameRules",
                table: "BodyTemplateParts");

            migrationBuilder.DropColumn(
                name: "DamageModifier",
                schema: "GameRules",
                table: "BodyTemplateParts");

            migrationBuilder.DropColumn(
                name: "HitPenalty",
                schema: "GameRules",
                table: "BodyTemplateParts");

            migrationBuilder.DropColumn(
                name: "MaxToHit",
                schema: "GameRules",
                table: "BodyTemplateParts");

            migrationBuilder.DropColumn(
                name: "MinToHit",
                schema: "GameRules",
                table: "BodyTemplateParts");

            migrationBuilder.DropColumn(
                name: "ModifiedByUserId",
                schema: "GameRules",
                table: "BodyTemplateParts");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                schema: "GameRules",
                table: "BodyTemplateParts");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "GameRules",
                table: "BodyTemplateParts");

            migrationBuilder.DropColumn(
                name: "RoleCreatedUser",
                schema: "GameRules",
                table: "BodyTemplateParts");

            migrationBuilder.DropColumn(
                name: "RoleModifiedUser",
                schema: "GameRules",
                table: "BodyTemplateParts");

            migrationBuilder.DropColumn(
                name: "BodyTemplatePartId",
                schema: "Items",
                table: "ArmorParts");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "Items",
                table: "ArmorParts");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                schema: "GameRules",
                table: "BodyTemplateParts",
                type: "uuid",
                nullable: false,
                defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreaturePartId",
                schema: "Items",
                table: "ArmorParts",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BodyTemplateParts_BodyParts_Id",
                schema: "GameRules",
                table: "BodyTemplateParts",
                column: "Id",
                principalSchema: "GameRules",
                principalTable: "BodyParts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
