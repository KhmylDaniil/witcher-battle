using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sindie.ApiService.Storage.Postgresql.Migrations
{
    public partial class addBaseGameDomain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Interface_InterfaceId",
                schema: "System",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Interface",
                schema: "System",
                table: "Interface");

            migrationBuilder.EnsureSchema(
                name: "BaseGame");

            migrationBuilder.RenameTable(
                name: "UserGames",
                newName: "UserGames",
                newSchema: "BaseGame");

            migrationBuilder.RenameTable(
                name: "Games",
                newName: "Games",
                newSchema: "BaseGame");

            migrationBuilder.RenameTable(
                name: "Interface",
                schema: "System",
                newName: "Interfaces",
                newSchema: "System");

            migrationBuilder.AlterTable(
                name: "UserGames",
                schema: "BaseGame",
                comment: "Игры пользователя");

            migrationBuilder.AlterTable(
                name: "Games",
                schema: "BaseGame",
                comment: "Игры");

            migrationBuilder.AddColumn<Guid>(
                name: "AvatarId",
                schema: "System",
                table: "Users",
                type: "uuid",
                nullable: true,
                comment: "Айди аватара");

            migrationBuilder.AlterColumn<string>(
                name: "Login",
                schema: "System",
                table: "UserAccounts",
                type: "text",
                nullable: false,
                defaultValue: "",
                comment: "Логин",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                schema: "System",
                table: "TextFiles",
                type: "uuid",
                nullable: false,
                defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)",
                oldComment: "Айди");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                schema: "System",
                table: "ImgFiles",
                type: "uuid",
                nullable: false,
                defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)",
                oldComment: "Айди");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                schema: "BaseGame",
                table: "UserGames",
                type: "uuid",
                nullable: false,
                comment: "Айди пользователя",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "GameId",
                schema: "BaseGame",
                table: "UserGames",
                type: "uuid",
                nullable: false,
                comment: "Айди игры",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "GameRoleId",
                schema: "BaseGame",
                table: "UserGames",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "Айди роли в игре");

            migrationBuilder.AddColumn<Guid>(
                name: "InterfaceId",
                schema: "BaseGame",
                table: "UserGames",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "Айди интерфейса");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "BaseGame",
                table: "Games",
                type: "text",
                nullable: false,
                defaultValue: "",
                comment: "Название",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                schema: "BaseGame",
                table: "Games",
                type: "text",
                nullable: true,
                comment: "Описание игры",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AvatarId",
                schema: "BaseGame",
                table: "Games",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "Айди аватара игры");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "System",
                table: "Interfaces",
                type: "text",
                nullable: false,
                comment: "Название интерфейса",
                oldClrType: typeof(string),
                oldType: "text",
                oldComment: "Название файла");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                schema: "System",
                table: "Interfaces",
                type: "text",
                nullable: false,
                defaultValue: "",
                comment: "Тип интерфейса");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Interfaces",
                schema: "System",
                table: "Interfaces",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Articles",
                schema: "System",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Название"),
                    Text = table.Column<string>(type: "text", nullable: false, comment: "Текст"),
                    GameId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди игры"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Articles_Games_GameId",
                        column: x => x.GameId,
                        principalSchema: "BaseGame",
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Статьи");

            migrationBuilder.CreateTable(
                name: "GameImgFiles",
                schema: "BaseGame",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    GameId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди игры"),
                    TextFileId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди графического файла"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameImgFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameImgFiles_Games_GameId",
                        column: x => x.GameId,
                        principalSchema: "BaseGame",
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameImgFiles_ImgFiles_TextFileId",
                        column: x => x.TextFileId,
                        principalSchema: "System",
                        principalTable: "ImgFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Графические файлы игры");

            migrationBuilder.CreateTable(
                name: "GameRoles",
                schema: "System",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Название"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameRoles", x => x.Id);
                },
                comment: "Роли в игре");

            migrationBuilder.CreateTable(
                name: "GameTextFiles",
                schema: "BaseGame",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    GameId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди игры"),
                    TextFileId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди текстового файла"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameTextFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameTextFiles_Games_GameId",
                        column: x => x.GameId,
                        principalSchema: "BaseGame",
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameTextFiles_TextFiles_TextFileId",
                        column: x => x.TextFileId,
                        principalSchema: "System",
                        principalTable: "TextFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Текстовые файлы игры");

            migrationBuilder.UpdateData(
                schema: "System",
                table: "Interfaces",
                keyColumn: "Id",
                keyValue: new Guid("8094e0d0-3137-4791-9053-9667cbe107d7"),
                column: "Type",
                value: "DarkThemeType");

            migrationBuilder.UpdateData(
                schema: "System",
                table: "Interfaces",
                keyColumn: "Id",
                keyValue: new Guid("8094e0d0-3137-4791-9053-9667cbe107d8"),
                column: "Type",
                value: "LightThemeType");

            migrationBuilder.CreateIndex(
                name: "IX_Users_AvatarId",
                schema: "System",
                table: "Users",
                column: "AvatarId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Name",
                schema: "System",
                table: "Users",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Name",
                schema: "System",
                table: "Roles",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserGames_GameRoleId",
                schema: "BaseGame",
                table: "UserGames",
                column: "GameRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGames_InterfaceId",
                schema: "BaseGame",
                table: "UserGames",
                column: "InterfaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_AvatarId",
                schema: "BaseGame",
                table: "Games",
                column: "AvatarId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Interfaces_Name",
                schema: "System",
                table: "Interfaces",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Articles_GameId",
                schema: "System",
                table: "Articles",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_GameImgFiles_GameId",
                schema: "BaseGame",
                table: "GameImgFiles",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_GameImgFiles_TextFileId",
                schema: "BaseGame",
                table: "GameImgFiles",
                column: "TextFileId");

            migrationBuilder.CreateIndex(
                name: "IX_GameRoles_Name",
                schema: "System",
                table: "GameRoles",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GameTextFiles_GameId",
                schema: "BaseGame",
                table: "GameTextFiles",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_GameTextFiles_TextFileId",
                schema: "BaseGame",
                table: "GameTextFiles",
                column: "TextFileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_ImgFiles_AvatarId",
                schema: "BaseGame",
                table: "Games",
                column: "AvatarId",
                principalSchema: "System",
                principalTable: "ImgFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserGames_GameRoles_GameRoleId",
                schema: "BaseGame",
                table: "UserGames",
                column: "GameRoleId",
                principalSchema: "System",
                principalTable: "GameRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_UserGames_Interfaces_InterfaceId",
                schema: "BaseGame",
                table: "UserGames",
                column: "InterfaceId",
                principalSchema: "System",
                principalTable: "Interfaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Interfaces_InterfaceId",
                schema: "System",
                table: "Users",
                column: "InterfaceId",
                principalSchema: "System",
                principalTable: "Interfaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_UserImgFiles_AvatarId",
                schema: "System",
                table: "Users",
                column: "AvatarId",
                principalSchema: "System",
                principalTable: "UserImgFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_ImgFiles_AvatarId",
                schema: "BaseGame",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_UserGames_GameRoles_GameRoleId",
                schema: "BaseGame",
                table: "UserGames");

            migrationBuilder.DropForeignKey(
                name: "FK_UserGames_Interfaces_InterfaceId",
                schema: "BaseGame",
                table: "UserGames");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Interfaces_InterfaceId",
                schema: "System",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_UserImgFiles_AvatarId",
                schema: "System",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Articles",
                schema: "System");

            migrationBuilder.DropTable(
                name: "GameImgFiles",
                schema: "BaseGame");

            migrationBuilder.DropTable(
                name: "GameRoles",
                schema: "System");

            migrationBuilder.DropTable(
                name: "GameTextFiles",
                schema: "BaseGame");

            migrationBuilder.DropIndex(
                name: "IX_Users_AvatarId",
                schema: "System",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_Name",
                schema: "System",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Roles_Name",
                schema: "System",
                table: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_UserGames_GameRoleId",
                schema: "BaseGame",
                table: "UserGames");

            migrationBuilder.DropIndex(
                name: "IX_UserGames_InterfaceId",
                schema: "BaseGame",
                table: "UserGames");

            migrationBuilder.DropIndex(
                name: "IX_Games_AvatarId",
                schema: "BaseGame",
                table: "Games");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Interfaces",
                schema: "System",
                table: "Interfaces");

            migrationBuilder.DropIndex(
                name: "IX_Interfaces_Name",
                schema: "System",
                table: "Interfaces");

            migrationBuilder.DropColumn(
                name: "AvatarId",
                schema: "System",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "GameRoleId",
                schema: "BaseGame",
                table: "UserGames");

            migrationBuilder.DropColumn(
                name: "InterfaceId",
                schema: "BaseGame",
                table: "UserGames");

            migrationBuilder.DropColumn(
                name: "AvatarId",
                schema: "BaseGame",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "Type",
                schema: "System",
                table: "Interfaces");

            migrationBuilder.RenameTable(
                name: "UserGames",
                schema: "BaseGame",
                newName: "UserGames");

            migrationBuilder.RenameTable(
                name: "Games",
                schema: "BaseGame",
                newName: "Games");

            migrationBuilder.RenameTable(
                name: "Interfaces",
                schema: "System",
                newName: "Interface",
                newSchema: "System");

            migrationBuilder.AlterTable(
                name: "UserGames",
                oldComment: "Игры пользователя");

            migrationBuilder.AlterTable(
                name: "Games",
                oldComment: "Игры");

            migrationBuilder.AlterColumn<string>(
                name: "Login",
                schema: "System",
                table: "UserAccounts",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldComment: "Логин");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                schema: "System",
                table: "TextFiles",
                type: "uuid",
                nullable: false,
                defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)",
                comment: "Айди",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                schema: "System",
                table: "ImgFiles",
                type: "uuid",
                nullable: false,
                defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)",
                comment: "Айди",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "UserGames",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "Айди пользователя");

            migrationBuilder.AlterColumn<Guid>(
                name: "GameId",
                table: "UserGames",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "Айди игры");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Games",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldComment: "Название");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Games",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "Описание игры");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "System",
                table: "Interface",
                type: "text",
                nullable: false,
                comment: "Название файла",
                oldClrType: typeof(string),
                oldType: "text",
                oldComment: "Название интерфейса");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Interface",
                schema: "System",
                table: "Interface",
                column: "Id");

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
    }
}
