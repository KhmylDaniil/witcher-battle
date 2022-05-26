using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sindie.ApiService.Storage.Postgresql.Migrations
{
    public partial class addSystemDomain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAccounts_Users_UserId",
                table: "UserAccounts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserAccounts",
                table: "UserAccounts");

            migrationBuilder.DropIndex(
                name: "IX_UserAccounts_UserId",
                table: "UserAccounts");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserAccounts");

            migrationBuilder.EnsureSchema(
                name: "System");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "Users",
                newSchema: "System");

            migrationBuilder.RenameTable(
                name: "UserRoles",
                newName: "UserRoles",
                newSchema: "System");

            migrationBuilder.RenameTable(
                name: "UserAccounts",
                newName: "UserAccounts",
                newSchema: "System");

            migrationBuilder.RenameTable(
                name: "Roles",
                newName: "Roles",
                newSchema: "System");

            migrationBuilder.AlterTable(
                name: "Users",
                schema: "System",
                comment: "Пользователи");

            migrationBuilder.AlterTable(
                name: "UserRoles",
                schema: "System",
                comment: "Роли пользователей");

            migrationBuilder.AlterTable(
                name: "UserAccounts",
                schema: "System",
                comment: "Аккаунты пользователей");

            migrationBuilder.AlterTable(
                name: "Roles",
                schema: "System",
                comment: "Роли");

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                schema: "System",
                table: "Users",
                type: "text",
                nullable: true,
                comment: "Телефон пользователя",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "System",
                table: "Users",
                type: "text",
                nullable: false,
                comment: "Имя пользователя",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                schema: "System",
                table: "Users",
                type: "text",
                nullable: true,
                comment: "Емэйл пользователя",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "InterfaceId",
                schema: "System",
                table: "Users",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "Интерфейс пользователя");

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                schema: "System",
                table: "UserAccounts",
                type: "text",
                nullable: false,
                comment: "Хэш пароля",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                schema: "System",
                table: "UserAccounts",
                type: "uuid",
                nullable: false,
                comment: "Айди",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)");

            migrationBuilder.AddColumn<Guid>(
                name: "Id1",
                schema: "System",
                table: "UserAccounts",
                type: "uuid",
                nullable: false,
                defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "System",
                table: "Roles",
                type: "text",
                nullable: false,
                comment: "Роль",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserAccounts",
                schema: "System",
                table: "UserAccounts",
                column: "Id1");

            migrationBuilder.CreateTable(
                name: "ImgFiles",
                schema: "System",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)", comment: "Айди"),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "название файла"),
                    Extension = table.Column<string>(type: "text", nullable: false, comment: "Расширение файла"),
                    Size = table.Column<int>(type: "integer", nullable: false, comment: "размер файла"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImgFiles", x => x.Id);
                },
                comment: "Графические файлы");

            migrationBuilder.CreateTable(
                name: "Interface",
                schema: "System",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)", comment: "Айди"),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "название файла"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Interface", x => x.Id);
                },
                comment: "Интерфейсы");

            migrationBuilder.CreateTable(
                name: "TextFiles",
                schema: "System",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)", comment: "Айди"),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "название файла"),
                    Extension = table.Column<string>(type: "text", nullable: false, comment: "Расширение файла"),
                    Size = table.Column<int>(type: "integer", nullable: false, comment: "размер файла"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TextFiles", x => x.Id);
                },
                comment: "Текстовые файлы");

            migrationBuilder.CreateTable(
                name: "UserImgFiles",
                schema: "System",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)", comment: "Айди"),
                    ImgFileId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди графического файла"),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди пользователя"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserImgFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserImgFiles_ImgFiles_ImgFileId",
                        column: x => x.ImgFileId,
                        principalSchema: "System",
                        principalTable: "ImgFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserImgFiles_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "System",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Графические файлы пользователей");

            migrationBuilder.CreateTable(
                name: "UserTextFiles",
                schema: "System",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)", comment: "Айди"),
                    TextFileId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди текстового файла"),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди пользователя"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTextFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserTextFiles_TextFiles_TextFileId",
                        column: x => x.TextFileId,
                        principalSchema: "System",
                        principalTable: "TextFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserTextFiles_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "System",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Текстовые файлы пользователей");

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

            migrationBuilder.CreateIndex(
                name: "IX_Users_InterfaceId",
                schema: "System",
                table: "Users",
                column: "InterfaceId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccounts_Id",
                schema: "System",
                table: "UserAccounts",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserImgFiles_ImgFileId",
                schema: "System",
                table: "UserImgFiles",
                column: "ImgFileId");

            migrationBuilder.CreateIndex(
                name: "IX_UserImgFiles_UserId",
                schema: "System",
                table: "UserImgFiles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTextFiles_TextFileId",
                schema: "System",
                table: "UserTextFiles",
                column: "TextFileId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTextFiles_UserId",
                schema: "System",
                table: "UserTextFiles",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAccounts_Users_Id",
                schema: "System",
                table: "UserAccounts",
                column: "Id",
                principalSchema: "System",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAccounts_Users_Id",
                schema: "System",
                table: "UserAccounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Interface_InterfaceId",
                schema: "System",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Interface",
                schema: "System");

            migrationBuilder.DropTable(
                name: "UserImgFiles",
                schema: "System");

            migrationBuilder.DropTable(
                name: "UserTextFiles",
                schema: "System");

            migrationBuilder.DropTable(
                name: "ImgFiles",
                schema: "System");

            migrationBuilder.DropTable(
                name: "TextFiles",
                schema: "System");

            migrationBuilder.DropIndex(
                name: "IX_Users_InterfaceId",
                schema: "System",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserAccounts",
                schema: "System",
                table: "UserAccounts");

            migrationBuilder.DropIndex(
                name: "IX_UserAccounts_Id",
                schema: "System",
                table: "UserAccounts");

            migrationBuilder.DropColumn(
                name: "InterfaceId",
                schema: "System",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Id1",
                schema: "System",
                table: "UserAccounts");

            migrationBuilder.RenameTable(
                name: "Users",
                schema: "System",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "UserRoles",
                schema: "System",
                newName: "UserRoles");

            migrationBuilder.RenameTable(
                name: "UserAccounts",
                schema: "System",
                newName: "UserAccounts");

            migrationBuilder.RenameTable(
                name: "Roles",
                schema: "System",
                newName: "Roles");

            migrationBuilder.AlterTable(
                name: "Users",
                oldComment: "Пользователи");

            migrationBuilder.AlterTable(
                name: "UserRoles",
                oldComment: "Роли пользователей");

            migrationBuilder.AlterTable(
                name: "UserAccounts",
                oldComment: "Аккаунты пользователей");

            migrationBuilder.AlterTable(
                name: "Roles",
                oldComment: "Роли");

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Users",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "Телефон пользователя");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Users",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldComment: "Имя пользователя");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "Емэйл пользователя");

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "UserAccounts",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldComment: "Хэш пароля");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "UserAccounts",
                type: "uuid",
                nullable: false,
                defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "Айди");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "UserAccounts",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Roles",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldComment: "Роль");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserAccounts",
                table: "UserAccounts",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccounts_UserId",
                table: "UserAccounts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAccounts_Users_UserId",
                table: "UserAccounts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
