using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Witcher.Storage.Postgresql.Migrations
{
    public partial class fixAppliedConditions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AppliedConditions",
                schema: "GameRules",
                table: "AppliedConditions");

            migrationBuilder.DropIndex(
                name: "IX_AppliedConditions_AbilityId",
                schema: "GameRules",
                table: "AppliedConditions");

            migrationBuilder.EnsureSchema(
                name: "Items");

            migrationBuilder.AlterTable(
                name: "AppliedConditions",
                schema: "GameRules",
                oldComment: "Применяемые состояния");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                schema: "GameRules",
                table: "AppliedConditions",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "now() at time zone 'utc'");

            migrationBuilder.AlterColumn<Guid>(
                name: "AbilityId",
                schema: "GameRules",
                table: "AppliedConditions",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "Айди способности");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                schema: "GameRules",
                table: "AppliedConditions",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppliedConditions",
                schema: "GameRules",
                table: "AppliedConditions",
                columns: new[] { "AbilityId", "Id" });

            migrationBuilder.CreateTable(
                name: "Items",
                schema: "Items",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    GameId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди игры"),
                    CharacterId = table.Column<Guid>(type: "uuid", nullable: true, comment: "Айди персонажа"),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Название"),
                    Description = table.Column<string>(type: "text", nullable: true, comment: "Описание"),
                    Weight = table.Column<int>(type: "integer", nullable: false, comment: "Вес"),
                    Price = table.Column<int>(type: "integer", nullable: false, comment: "Цена"),
                    IsStackable = table.Column<bool>(type: "boolean", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false, comment: "Количество"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Items_Characters_GameId",
                        column: x => x.GameId,
                        principalSchema: "Characters",
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Items_Games_GameId",
                        column: x => x.GameId,
                        principalSchema: "BaseGame",
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Предметы");

            migrationBuilder.CreateIndex(
                name: "IX_Items_GameId",
                schema: "Items",
                table: "Items",
                column: "GameId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Items",
                schema: "Items");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppliedConditions",
                schema: "GameRules",
                table: "AppliedConditions");

            migrationBuilder.AlterTable(
                name: "AppliedConditions",
                schema: "GameRules",
                comment: "Применяемые состояния");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                schema: "GameRules",
                table: "AppliedConditions",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now() at time zone 'utc'",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                schema: "GameRules",
                table: "AppliedConditions",
                type: "uuid",
                nullable: false,
                defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "AbilityId",
                schema: "GameRules",
                table: "AppliedConditions",
                type: "uuid",
                nullable: false,
                comment: "Айди способности",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppliedConditions",
                schema: "GameRules",
                table: "AppliedConditions",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_AppliedConditions_AbilityId",
                schema: "GameRules",
                table: "AppliedConditions",
                column: "AbilityId");
        }
    }
}
