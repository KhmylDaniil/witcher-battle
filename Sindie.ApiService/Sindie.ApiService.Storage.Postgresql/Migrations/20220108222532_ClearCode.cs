using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sindie.ApiService.Storage.Postgresql.Migrations
{
    public partial class ClearCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Interface_InterfaceId",
                schema: "System",
                table: "Users");

            migrationBuilder.DropTable(
                name: "GameTemplates");

            migrationBuilder.DeleteData(
                schema: "System",
                table: "Interface",
                keyColumn: "Id",
                keyValue: new Guid("818e1a78-5162-44d8-b407-d3fc07c1fb86"));

            migrationBuilder.AddColumn<string>(
                name: "RoleCreatedUser",
                schema: "System",
                table: "UserTextFiles",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoleModifiedUser",
                schema: "System",
                table: "UserTextFiles",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoleCreatedUser",
                schema: "System",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoleModifiedUser",
                schema: "System",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoleCreatedUser",
                schema: "System",
                table: "UserRoles",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoleModifiedUser",
                schema: "System",
                table: "UserRoles",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoleCreatedUser",
                schema: "System",
                table: "UserImgFiles",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoleModifiedUser",
                schema: "System",
                table: "UserImgFiles",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoleCreatedUser",
                table: "UserGames",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoleModifiedUser",
                table: "UserGames",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoleCreatedUser",
                table: "UserCharacters",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoleModifiedUser",
                table: "UserCharacters",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoleCreatedUser",
                schema: "System",
                table: "UserAccounts",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoleModifiedUser",
                schema: "System",
                table: "UserAccounts",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoleCreatedUser",
                schema: "System",
                table: "TextFiles",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoleModifiedUser",
                schema: "System",
                table: "TextFiles",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoleCreatedUser",
                table: "Slots",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoleModifiedUser",
                table: "Slots",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoleCreatedUser",
                schema: "System",
                table: "Roles",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoleModifiedUser",
                schema: "System",
                table: "Roles",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoleCreatedUser",
                table: "Parameters",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoleModifiedUser",
                table: "Parameters",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoleCreatedUser",
                table: "ParameterItems",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoleModifiedUser",
                table: "ParameterItems",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoleCreatedUser",
                table: "Modifiers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoleModifiedUser",
                table: "Modifiers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoleCreatedUser",
                table: "ModifierParametrs",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoleModifiedUser",
                table: "ModifierParametrs",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoleCreatedUser",
                table: "Items",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoleModifiedUser",
                table: "Items",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "System",
                table: "Interface",
                type: "text",
                nullable: false,
                comment: "Название файла",
                oldClrType: typeof(string),
                oldType: "text",
                oldComment: "название файла");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                schema: "System",
                table: "Interface",
                type: "uuid",
                nullable: false,
                defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)",
                oldComment: "Айди");

            migrationBuilder.AddColumn<string>(
                name: "RoleCreatedUser",
                schema: "System",
                table: "Interface",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoleModifiedUser",
                schema: "System",
                table: "Interface",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoleCreatedUser",
                schema: "System",
                table: "ImgFiles",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoleModifiedUser",
                schema: "System",
                table: "ImgFiles",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoleCreatedUser",
                table: "Games",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoleModifiedUser",
                table: "Games",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoleCreatedUser",
                table: "Characters",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoleModifiedUser",
                table: "Characters",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoleCreatedUser",
                table: "CharacterParameters",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoleModifiedUser",
                table: "CharacterParameters",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoleCreatedUser",
                table: "CharacteristicModifier",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoleModifiedUser",
                table: "CharacteristicModifier",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoleCreatedUser",
                table: "Characteristic",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoleModifiedUser",
                table: "Characteristic",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoleCreatedUser",
                table: "CharacterCharacteristic",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoleModifiedUser",
                table: "CharacterCharacteristic",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoleCreatedUser",
                table: "BodyItem",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoleModifiedUser",
                table: "BodyItem",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoleCreatedUser",
                table: "Body",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoleModifiedUser",
                table: "Body",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoleCreatedUser",
                table: "Bags",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoleModifiedUser",
                table: "Bags",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoleCreatedUser",
                table: "BagItems",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoleModifiedUser",
                table: "BagItems",
                type: "text",
                nullable: true);

            migrationBuilder.InsertData(
                schema: "System",
                table: "Interface",
                columns: new[] { "Id", "CreatedByUserId", "CreatedOn", "ModifiedByUserId", "ModifiedOn", "Name", "RoleCreatedUser", "RoleModifiedUser" },
                values: new object[,]
                {
                    { new Guid("8094e0d0-3137-4791-9053-9667cbe107d7"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "DarkTheme", "Default", "Default" },
                    { new Guid("8094e0d0-3137-4791-9053-9667cbe107d8"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "LightTheme", "Default", "Default" }
                });

            migrationBuilder.InsertData(
                schema: "System",
                table: "Roles",
                columns: new[] { "Id", "CreatedByUserId", "CreatedOn", "ModifiedByUserId", "ModifiedOn", "Name", "RoleCreatedUser", "RoleModifiedUser" },
                values: new object[,]
                {
                    { new Guid("8094e0d0-3147-4791-9053-9667cbe107d7"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "AndminRole", "Default", "Default" },
                    { new Guid("8094e0d0-3148-4791-9053-9667cbe107d8"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "UserRole", "Default", "Default" }
                });

            migrationBuilder.UpdateData(
                schema: "System",
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"),
                columns: new[] { "InterfaceId", "RoleCreatedUser", "RoleModifiedUser" },
                values: new object[] { new Guid("8094e0d0-3137-4791-9053-9667cbe107d7"), "Default", "Default" });

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Interface_InterfaceId",
                schema: "System",
                table: "Users",
                column: "InterfaceId",
                principalSchema: "System",
                principalTable: "Interface",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Interface_InterfaceId",
                schema: "System",
                table: "Users");

            migrationBuilder.DeleteData(
                schema: "System",
                table: "Interface",
                keyColumn: "Id",
                keyValue: new Guid("8094e0d0-3137-4791-9053-9667cbe107d7"));

            migrationBuilder.DeleteData(
                schema: "System",
                table: "Interface",
                keyColumn: "Id",
                keyValue: new Guid("8094e0d0-3137-4791-9053-9667cbe107d8"));

            migrationBuilder.DeleteData(
                schema: "System",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("8094e0d0-3147-4791-9053-9667cbe107d7"));

            migrationBuilder.DeleteData(
                schema: "System",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("8094e0d0-3148-4791-9053-9667cbe107d8"));

            migrationBuilder.DropColumn(
                name: "RoleCreatedUser",
                schema: "System",
                table: "UserTextFiles");

            migrationBuilder.DropColumn(
                name: "RoleModifiedUser",
                schema: "System",
                table: "UserTextFiles");

            migrationBuilder.DropColumn(
                name: "RoleCreatedUser",
                schema: "System",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RoleModifiedUser",
                schema: "System",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RoleCreatedUser",
                schema: "System",
                table: "UserRoles");

            migrationBuilder.DropColumn(
                name: "RoleModifiedUser",
                schema: "System",
                table: "UserRoles");

            migrationBuilder.DropColumn(
                name: "RoleCreatedUser",
                schema: "System",
                table: "UserImgFiles");

            migrationBuilder.DropColumn(
                name: "RoleModifiedUser",
                schema: "System",
                table: "UserImgFiles");

            migrationBuilder.DropColumn(
                name: "RoleCreatedUser",
                table: "UserGames");

            migrationBuilder.DropColumn(
                name: "RoleModifiedUser",
                table: "UserGames");

            migrationBuilder.DropColumn(
                name: "RoleCreatedUser",
                table: "UserCharacters");

            migrationBuilder.DropColumn(
                name: "RoleModifiedUser",
                table: "UserCharacters");

            migrationBuilder.DropColumn(
                name: "RoleCreatedUser",
                schema: "System",
                table: "UserAccounts");

            migrationBuilder.DropColumn(
                name: "RoleModifiedUser",
                schema: "System",
                table: "UserAccounts");

            migrationBuilder.DropColumn(
                name: "RoleCreatedUser",
                schema: "System",
                table: "TextFiles");

            migrationBuilder.DropColumn(
                name: "RoleModifiedUser",
                schema: "System",
                table: "TextFiles");

            migrationBuilder.DropColumn(
                name: "RoleCreatedUser",
                table: "Slots");

            migrationBuilder.DropColumn(
                name: "RoleModifiedUser",
                table: "Slots");

            migrationBuilder.DropColumn(
                name: "RoleCreatedUser",
                schema: "System",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "RoleModifiedUser",
                schema: "System",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "RoleCreatedUser",
                table: "Parameters");

            migrationBuilder.DropColumn(
                name: "RoleModifiedUser",
                table: "Parameters");

            migrationBuilder.DropColumn(
                name: "RoleCreatedUser",
                table: "ParameterItems");

            migrationBuilder.DropColumn(
                name: "RoleModifiedUser",
                table: "ParameterItems");

            migrationBuilder.DropColumn(
                name: "RoleCreatedUser",
                table: "Modifiers");

            migrationBuilder.DropColumn(
                name: "RoleModifiedUser",
                table: "Modifiers");

            migrationBuilder.DropColumn(
                name: "RoleCreatedUser",
                table: "ModifierParametrs");

            migrationBuilder.DropColumn(
                name: "RoleModifiedUser",
                table: "ModifierParametrs");

            migrationBuilder.DropColumn(
                name: "RoleCreatedUser",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "RoleModifiedUser",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "RoleCreatedUser",
                schema: "System",
                table: "Interface");

            migrationBuilder.DropColumn(
                name: "RoleModifiedUser",
                schema: "System",
                table: "Interface");

            migrationBuilder.DropColumn(
                name: "RoleCreatedUser",
                schema: "System",
                table: "ImgFiles");

            migrationBuilder.DropColumn(
                name: "RoleModifiedUser",
                schema: "System",
                table: "ImgFiles");

            migrationBuilder.DropColumn(
                name: "RoleCreatedUser",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "RoleModifiedUser",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "RoleCreatedUser",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "RoleModifiedUser",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "RoleCreatedUser",
                table: "CharacterParameters");

            migrationBuilder.DropColumn(
                name: "RoleModifiedUser",
                table: "CharacterParameters");

            migrationBuilder.DropColumn(
                name: "RoleCreatedUser",
                table: "CharacteristicModifier");

            migrationBuilder.DropColumn(
                name: "RoleModifiedUser",
                table: "CharacteristicModifier");

            migrationBuilder.DropColumn(
                name: "RoleCreatedUser",
                table: "Characteristic");

            migrationBuilder.DropColumn(
                name: "RoleModifiedUser",
                table: "Characteristic");

            migrationBuilder.DropColumn(
                name: "RoleCreatedUser",
                table: "CharacterCharacteristic");

            migrationBuilder.DropColumn(
                name: "RoleModifiedUser",
                table: "CharacterCharacteristic");

            migrationBuilder.DropColumn(
                name: "RoleCreatedUser",
                table: "BodyItem");

            migrationBuilder.DropColumn(
                name: "RoleModifiedUser",
                table: "BodyItem");

            migrationBuilder.DropColumn(
                name: "RoleCreatedUser",
                table: "Body");

            migrationBuilder.DropColumn(
                name: "RoleModifiedUser",
                table: "Body");

            migrationBuilder.DropColumn(
                name: "RoleCreatedUser",
                table: "Bags");

            migrationBuilder.DropColumn(
                name: "RoleModifiedUser",
                table: "Bags");

            migrationBuilder.DropColumn(
                name: "RoleCreatedUser",
                table: "BagItems");

            migrationBuilder.DropColumn(
                name: "RoleModifiedUser",
                table: "BagItems");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "System",
                table: "Interface",
                type: "text",
                nullable: false,
                comment: "название файла",
                oldClrType: typeof(string),
                oldType: "text",
                oldComment: "Название файла");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                schema: "System",
                table: "Interface",
                type: "uuid",
                nullable: false,
                defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)",
                comment: "Айди",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)");

            migrationBuilder.CreateTable(
                name: "GameTemplates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    Description = table.Column<string>(type: "text", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameTemplates", x => x.Id);
                });

            migrationBuilder.InsertData(
                schema: "System",
                table: "Interface",
                columns: new[] { "Id", "CreatedByUserId", "CreatedOn", "ModifiedByUserId", "ModifiedOn", "Name" },
                values: new object[] { new Guid("818e1a78-5162-44d8-b407-d3fc07c1fb86"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Системный интерфейс" });

            migrationBuilder.UpdateData(
                schema: "System",
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"),
                column: "InterfaceId",
                value: new Guid("818e1a78-5162-44d8-b407-d3fc07c1fb86"));

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Interface_InterfaceId",
                schema: "System",
                table: "Users",
                column: "InterfaceId",
                principalSchema: "System",
                principalTable: "Interface",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
