using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Witcher.Storage.Postgresql.Migrations
{
    public partial class DBPurge : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "GameRules");

            migrationBuilder.EnsureSchema(
                name: "Battles");

            migrationBuilder.EnsureSchema(
                name: "Characters");

            migrationBuilder.EnsureSchema(
                name: "System");

            migrationBuilder.EnsureSchema(
                name: "BaseGame");

            migrationBuilder.CreateTable(
                name: "BodyPartTypes",
                schema: "GameRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Название"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BodyPartTypes", x => x.Id);
                },
                comment: "Типы частей тела");

            migrationBuilder.CreateTable(
                name: "Conditions",
                schema: "GameRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Название состояния"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conditions", x => x.Id);
                },
                comment: "Состояния");

            migrationBuilder.CreateTable(
                name: "CreatureTypes",
                schema: "GameRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Название"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreatureTypes", x => x.Id);
                },
                comment: "Типы существ");

            migrationBuilder.CreateTable(
                name: "DamageTypes",
                schema: "GameRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Название"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DamageTypes", x => x.Id);
                },
                comment: "Типы урона");

            migrationBuilder.CreateTable(
                name: "GameRoles",
                schema: "System",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Название"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
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
                name: "ImgFiles",
                schema: "System",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "название файла"),
                    Extension = table.Column<string>(type: "text", nullable: false, comment: "Расширение файла"),
                    Size = table.Column<int>(type: "integer", nullable: false, comment: "размер файла"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImgFiles", x => x.Id);
                },
                comment: "Графические файлы");

            migrationBuilder.CreateTable(
                name: "Interfaces",
                schema: "System",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Название интерфейса"),
                    Type = table.Column<string>(type: "text", nullable: false, comment: "Тип интерфейса"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Interfaces", x => x.Id);
                },
                comment: "Интерфейсы");

            migrationBuilder.CreateTable(
                name: "SystemRoles",
                schema: "System",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Роль в системе"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemRoles", x => x.Id);
                },
                comment: "Роли в системе");

            migrationBuilder.CreateTable(
                name: "TextFiles",
                schema: "System",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "название файла"),
                    Extension = table.Column<string>(type: "text", nullable: false, comment: "Расширение файла"),
                    Size = table.Column<int>(type: "integer", nullable: false, comment: "размер файла"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TextFiles", x => x.Id);
                },
                comment: "Текстовые файлы");

            migrationBuilder.CreateTable(
                name: "BodyParts",
                schema: "GameRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    BodyPartTypeId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди типа части тела"),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Название"),
                    HitPenalty = table.Column<int>(type: "integer", nullable: false, comment: "Пенальти за прицеливание"),
                    DamageModifer = table.Column<double>(type: "double precision", nullable: false, comment: "Модификатор урона"),
                    MinToHit = table.Column<int>(type: "integer", nullable: false, comment: "Минимальное значение попадания"),
                    MaxToHit = table.Column<int>(type: "integer", nullable: false, comment: "Максимальное значение попадания"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BodyParts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BodyParts_BodyPartTypes_BodyPartTypeId",
                        column: x => x.BodyPartTypeId,
                        principalSchema: "GameRules",
                        principalTable: "BodyPartTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Части тела");

            migrationBuilder.CreateTable(
                name: "Games",
                schema: "BaseGame",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Название"),
                    AvatarId = table.Column<Guid>(type: "uuid", nullable: true, comment: "Айди аватара игры"),
                    Description = table.Column<string>(type: "text", nullable: true, comment: "Описание игры"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Games_ImgFiles_AvatarId",
                        column: x => x.AvatarId,
                        principalSchema: "System",
                        principalTable: "ImgFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                },
                comment: "Игры");

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "System",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Имя пользователя"),
                    Email = table.Column<string>(type: "text", nullable: true, comment: "Емэйл пользователя"),
                    Phone = table.Column<string>(type: "text", nullable: true, comment: "Телефон пользователя"),
                    InterfaceId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Интерфейс пользователя"),
                    AvatarId = table.Column<Guid>(type: "uuid", nullable: true, comment: "Айди аватара"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_ImgFiles_AvatarId",
                        column: x => x.AvatarId,
                        principalSchema: "System",
                        principalTable: "ImgFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Users_Interfaces_InterfaceId",
                        column: x => x.InterfaceId,
                        principalSchema: "System",
                        principalTable: "Interfaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                },
                comment: "Пользователи");

            migrationBuilder.CreateTable(
                name: "BodyTemplates",
                schema: "GameRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Название шаблона тела"),
                    Description = table.Column<string>(type: "text", nullable: true, comment: "Описание"),
                    GameId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди игры"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BodyTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BodyTemplates_Games_GameId",
                        column: x => x.GameId,
                        principalSchema: "BaseGame",
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Шаблоны тел");

            migrationBuilder.CreateTable(
                name: "GameImgFile",
                columns: table => new
                {
                    GamesId = table.Column<Guid>(type: "uuid", nullable: false),
                    ImgFilesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameImgFile", x => new { x.GamesId, x.ImgFilesId });
                    table.ForeignKey(
                        name: "FK_GameImgFile_Games_GamesId",
                        column: x => x.GamesId,
                        principalSchema: "BaseGame",
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameImgFile_ImgFiles_ImgFilesId",
                        column: x => x.ImgFilesId,
                        principalSchema: "System",
                        principalTable: "ImgFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameTextFile",
                columns: table => new
                {
                    GamesId = table.Column<Guid>(type: "uuid", nullable: false),
                    TextFilesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameTextFile", x => new { x.GamesId, x.TextFilesId });
                    table.ForeignKey(
                        name: "FK_GameTextFile_Games_GamesId",
                        column: x => x.GamesId,
                        principalSchema: "BaseGame",
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameTextFile_TextFiles_TextFilesId",
                        column: x => x.TextFilesId,
                        principalSchema: "System",
                        principalTable: "TextFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Skills",
                schema: "GameRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    GameId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди игры"),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Название навыка"),
                    Description = table.Column<string>(type: "text", nullable: true, comment: "Описание навыка"),
                    StatName = table.Column<string>(type: "text", nullable: true, comment: "Название корреспондирующей характеристики"),
                    MinValueSkills = table.Column<int>(type: "integer", nullable: true, comment: "Минимальное значение навыка"),
                    MaxValueSkills = table.Column<int>(type: "integer", nullable: true, comment: "Максимальное значение навыка"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Skills_Games_GameId",
                        column: x => x.GameId,
                        principalSchema: "BaseGame",
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Навыки");

            migrationBuilder.CreateTable(
                name: "ImgFileUser",
                schema: "System",
                columns: table => new
                {
                    ImgFilesId = table.Column<Guid>(type: "uuid", nullable: false),
                    UsersId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImgFileUser", x => new { x.ImgFilesId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_ImgFileUser_ImgFiles_ImgFilesId",
                        column: x => x.ImgFilesId,
                        principalSchema: "System",
                        principalTable: "ImgFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ImgFileUser_Users_UsersId",
                        column: x => x.UsersId,
                        principalSchema: "System",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TextFileUser",
                schema: "System",
                columns: table => new
                {
                    TextFilesId = table.Column<Guid>(type: "uuid", nullable: false),
                    UsersId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TextFileUser", x => new { x.TextFilesId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_TextFileUser_TextFiles_TextFilesId",
                        column: x => x.TextFilesId,
                        principalSchema: "System",
                        principalTable: "TextFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TextFileUser_Users_UsersId",
                        column: x => x.UsersId,
                        principalSchema: "System",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserAccounts",
                schema: "System",
                columns: table => new
                {
                    Id1 = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    Id = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди"),
                    Login = table.Column<string>(type: "text", nullable: false, comment: "Логин"),
                    PasswordHash = table.Column<string>(type: "text", nullable: false, comment: "Хэш пароля"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccounts", x => x.Id1);
                    table.ForeignKey(
                        name: "FK_UserAccounts_Users_Id",
                        column: x => x.Id,
                        principalSchema: "System",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Аккаунты пользователей");

            migrationBuilder.CreateTable(
                name: "UserGames",
                schema: "BaseGame",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди пользователя"),
                    GameId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди игры"),
                    InterfaceId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди интерфейса"),
                    GameRoleId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди роли в игре"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGames", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserGames_GameRoles_GameRoleId",
                        column: x => x.GameRoleId,
                        principalSchema: "System",
                        principalTable: "GameRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_UserGames_Games_GameId",
                        column: x => x.GameId,
                        principalSchema: "BaseGame",
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserGames_Interfaces_InterfaceId",
                        column: x => x.InterfaceId,
                        principalSchema: "System",
                        principalTable: "Interfaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_UserGames_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "System",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Игры пользователя");

            migrationBuilder.CreateTable(
                name: "UserRoles",
                schema: "System",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    SystemRoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRoles_SystemRoles_SystemRoleId",
                        column: x => x.SystemRoleId,
                        principalSchema: "System",
                        principalTable: "SystemRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "System",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Роли пользователей");

            migrationBuilder.CreateTable(
                name: "BodyTemplateParts",
                schema: "GameRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    BodyTemplateId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди шаблона тела")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BodyTemplateParts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BodyTemplateParts_BodyParts_Id",
                        column: x => x.Id,
                        principalSchema: "GameRules",
                        principalTable: "BodyParts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BodyTemplateParts_BodyTemplates_BodyTemplateId",
                        column: x => x.BodyTemplateId,
                        principalSchema: "GameRules",
                        principalTable: "BodyTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Части шаблона тела");

            migrationBuilder.CreateTable(
                name: "CreatureTemplates",
                schema: "GameRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    GameId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди игры"),
                    ImgFileId = table.Column<Guid>(type: "uuid", nullable: true, comment: "Айди графического файла"),
                    BodyTemplateId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди шаблона тела"),
                    CreatureTypeId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди типа существа"),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Название шаблона"),
                    Description = table.Column<string>(type: "text", nullable: true, comment: "Описание шаблона"),
                    HP = table.Column<int>(type: "integer", nullable: false, comment: "Хиты"),
                    Sta = table.Column<int>(type: "integer", nullable: false, comment: "Стамина"),
                    Int = table.Column<int>(type: "integer", nullable: false, comment: "Интеллект"),
                    Ref = table.Column<int>(type: "integer", nullable: false, comment: "Рефлексы"),
                    Dex = table.Column<int>(type: "integer", nullable: false, comment: "Ловкость"),
                    Body = table.Column<int>(type: "integer", nullable: false, comment: "Телосложение"),
                    Emp = table.Column<int>(type: "integer", nullable: false, comment: "Эмпатия"),
                    Cra = table.Column<int>(type: "integer", nullable: false, comment: "Крафт"),
                    Will = table.Column<int>(type: "integer", nullable: false, comment: "Воля"),
                    Luck = table.Column<int>(type: "integer", nullable: false, comment: "Удача"),
                    Speed = table.Column<int>(type: "integer", nullable: false, comment: "Скорость"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreatureTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CreatureTemplates_BodyTemplates_BodyTemplateId",
                        column: x => x.BodyTemplateId,
                        principalSchema: "GameRules",
                        principalTable: "BodyTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CreatureTemplates_CreatureTypes_CreatureTypeId",
                        column: x => x.CreatureTypeId,
                        principalSchema: "GameRules",
                        principalTable: "CreatureTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CreatureTemplates_Games_GameId",
                        column: x => x.GameId,
                        principalSchema: "BaseGame",
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CreatureTemplates_ImgFiles_ImgFileId",
                        column: x => x.ImgFileId,
                        principalSchema: "System",
                        principalTable: "ImgFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                },
                comment: "Шаблоны существ");

            migrationBuilder.CreateTable(
                name: "Abilities",
                schema: "GameRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    GameId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди игры"),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Название способности"),
                    Description = table.Column<string>(type: "text", nullable: true, comment: "Описание способности"),
                    AttackSkillId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Навык атаки"),
                    AttackDiceQuantity = table.Column<int>(type: "integer", nullable: false, comment: "Количество кубов атаки"),
                    DamageModifier = table.Column<int>(type: "integer", nullable: false, comment: "Модификатор атаки"),
                    AttackSpeed = table.Column<int>(type: "integer", nullable: false, comment: "Скорость атаки"),
                    Accuracy = table.Column<int>(type: "integer", nullable: false, comment: "Точность атаки"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Abilities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Abilities_Games_GameId",
                        column: x => x.GameId,
                        principalSchema: "BaseGame",
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Abilities_Skills_AttackSkillId",
                        column: x => x.AttackSkillId,
                        principalSchema: "GameRules",
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Способности");

            migrationBuilder.CreateTable(
                name: "Battles",
                schema: "Battles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    GameId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди игры"),
                    ImgFileId = table.Column<Guid>(type: "uuid", nullable: true, comment: "Айди графического файла"),
                    UserGameActivatedId = table.Column<Guid>(type: "uuid", nullable: true, comment: "Айди активировавшего игру пользователя"),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Название боя"),
                    Description = table.Column<string>(type: "text", nullable: true, comment: "Описание боя"),
                    ActivationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Время активации боя"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Battles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Battles_Games_GameId",
                        column: x => x.GameId,
                        principalSchema: "BaseGame",
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Battles_ImgFiles_ImgFileId",
                        column: x => x.ImgFileId,
                        principalSchema: "System",
                        principalTable: "ImgFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Battles_UserGames_UserGameActivatedId",
                        column: x => x.UserGameActivatedId,
                        principalSchema: "BaseGame",
                        principalTable: "UserGames",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                },
                comment: "Экземпляры");

            migrationBuilder.CreateTable(
                name: "CreatureTemplateParameters",
                schema: "GameRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    CreatureId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди шаблона существа"),
                    SkillId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди навыка"),
                    StatName = table.Column<string>(type: "text", nullable: true, comment: "Название корреспондирующей характеристики"),
                    SkillValue = table.Column<int>(type: "integer", nullable: false, comment: "Значение навыка"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreatureTemplateParameters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CreatureTemplateParameters_CreatureTemplates_CreatureId",
                        column: x => x.CreatureId,
                        principalSchema: "GameRules",
                        principalTable: "CreatureTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CreatureTemplateParameters_Skills_SkillId",
                        column: x => x.SkillId,
                        principalSchema: "GameRules",
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Параметры шаблона существа");

            migrationBuilder.CreateTable(
                name: "CreatureTemplateParts",
                schema: "GameRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    CreatureTemplateId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди шаблона существа"),
                    Armor = table.Column<int>(type: "integer", nullable: false, comment: "Броня")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreatureTemplateParts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CreatureTemplateParts_BodyParts_Id",
                        column: x => x.Id,
                        principalSchema: "GameRules",
                        principalTable: "BodyParts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CreatureTemplateParts_CreatureTemplates_CreatureTemplateId",
                        column: x => x.CreatureTemplateId,
                        principalSchema: "GameRules",
                        principalTable: "CreatureTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Части шаблона существа");

            migrationBuilder.CreateTable(
                name: "CreatureTemplateResistances",
                schema: "GameRules",
                columns: table => new
                {
                    ResistancesId = table.Column<Guid>(type: "uuid", nullable: false),
                    ResistantCreatureTemplatesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreatureTemplateResistances", x => new { x.ResistancesId, x.ResistantCreatureTemplatesId });
                    table.ForeignKey(
                        name: "FK_CreatureTemplateResistances_CreatureTemplates_ResistantCrea~",
                        column: x => x.ResistantCreatureTemplatesId,
                        principalSchema: "GameRules",
                        principalTable: "CreatureTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CreatureTemplateResistances_DamageTypes_ResistancesId",
                        column: x => x.ResistancesId,
                        principalSchema: "GameRules",
                        principalTable: "DamageTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CreatureTemplateVulnerables",
                schema: "GameRules",
                columns: table => new
                {
                    VulnerableCreatureTemplatesId = table.Column<Guid>(type: "uuid", nullable: false),
                    VulnerablesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreatureTemplateVulnerables", x => new { x.VulnerableCreatureTemplatesId, x.VulnerablesId });
                    table.ForeignKey(
                        name: "FK_CreatureTemplateVulnerables_CreatureTemplates_VulnerableCre~",
                        column: x => x.VulnerableCreatureTemplatesId,
                        principalSchema: "GameRules",
                        principalTable: "CreatureTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CreatureTemplateVulnerables_DamageTypes_VulnerablesId",
                        column: x => x.VulnerablesId,
                        principalSchema: "GameRules",
                        principalTable: "DamageTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateTable(
                name: "AppliedConditions",
                schema: "GameRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    AbilityId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди способнности"),
                    ApplyChance = table.Column<int>(type: "integer", nullable: false, comment: "Шанс применения"),
                    ConditionId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди состояния"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppliedConditions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppliedConditions_Abilities_AbilityId",
                        column: x => x.AbilityId,
                        principalSchema: "GameRules",
                        principalTable: "Abilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppliedConditions_Conditions_ConditionId",
                        column: x => x.ConditionId,
                        principalSchema: "GameRules",
                        principalTable: "Conditions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Применяемые состояния");

            migrationBuilder.CreateTable(
                name: "CreatureTemplateAbilities",
                schema: "GameRules",
                columns: table => new
                {
                    AbilitiesId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatureTemplatesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreatureTemplateAbilities", x => new { x.AbilitiesId, x.CreatureTemplatesId });
                    table.ForeignKey(
                        name: "FK_CreatureTemplateAbilities_Abilities_AbilitiesId",
                        column: x => x.AbilitiesId,
                        principalSchema: "GameRules",
                        principalTable: "Abilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CreatureTemplateAbilities_CreatureTemplates_CreatureTemplat~",
                        column: x => x.CreatureTemplatesId,
                        principalSchema: "GameRules",
                        principalTable: "CreatureTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DefensiveSkills",
                schema: "GameRules",
                columns: table => new
                {
                    AbilitiesForDefenseId = table.Column<Guid>(type: "uuid", nullable: false),
                    DefensiveSkillsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DefensiveSkills", x => new { x.AbilitiesForDefenseId, x.DefensiveSkillsId });
                    table.ForeignKey(
                        name: "FK_DefensiveSkills_Abilities_AbilitiesForDefenseId",
                        column: x => x.AbilitiesForDefenseId,
                        principalSchema: "GameRules",
                        principalTable: "Abilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DefensiveSkills_Skills_DefensiveSkillsId",
                        column: x => x.DefensiveSkillsId,
                        principalSchema: "GameRules",
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Creatures",
                schema: "Battles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    BattleId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди боя"),
                    ImgFileId = table.Column<Guid>(type: "uuid", nullable: true, comment: "Айди графического файла"),
                    CreatureTemplateId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди шаблона существа"),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Название существа"),
                    Description = table.Column<string>(type: "text", nullable: true, comment: "Описание шаблона"),
                    CreatureTypeId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди типа существа"),
                    HP = table.Column<int>(type: "integer", nullable: false, comment: "Хиты"),
                    Sta = table.Column<int>(type: "integer", nullable: false, comment: "Стамина"),
                    Int = table.Column<int>(type: "integer", nullable: false, comment: "Интеллект"),
                    Ref = table.Column<int>(type: "integer", nullable: false, comment: "Рефлексы"),
                    Dex = table.Column<int>(type: "integer", nullable: false, comment: "Ловкость"),
                    Body = table.Column<int>(type: "integer", nullable: false, comment: "Телосложение"),
                    Emp = table.Column<int>(type: "integer", nullable: false, comment: "Эмпатия"),
                    Cra = table.Column<int>(type: "integer", nullable: false, comment: "Крафт"),
                    Will = table.Column<int>(type: "integer", nullable: false, comment: "Воля"),
                    Luck = table.Column<int>(type: "integer", nullable: false, comment: "Удача"),
                    Speed = table.Column<int>(type: "integer", nullable: false, comment: "Скорость"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Creatures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Creatures_Battles_BattleId",
                        column: x => x.BattleId,
                        principalSchema: "Battles",
                        principalTable: "Battles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Creatures_CreatureTemplates_CreatureTemplateId",
                        column: x => x.CreatureTemplateId,
                        principalSchema: "GameRules",
                        principalTable: "CreatureTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Creatures_CreatureTypes_CreatureTypeId",
                        column: x => x.CreatureTypeId,
                        principalSchema: "GameRules",
                        principalTable: "CreatureTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Creatures_ImgFiles_ImgFileId",
                        column: x => x.ImgFileId,
                        principalSchema: "System",
                        principalTable: "ImgFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                },
                comment: "Существа");

            migrationBuilder.CreateTable(
                name: "CreatureAbilities",
                schema: "Battles",
                columns: table => new
                {
                    AbilitiesId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreaturesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreatureAbilities", x => new { x.AbilitiesId, x.CreaturesId });
                    table.ForeignKey(
                        name: "FK_CreatureAbilities_Abilities_AbilitiesId",
                        column: x => x.AbilitiesId,
                        principalSchema: "GameRules",
                        principalTable: "Abilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CreatureAbilities_Creatures_CreaturesId",
                        column: x => x.CreaturesId,
                        principalSchema: "Battles",
                        principalTable: "Creatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CreatureParameters",
                schema: "Battles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    CreatureId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди существа"),
                    SkillId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди навыка"),
                    StatName = table.Column<string>(type: "text", nullable: true, comment: "Название корреспондирующей характеристики"),
                    SkillValue = table.Column<int>(type: "integer", nullable: false, comment: "Значение навыка"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreatureParameters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CreatureParameters_Creatures_CreatureId",
                        column: x => x.CreatureId,
                        principalSchema: "Battles",
                        principalTable: "Creatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CreatureParameters_Skills_SkillId",
                        column: x => x.SkillId,
                        principalSchema: "GameRules",
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Параметры существа");

            migrationBuilder.CreateTable(
                name: "CreatureParts",
                schema: "Battles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    CreatureId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди существа"),
                    StartingArmor = table.Column<int>(type: "integer", nullable: false, comment: "Стартовая броня"),
                    CurrentArmor = table.Column<int>(type: "integer", nullable: false, comment: "Текущая броня")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreatureParts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CreatureParts_BodyParts_Id",
                        column: x => x.Id,
                        principalSchema: "GameRules",
                        principalTable: "BodyParts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CreatureParts_Creatures_CreatureId",
                        column: x => x.CreatureId,
                        principalSchema: "Battles",
                        principalTable: "Creatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Части существа");

            migrationBuilder.CreateTable(
                name: "CreatureResistances",
                schema: "Battles",
                columns: table => new
                {
                    ResistancesId = table.Column<Guid>(type: "uuid", nullable: false),
                    ResistantCreaturesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreatureResistances", x => new { x.ResistancesId, x.ResistantCreaturesId });
                    table.ForeignKey(
                        name: "FK_CreatureResistances_Creatures_ResistantCreaturesId",
                        column: x => x.ResistantCreaturesId,
                        principalSchema: "Battles",
                        principalTable: "Creatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CreatureResistances_DamageTypes_ResistancesId",
                        column: x => x.ResistancesId,
                        principalSchema: "GameRules",
                        principalTable: "DamageTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CreatureVulnerables",
                schema: "Battles",
                columns: table => new
                {
                    VulnerableCreaturesId = table.Column<Guid>(type: "uuid", nullable: false),
                    VulnerablesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreatureVulnerables", x => new { x.VulnerableCreaturesId, x.VulnerablesId });
                    table.ForeignKey(
                        name: "FK_CreatureVulnerables_Creatures_VulnerableCreaturesId",
                        column: x => x.VulnerableCreaturesId,
                        principalSchema: "Battles",
                        principalTable: "Creatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CreatureVulnerables_DamageTypes_VulnerablesId",
                        column: x => x.VulnerablesId,
                        principalSchema: "GameRules",
                        principalTable: "DamageTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CurrentConditions",
                schema: "Battles",
                columns: table => new
                {
                    ConditionsId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreaturesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrentConditions", x => new { x.ConditionsId, x.CreaturesId });
                    table.ForeignKey(
                        name: "FK_CurrentConditions_Conditions_ConditionsId",
                        column: x => x.ConditionsId,
                        principalSchema: "GameRules",
                        principalTable: "Conditions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CurrentConditions_Creatures_CreaturesId",
                        column: x => x.CreaturesId,
                        principalSchema: "Battles",
                        principalTable: "Creatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Characters",
                schema: "Characters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    UserGameActivatedId = table.Column<Guid>(type: "uuid", nullable: true, comment: "Айди активировашего персонажа пользователя"),
                    ActivationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Время активации персонажа")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Characters_Creatures_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "Creatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Персонажи");

            migrationBuilder.CreateTable(
                name: "UserGameCharacters",
                schema: "Characters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    CharacterId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди персонажа"),
                    UserGameId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди пользователя игры"),
                    InterfaceId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди интерфейса"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGameCharacters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserGameCharacters_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalSchema: "Characters",
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserGameCharacters_Interfaces_InterfaceId",
                        column: x => x.InterfaceId,
                        principalSchema: "System",
                        principalTable: "Interfaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserGameCharacters_UserGames_UserGameId",
                        column: x => x.UserGameId,
                        principalSchema: "BaseGame",
                        principalTable: "UserGames",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Персонажи пользователя игры");

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
                table: "Conditions",
                columns: new[] { "Id", "CreatedByUserId", "CreatedOn", "ModifiedByUserId", "ModifiedOn", "Name", "RoleCreatedUser", "RoleModifiedUser" },
                values: new object[,]
                {
                    { new Guid("8894e0d0-3147-4791-9053-9667cbe127d7"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Poison", "Default", "Default" },
                    { new Guid("9994e0d0-3147-4791-9053-9667cbe127d7"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Bleed", "Default", "Default" }
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

            migrationBuilder.InsertData(
                schema: "GameRules",
                table: "DamageTypes",
                columns: new[] { "Id", "CreatedByUserId", "CreatedOn", "ModifiedByUserId", "ModifiedOn", "Name", "RoleCreatedUser", "RoleModifiedUser" },
                values: new object[,]
                {
                    { new Guid("42e5a598-f6e6-4ccd-8de3-d0e0963d1a33"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Slashing", "Default", "Default" },
                    { new Guid("43e5a598-f6e6-4ccd-8de3-d0e0963d1a33"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Piercing", "Default", "Default" },
                    { new Guid("44e5a598-f6e6-4ccd-8de3-d0e0963d1a33"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Bludgeoning", "Default", "Default" },
                    { new Guid("45e5a598-f6e6-4ccd-8de3-d0e0963d1a33"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Elemental", "Default", "Default" },
                    { new Guid("46e5a598-f6e6-4ccd-8de3-d0e0963d1a33"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Fire", "Default", "Default" },
                    { new Guid("47e5a598-f6e6-4ccd-8de3-d0e0963d1a33"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Silver", "Default", "Default" }
                });

            migrationBuilder.InsertData(
                schema: "System",
                table: "GameRoles",
                columns: new[] { "Id", "CreatedByUserId", "CreatedOn", "ModifiedByUserId", "ModifiedOn", "Name", "RoleCreatedUser", "RoleModifiedUser" },
                values: new object[,]
                {
                    { new Guid("8094e0d0-3147-4791-9053-9667cbe117d7"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Master", "Default", "Default" },
                    { new Guid("8094e0d0-3147-4791-9053-9667cbe127d7"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "MainMaster", "Default", "Default" },
                    { new Guid("8094e0d0-3148-4791-9053-9667cbe137d8"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Player", "Default", "Default" }
                });

            migrationBuilder.InsertData(
                schema: "System",
                table: "Interfaces",
                columns: new[] { "Id", "CreatedByUserId", "CreatedOn", "ModifiedByUserId", "ModifiedOn", "Name", "RoleCreatedUser", "RoleModifiedUser", "Type" },
                values: new object[,]
                {
                    { new Guid("8094e0d0-3137-4791-9053-9667cbe107d0"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "CharacterLightTheme", "Default", "Default", "CharacterInterface" },
                    { new Guid("8094e0d0-3137-4791-9053-9667cbe107d5"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "CharacterDarkTheme", "Default", "Default", "CharacterInterface" },
                    { new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "GameDarkTheme", "Default", "Default", "GameInterface" },
                    { new Guid("8094e0d0-3137-4791-9053-9667cbe107d7"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "SystemDarkTheme", "Default", "Default", "SystemInterface" },
                    { new Guid("8094e0d0-3137-4791-9053-9667cbe107d8"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), " SystemLightTheme", "Default", "Default", "SystemInterface" },
                    { new Guid("8094e0d0-3137-4791-9053-9667cbe107d9"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "GameLightTheme", "Default", "Default", "GameInterface" }
                });

            migrationBuilder.InsertData(
                schema: "System",
                table: "SystemRoles",
                columns: new[] { "Id", "CreatedByUserId", "CreatedOn", "ModifiedByUserId", "ModifiedOn", "Name", "RoleCreatedUser", "RoleModifiedUser" },
                values: new object[,]
                {
                    { new Guid("8094e0d0-3147-4791-9053-9667cbe107d7"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "AndminRole", "Default", "Default" },
                    { new Guid("8094e0d0-3148-4791-9053-9667cbe107d8"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "UserRole", "Default", "Default" }
                });

            migrationBuilder.InsertData(
                schema: "System",
                table: "Users",
                columns: new[] { "Id", "AvatarId", "CreatedByUserId", "CreatedOn", "Email", "InterfaceId", "ModifiedByUserId", "ModifiedOn", "Name", "Phone", "RoleCreatedUser", "RoleModifiedUser" },
                values: new object[] { new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), null, new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "andmin@email.ru", new Guid("8094e0d0-3137-4791-9053-9667cbe107d7"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Системный пользователь", "Нет", "Default", "Default" });

            migrationBuilder.CreateIndex(
                name: "IX_Abilities_AttackSkillId",
                schema: "GameRules",
                table: "Abilities",
                column: "AttackSkillId");

            migrationBuilder.CreateIndex(
                name: "IX_Abilities_GameId",
                schema: "GameRules",
                table: "Abilities",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_AbilityDamageTypes_DamageTypesId",
                schema: "GameRules",
                table: "AbilityDamageTypes",
                column: "DamageTypesId");

            migrationBuilder.CreateIndex(
                name: "IX_AppliedConditions_AbilityId",
                schema: "GameRules",
                table: "AppliedConditions",
                column: "AbilityId");

            migrationBuilder.CreateIndex(
                name: "IX_AppliedConditions_ConditionId",
                schema: "GameRules",
                table: "AppliedConditions",
                column: "ConditionId");

            migrationBuilder.CreateIndex(
                name: "IX_Battles_GameId",
                schema: "Battles",
                table: "Battles",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Battles_ImgFileId",
                schema: "Battles",
                table: "Battles",
                column: "ImgFileId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Battles_UserGameActivatedId",
                schema: "Battles",
                table: "Battles",
                column: "UserGameActivatedId");

            migrationBuilder.CreateIndex(
                name: "IX_BodyParts_BodyPartTypeId",
                schema: "GameRules",
                table: "BodyParts",
                column: "BodyPartTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_BodyTemplateParts_BodyTemplateId",
                schema: "GameRules",
                table: "BodyTemplateParts",
                column: "BodyTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_BodyTemplates_GameId",
                schema: "GameRules",
                table: "BodyTemplates",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_UserGameActivatedId",
                schema: "Characters",
                table: "Characters",
                column: "UserGameActivatedId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CreatureAbilities_CreaturesId",
                schema: "Battles",
                table: "CreatureAbilities",
                column: "CreaturesId");

            migrationBuilder.CreateIndex(
                name: "IX_CreatureParameters_CreatureId",
                schema: "Battles",
                table: "CreatureParameters",
                column: "CreatureId");

            migrationBuilder.CreateIndex(
                name: "IX_CreatureParameters_SkillId",
                schema: "Battles",
                table: "CreatureParameters",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_CreatureParts_CreatureId",
                schema: "Battles",
                table: "CreatureParts",
                column: "CreatureId");

            migrationBuilder.CreateIndex(
                name: "IX_CreatureResistances_ResistantCreaturesId",
                schema: "Battles",
                table: "CreatureResistances",
                column: "ResistantCreaturesId");

            migrationBuilder.CreateIndex(
                name: "IX_Creatures_BattleId",
                schema: "Battles",
                table: "Creatures",
                column: "BattleId");

            migrationBuilder.CreateIndex(
                name: "IX_Creatures_CreatureTemplateId",
                schema: "Battles",
                table: "Creatures",
                column: "CreatureTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Creatures_CreatureTypeId",
                schema: "Battles",
                table: "Creatures",
                column: "CreatureTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Creatures_ImgFileId",
                schema: "Battles",
                table: "Creatures",
                column: "ImgFileId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CreatureTemplateAbilities_CreatureTemplatesId",
                schema: "GameRules",
                table: "CreatureTemplateAbilities",
                column: "CreatureTemplatesId");

            migrationBuilder.CreateIndex(
                name: "IX_CreatureTemplateParameters_CreatureId",
                schema: "GameRules",
                table: "CreatureTemplateParameters",
                column: "CreatureId");

            migrationBuilder.CreateIndex(
                name: "IX_CreatureTemplateParameters_SkillId",
                schema: "GameRules",
                table: "CreatureTemplateParameters",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_CreatureTemplateParts_CreatureTemplateId",
                schema: "GameRules",
                table: "CreatureTemplateParts",
                column: "CreatureTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_CreatureTemplateResistances_ResistantCreatureTemplatesId",
                schema: "GameRules",
                table: "CreatureTemplateResistances",
                column: "ResistantCreatureTemplatesId");

            migrationBuilder.CreateIndex(
                name: "IX_CreatureTemplates_BodyTemplateId",
                schema: "GameRules",
                table: "CreatureTemplates",
                column: "BodyTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_CreatureTemplates_CreatureTypeId",
                schema: "GameRules",
                table: "CreatureTemplates",
                column: "CreatureTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CreatureTemplates_GameId",
                schema: "GameRules",
                table: "CreatureTemplates",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_CreatureTemplates_ImgFileId",
                schema: "GameRules",
                table: "CreatureTemplates",
                column: "ImgFileId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CreatureTemplateVulnerables_VulnerablesId",
                schema: "GameRules",
                table: "CreatureTemplateVulnerables",
                column: "VulnerablesId");

            migrationBuilder.CreateIndex(
                name: "IX_CreatureVulnerables_VulnerablesId",
                schema: "Battles",
                table: "CreatureVulnerables",
                column: "VulnerablesId");

            migrationBuilder.CreateIndex(
                name: "IX_CurrentConditions_CreaturesId",
                schema: "Battles",
                table: "CurrentConditions",
                column: "CreaturesId");

            migrationBuilder.CreateIndex(
                name: "IX_DefensiveSkills_DefensiveSkillsId",
                schema: "GameRules",
                table: "DefensiveSkills",
                column: "DefensiveSkillsId");

            migrationBuilder.CreateIndex(
                name: "IX_GameImgFile_ImgFilesId",
                table: "GameImgFile",
                column: "ImgFilesId");

            migrationBuilder.CreateIndex(
                name: "IX_GameRoles_Name",
                schema: "System",
                table: "GameRoles",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Games_AvatarId",
                schema: "BaseGame",
                table: "Games",
                column: "AvatarId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Games_Name",
                schema: "BaseGame",
                table: "Games",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GameTextFile_TextFilesId",
                table: "GameTextFile",
                column: "TextFilesId");

            migrationBuilder.CreateIndex(
                name: "IX_ImgFileUser_UsersId",
                schema: "System",
                table: "ImgFileUser",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_Interfaces_Name",
                schema: "System",
                table: "Interfaces",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Skills_GameId",
                schema: "GameRules",
                table: "Skills",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_SystemRoles_Name",
                schema: "System",
                table: "SystemRoles",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TextFileUser_UsersId",
                schema: "System",
                table: "TextFileUser",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccounts_Id",
                schema: "System",
                table: "UserAccounts",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccounts_Login",
                schema: "System",
                table: "UserAccounts",
                column: "Login",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserGameCharacters_CharacterId",
                schema: "Characters",
                table: "UserGameCharacters",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGameCharacters_InterfaceId",
                schema: "Characters",
                table: "UserGameCharacters",
                column: "InterfaceId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGameCharacters_UserGameId",
                schema: "Characters",
                table: "UserGameCharacters",
                column: "UserGameId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGames_GameId",
                schema: "BaseGame",
                table: "UserGames",
                column: "GameId");

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
                name: "IX_UserGames_UserId",
                schema: "BaseGame",
                table: "UserGames",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_SystemRoleId",
                schema: "System",
                table: "UserRoles",
                column: "SystemRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserId",
                schema: "System",
                table: "UserRoles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_AvatarId",
                schema: "System",
                table: "Users",
                column: "AvatarId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_InterfaceId",
                schema: "System",
                table: "Users",
                column: "InterfaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Name",
                schema: "System",
                table: "Users",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_UserGameCharacters_UserGameActivatedId",
                schema: "Characters",
                table: "Characters",
                column: "UserGameActivatedId",
                principalSchema: "Characters",
                principalTable: "UserGameCharacters",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Battles_Games_GameId",
                schema: "Battles",
                table: "Battles");

            migrationBuilder.DropForeignKey(
                name: "FK_BodyTemplates_Games_GameId",
                schema: "GameRules",
                table: "BodyTemplates");

            migrationBuilder.DropForeignKey(
                name: "FK_CreatureTemplates_Games_GameId",
                schema: "GameRules",
                table: "CreatureTemplates");

            migrationBuilder.DropForeignKey(
                name: "FK_UserGames_Games_GameId",
                schema: "BaseGame",
                table: "UserGames");

            migrationBuilder.DropForeignKey(
                name: "FK_Battles_ImgFiles_ImgFileId",
                schema: "Battles",
                table: "Battles");

            migrationBuilder.DropForeignKey(
                name: "FK_Creatures_ImgFiles_ImgFileId",
                schema: "Battles",
                table: "Creatures");

            migrationBuilder.DropForeignKey(
                name: "FK_CreatureTemplates_ImgFiles_ImgFileId",
                schema: "GameRules",
                table: "CreatureTemplates");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_ImgFiles_AvatarId",
                schema: "System",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Battles_UserGames_UserGameActivatedId",
                schema: "Battles",
                table: "Battles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserGameCharacters_UserGames_UserGameId",
                schema: "Characters",
                table: "UserGameCharacters");

            migrationBuilder.DropForeignKey(
                name: "FK_CreatureTemplates_BodyTemplates_BodyTemplateId",
                schema: "GameRules",
                table: "CreatureTemplates");

            migrationBuilder.DropForeignKey(
                name: "FK_Characters_Creatures_Id",
                schema: "Characters",
                table: "Characters");

            migrationBuilder.DropForeignKey(
                name: "FK_Characters_UserGameCharacters_UserGameActivatedId",
                schema: "Characters",
                table: "Characters");

            migrationBuilder.DropTable(
                name: "AbilityDamageTypes",
                schema: "GameRules");

            migrationBuilder.DropTable(
                name: "AppliedConditions",
                schema: "GameRules");

            migrationBuilder.DropTable(
                name: "BodyTemplateParts",
                schema: "GameRules");

            migrationBuilder.DropTable(
                name: "CreatureAbilities",
                schema: "Battles");

            migrationBuilder.DropTable(
                name: "CreatureParameters",
                schema: "Battles");

            migrationBuilder.DropTable(
                name: "CreatureParts",
                schema: "Battles");

            migrationBuilder.DropTable(
                name: "CreatureResistances",
                schema: "Battles");

            migrationBuilder.DropTable(
                name: "CreatureTemplateAbilities",
                schema: "GameRules");

            migrationBuilder.DropTable(
                name: "CreatureTemplateParameters",
                schema: "GameRules");

            migrationBuilder.DropTable(
                name: "CreatureTemplateParts",
                schema: "GameRules");

            migrationBuilder.DropTable(
                name: "CreatureTemplateResistances",
                schema: "GameRules");

            migrationBuilder.DropTable(
                name: "CreatureTemplateVulnerables",
                schema: "GameRules");

            migrationBuilder.DropTable(
                name: "CreatureVulnerables",
                schema: "Battles");

            migrationBuilder.DropTable(
                name: "CurrentConditions",
                schema: "Battles");

            migrationBuilder.DropTable(
                name: "DefensiveSkills",
                schema: "GameRules");

            migrationBuilder.DropTable(
                name: "GameImgFile");

            migrationBuilder.DropTable(
                name: "GameTextFile");

            migrationBuilder.DropTable(
                name: "ImgFileUser",
                schema: "System");

            migrationBuilder.DropTable(
                name: "TextFileUser",
                schema: "System");

            migrationBuilder.DropTable(
                name: "UserAccounts",
                schema: "System");

            migrationBuilder.DropTable(
                name: "UserRoles",
                schema: "System");

            migrationBuilder.DropTable(
                name: "BodyParts",
                schema: "GameRules");

            migrationBuilder.DropTable(
                name: "DamageTypes",
                schema: "GameRules");

            migrationBuilder.DropTable(
                name: "Conditions",
                schema: "GameRules");

            migrationBuilder.DropTable(
                name: "Abilities",
                schema: "GameRules");

            migrationBuilder.DropTable(
                name: "TextFiles",
                schema: "System");

            migrationBuilder.DropTable(
                name: "SystemRoles",
                schema: "System");

            migrationBuilder.DropTable(
                name: "BodyPartTypes",
                schema: "GameRules");

            migrationBuilder.DropTable(
                name: "Skills",
                schema: "GameRules");

            migrationBuilder.DropTable(
                name: "Games",
                schema: "BaseGame");

            migrationBuilder.DropTable(
                name: "ImgFiles",
                schema: "System");

            migrationBuilder.DropTable(
                name: "UserGames",
                schema: "BaseGame");

            migrationBuilder.DropTable(
                name: "GameRoles",
                schema: "System");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "System");

            migrationBuilder.DropTable(
                name: "BodyTemplates",
                schema: "GameRules");

            migrationBuilder.DropTable(
                name: "Creatures",
                schema: "Battles");

            migrationBuilder.DropTable(
                name: "Battles",
                schema: "Battles");

            migrationBuilder.DropTable(
                name: "CreatureTemplates",
                schema: "GameRules");

            migrationBuilder.DropTable(
                name: "CreatureTypes",
                schema: "GameRules");

            migrationBuilder.DropTable(
                name: "UserGameCharacters",
                schema: "Characters");

            migrationBuilder.DropTable(
                name: "Characters",
                schema: "Characters");

            migrationBuilder.DropTable(
                name: "Interfaces",
                schema: "System");
        }
    }
}
