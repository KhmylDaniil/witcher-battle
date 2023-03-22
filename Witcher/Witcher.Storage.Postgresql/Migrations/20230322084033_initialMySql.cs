using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Witcher.Storage.MySql.Migrations
{
    /// <inheritdoc />
    public partial class initialMySql : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "GameRules");

            migrationBuilder.EnsureSchema(
                name: "Battles");

            migrationBuilder.EnsureSchema(
                name: "Effects");

            migrationBuilder.EnsureSchema(
                name: "Characters");

            migrationBuilder.EnsureSchema(
                name: "System");

            migrationBuilder.EnsureSchema(
                name: "BaseGame");

            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "BodyParts",
                schema: "GameRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    BodyPartType = table.Column<string>(type: "longtext", nullable: false, comment: "Тип части тела")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "longtext", nullable: false, comment: "Название")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HitPenalty = table.Column<int>(type: "int", nullable: false, comment: "Пенальти за прицеливание"),
                    DamageModifer = table.Column<double>(type: "double", nullable: false, comment: "Модификатор урона"),
                    MinToHit = table.Column<int>(type: "int", nullable: false, comment: "Минимальное значение попадания"),
                    MaxToHit = table.Column<int>(type: "int", nullable: false, comment: "Максимальное значение попадания"),
                    CreatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RoleCreatedUser = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ModifiedByUserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RoleModifiedUser = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BodyParts", x => x.Id);
                },
                comment: "Части тела")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "GameRoles",
                schema: "System",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "varchar(255)", nullable: false, comment: "Название")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RoleCreatedUser = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ModifiedByUserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RoleModifiedUser = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameRoles", x => x.Id);
                },
                comment: "Роли в игре")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ImgFiles",
                schema: "System",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: false, comment: "название файла")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Extension = table.Column<string>(type: "longtext", nullable: false, comment: "Расширение файла")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Size = table.Column<int>(type: "int", nullable: false, comment: "размер файла"),
                    CreatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RoleCreatedUser = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ModifiedByUserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RoleModifiedUser = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImgFiles", x => x.Id);
                },
                comment: "Графические файлы")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Interfaces",
                schema: "System",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "varchar(255)", nullable: false, comment: "Название интерфейса")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Type = table.Column<string>(type: "longtext", nullable: false, comment: "Тип интерфейса")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RoleCreatedUser = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ModifiedByUserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RoleModifiedUser = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Interfaces", x => x.Id);
                },
                comment: "Интерфейсы")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SystemRoles",
                schema: "System",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "varchar(255)", nullable: false, comment: "Роль в системе")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RoleCreatedUser = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ModifiedByUserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RoleModifiedUser = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemRoles", x => x.Id);
                },
                comment: "Роли в системе")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TextFiles",
                schema: "System",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: false, comment: "название файла")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Extension = table.Column<string>(type: "longtext", nullable: false, comment: "Расширение файла")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Size = table.Column<int>(type: "int", nullable: false, comment: "размер файла"),
                    CreatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RoleCreatedUser = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ModifiedByUserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RoleModifiedUser = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TextFiles", x => x.Id);
                },
                comment: "Текстовые файлы")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Games",
                schema: "BaseGame",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "varchar(255)", nullable: false, comment: "Название")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AvatarId = table.Column<Guid>(type: "char(36)", nullable: true, comment: "Айди аватара игры", collation: "ascii_general_ci"),
                    Description = table.Column<string>(type: "longtext", nullable: true, comment: "Описание игры")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RoleCreatedUser = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ModifiedByUserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RoleModifiedUser = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
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
                comment: "Игры")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "System",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "varchar(255)", nullable: false, comment: "Имя пользователя")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: true, comment: "Емэйл пользователя")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Phone = table.Column<string>(type: "longtext", nullable: true, comment: "Телефон пользователя")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    InterfaceId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "Интерфейс пользователя", collation: "ascii_general_ci"),
                    AvatarId = table.Column<Guid>(type: "char(36)", nullable: true, comment: "Айди аватара", collation: "ascii_general_ci"),
                    CreatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RoleCreatedUser = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ModifiedByUserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RoleModifiedUser = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
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
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Пользователи")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Abilities",
                schema: "GameRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    GameId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "Айди игры", collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: false, comment: "Название способности")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true, comment: "Описание способности")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AttackSkill = table.Column<string>(type: "longtext", nullable: false, comment: "Навык атаки")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DamageType = table.Column<string>(type: "longtext", nullable: false, comment: "Тип урона")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AttackDiceQuantity = table.Column<int>(type: "int", nullable: false, comment: "Количество кубов атаки"),
                    DamageModifier = table.Column<int>(type: "int", nullable: false, comment: "Модификатор атаки"),
                    AttackSpeed = table.Column<int>(type: "int", nullable: false, comment: "Скорость атаки"),
                    Accuracy = table.Column<int>(type: "int", nullable: false, comment: "Точность атаки"),
                    CreatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RoleCreatedUser = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ModifiedByUserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RoleModifiedUser = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
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
                },
                comment: "Способности")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "BodyTemplates",
                schema: "GameRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: false, comment: "Название шаблона тела")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true, comment: "Описание")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    GameId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "Айди игры", collation: "ascii_general_ci"),
                    CreatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RoleCreatedUser = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ModifiedByUserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RoleModifiedUser = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
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
                comment: "Шаблоны тел")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "GameImgFile",
                columns: table => new
                {
                    GamesId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ImgFilesId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
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
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "GameTextFile",
                columns: table => new
                {
                    GamesId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    TextFilesId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
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
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ImgFileUser",
                schema: "System",
                columns: table => new
                {
                    ImgFilesId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UsersId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
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
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TextFileUser",
                schema: "System",
                columns: table => new
                {
                    TextFilesId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UsersId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
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
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserAccounts",
                schema: "System",
                columns: table => new
                {
                    Id1 = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, comment: "Айди", collation: "ascii_general_ci"),
                    Login = table.Column<string>(type: "varchar(255)", nullable: false, comment: "Логин")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PasswordHash = table.Column<string>(type: "longtext", nullable: false, comment: "Хэш пароля")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RoleCreatedUser = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ModifiedByUserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RoleModifiedUser = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
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
                comment: "Аккаунты пользователей")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserGames",
                schema: "BaseGame",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "Айди пользователя", collation: "ascii_general_ci"),
                    GameId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "Айди игры", collation: "ascii_general_ci"),
                    InterfaceId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "Айди интерфейса", collation: "ascii_general_ci"),
                    GameRoleId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "Айди роли в игре", collation: "ascii_general_ci"),
                    CreatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RoleCreatedUser = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ModifiedByUserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RoleModifiedUser = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
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
                        onDelete: ReferentialAction.Cascade);
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
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserGames_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "System",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Игры пользователя")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserRoles",
                schema: "System",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SystemRoleId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RoleCreatedUser = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ModifiedByUserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RoleModifiedUser = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
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
                comment: "Роли пользователей")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AppliedConditions",
                schema: "GameRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    AbilityId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "Айди способности", collation: "ascii_general_ci"),
                    ApplyChance = table.Column<int>(type: "int", nullable: false, comment: "Шанс применения"),
                    Condition = table.Column<string>(type: "longtext", nullable: false, comment: "Тип состояния")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RoleCreatedUser = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ModifiedByUserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RoleModifiedUser = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
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
                },
                comment: "Применяемые состояния")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DefensiveSkill",
                schema: "GameRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    AbilityId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Skill = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RoleCreatedUser = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ModifiedByUserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RoleModifiedUser = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DefensiveSkill", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DefensiveSkill_Abilities_AbilityId",
                        column: x => x.AbilityId,
                        principalSchema: "GameRules",
                        principalTable: "Abilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "BodyTemplateParts",
                schema: "GameRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    BodyTemplateId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "Айди шаблона тела", collation: "ascii_general_ci")
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
                comment: "Части шаблона тела")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CreatureTemplates",
                schema: "GameRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    GameId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "Айди игры", collation: "ascii_general_ci"),
                    ImgFileId = table.Column<Guid>(type: "char(36)", nullable: true, comment: "Айди графического файла", collation: "ascii_general_ci"),
                    BodyTemplateId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "Айди шаблона тела", collation: "ascii_general_ci"),
                    CreatureType = table.Column<string>(type: "longtext", nullable: false, comment: "Тип существа")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "longtext", nullable: false, comment: "Название шаблона")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true, comment: "Описание шаблона")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HP = table.Column<int>(type: "int", nullable: false, comment: "Хиты"),
                    Sta = table.Column<int>(type: "int", nullable: false, comment: "Стамина"),
                    Int = table.Column<int>(type: "int", nullable: false, comment: "Интеллект"),
                    Ref = table.Column<int>(type: "int", nullable: false, comment: "Рефлексы"),
                    Dex = table.Column<int>(type: "int", nullable: false, comment: "Ловкость"),
                    Body = table.Column<int>(type: "int", nullable: false, comment: "Телосложение"),
                    Emp = table.Column<int>(type: "int", nullable: false, comment: "Эмпатия"),
                    Cra = table.Column<int>(type: "int", nullable: false, comment: "Крафт"),
                    Will = table.Column<int>(type: "int", nullable: false, comment: "Воля"),
                    Luck = table.Column<int>(type: "int", nullable: false, comment: "Удача"),
                    Speed = table.Column<int>(type: "int", nullable: false, comment: "Скорость"),
                    CreatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RoleCreatedUser = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ModifiedByUserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RoleModifiedUser = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
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
                comment: "Шаблоны существ")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Battles",
                schema: "Battles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    GameId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "Айди игры", collation: "ascii_general_ci"),
                    ImgFileId = table.Column<Guid>(type: "char(36)", nullable: true, comment: "Айди графического файла", collation: "ascii_general_ci"),
                    UserGameActivatedId = table.Column<Guid>(type: "char(36)", nullable: true, comment: "Айди активировавшего игру пользователя", collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: false, comment: "Название боя")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true, comment: "Описание боя")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NextInitiative = table.Column<int>(type: "int", nullable: false, comment: "Значение инициативы следующего существа"),
                    BattleLog = table.Column<string>(type: "longtext", nullable: true, comment: "Журнал боя")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RoleCreatedUser = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ModifiedByUserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RoleModifiedUser = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
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
                comment: "Экземпляры")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CreatureTemplateAbilities",
                schema: "GameRules",
                columns: table => new
                {
                    AbilitiesId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatureTemplatesId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
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
                        name: "FK_CreatureTemplateAbilities_CreatureTemplates_CreatureTemplate~",
                        column: x => x.CreatureTemplatesId,
                        principalSchema: "GameRules",
                        principalTable: "CreatureTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CreatureTemplateDamageTypeModifier",
                schema: "GameRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DamageType = table.Column<int>(type: "int", nullable: false),
                    DamageTypeModifier = table.Column<int>(type: "int", nullable: false),
                    PrimaryEntityid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatureTemplateId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RoleCreatedUser = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ModifiedByUserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RoleModifiedUser = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreatureTemplateDamageTypeModifier", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CreatureTemplateDamageTypeModifier_CreatureTemplates_Creatur~",
                        column: x => x.CreatureTemplateId,
                        principalSchema: "GameRules",
                        principalTable: "CreatureTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CreatureTemplateParts",
                schema: "GameRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatureTemplateId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "Айди шаблона существа", collation: "ascii_general_ci"),
                    Armor = table.Column<int>(type: "int", nullable: false, comment: "Броня")
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
                comment: "Части шаблона существа")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CreatureTemplateSkills",
                schema: "GameRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatureId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "Айди шаблона существа", collation: "ascii_general_ci"),
                    Skill = table.Column<string>(type: "longtext", nullable: false, comment: "Навык")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SkillValue = table.Column<int>(type: "int", nullable: false, comment: "Значение навыка"),
                    CreatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RoleCreatedUser = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ModifiedByUserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RoleModifiedUser = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreatureTemplateSkills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CreatureTemplateSkills_CreatureTemplates_CreatureId",
                        column: x => x.CreatureId,
                        principalSchema: "GameRules",
                        principalTable: "CreatureTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Навыки шаблона существа")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Creatures",
                schema: "Battles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    BattleId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "Айди боя", collation: "ascii_general_ci"),
                    ImgFileId = table.Column<Guid>(type: "char(36)", nullable: true, comment: "Айди графического файла", collation: "ascii_general_ci"),
                    CreatureTemplateId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "Айди шаблона существа", collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: false, comment: "Название существа")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true, comment: "Описание шаблона")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatureType = table.Column<string>(type: "longtext", nullable: false, comment: "Тип существа")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MaxHP = table.Column<int>(type: "int", nullable: false, comment: "Максимальные хиты"),
                    MaxSta = table.Column<int>(type: "int", nullable: false, comment: "Максимальная стамина"),
                    MaxInt = table.Column<int>(type: "int", nullable: false, comment: "Максимальный интеллект"),
                    MaxRef = table.Column<int>(type: "int", nullable: false, comment: "Максимальные рефлексы"),
                    MaxDex = table.Column<int>(type: "int", nullable: false, comment: "Максимальна ловкость"),
                    MaxBody = table.Column<int>(type: "int", nullable: false, comment: "Максимальное телосложение"),
                    MaxEmp = table.Column<int>(type: "int", nullable: false, comment: "Максимальная эмпатия"),
                    MaxCra = table.Column<int>(type: "int", nullable: false, comment: "Максимальный крафт"),
                    MaxWill = table.Column<int>(type: "int", nullable: false, comment: "Максимальная воля"),
                    MaxSpeed = table.Column<int>(type: "int", nullable: false, comment: "Максимальная скорость"),
                    MaxLuck = table.Column<int>(type: "int", nullable: false, comment: "Максимальная удача"),
                    Stun = table.Column<int>(type: "int", nullable: false, comment: "Устойчивость"),
                    HP = table.Column<int>(type: "int", nullable: false, comment: "Хиты"),
                    Sta = table.Column<int>(type: "int", nullable: false, comment: "Стамина"),
                    Int = table.Column<int>(type: "int", nullable: false, comment: "Интеллект"),
                    Ref = table.Column<int>(type: "int", nullable: false, comment: "Рефлексы"),
                    Dex = table.Column<int>(type: "int", nullable: false, comment: "Ловкость"),
                    Body = table.Column<int>(type: "int", nullable: false, comment: "Телосложение"),
                    Emp = table.Column<int>(type: "int", nullable: false, comment: "Эмпатия"),
                    Cra = table.Column<int>(type: "int", nullable: false, comment: "Крафт"),
                    Will = table.Column<int>(type: "int", nullable: false, comment: "Воля"),
                    Luck = table.Column<int>(type: "int", nullable: false, comment: "Удача"),
                    Speed = table.Column<int>(type: "int", nullable: false, comment: "Скорость"),
                    LeadingArmId = table.Column<Guid>(type: "char(36)", nullable: true, comment: "Айди ведущей руки", collation: "ascii_general_ci"),
                    InitiativeInBattle = table.Column<int>(type: "int", nullable: false, comment: "Значение инициативы в битве"),
                    TurnBeginningEffectsAreTriggered = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RoleCreatedUser = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ModifiedByUserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RoleModifiedUser = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
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
                        name: "FK_Creatures_ImgFiles_ImgFileId",
                        column: x => x.ImgFileId,
                        principalSchema: "System",
                        principalTable: "ImgFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                },
                comment: "Существа")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CreatureAbilities",
                schema: "Battles",
                columns: table => new
                {
                    AbilitiesId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreaturesId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
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
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CreatureDamageTypeModifier",
                schema: "Battles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatureId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RoleCreatedUser = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ModifiedByUserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RoleModifiedUser = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DamageType = table.Column<int>(type: "int", nullable: false),
                    DamageTypeModifier = table.Column<int>(type: "int", nullable: false),
                    PrimaryEntityid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreatureDamageTypeModifier", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CreatureDamageTypeModifier_Creatures_CreatureId",
                        column: x => x.CreatureId,
                        principalSchema: "Battles",
                        principalTable: "Creatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CreatureParts",
                schema: "Battles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatureId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "Айди существа", collation: "ascii_general_ci"),
                    StartingArmor = table.Column<int>(type: "int", nullable: false, comment: "Стартовая броня"),
                    CurrentArmor = table.Column<int>(type: "int", nullable: false, comment: "Текущая броня")
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
                comment: "Части существа")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CreatureSkills",
                schema: "Battles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatureId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "Айди существа", collation: "ascii_general_ci"),
                    Skill = table.Column<string>(type: "longtext", nullable: false, comment: "Навык")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MaxValue = table.Column<int>(type: "int", nullable: false, comment: "Макксимальное значение навыка"),
                    SkillValue = table.Column<int>(type: "int", nullable: false, comment: "Значение навыка"),
                    CreatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RoleCreatedUser = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ModifiedByUserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RoleModifiedUser = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreatureSkills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CreatureSkills_Creatures_CreatureId",
                        column: x => x.CreatureId,
                        principalSchema: "Battles",
                        principalTable: "Creatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Навыки существа")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Effects",
                schema: "Battles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: false, comment: "Название эффекта")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatureId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "Айди существа", collation: "ascii_general_ci"),
                    CreatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RoleCreatedUser = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ModifiedByUserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RoleModifiedUser = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Effects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Effects_Creatures_CreatureId",
                        column: x => x.CreatureId,
                        principalSchema: "Battles",
                        principalTable: "Creatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "BleedEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BleedEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BleedEffects_Effects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "Effects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты кровотечения")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "BleedingWoundEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Severity = table.Column<int>(type: "int", nullable: false, comment: "Тяжесть"),
                    Damage = table.Column<int>(type: "int", nullable: false, comment: "Урон")
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
                comment: "Эффекты кровавой раны")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CritEffects",
                schema: "Battles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Severity = table.Column<string>(type: "longtext", nullable: false, comment: "Тяжесть критического эффекта")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BodyPartLocation = table.Column<string>(type: "longtext", nullable: false, comment: "Тип части тела")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreaturePartId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "Айди части тела", collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CritEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CritEffects_CreatureParts_CreaturePartId",
                        column: x => x.CreaturePartId,
                        principalSchema: "Battles",
                        principalTable: "CreatureParts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CritEffects_Effects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "Effects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Критические эффекты")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DeadEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeadEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeadEffects_Effects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "Effects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты смерти")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DyingEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Counter = table.Column<int>(type: "int", nullable: false, comment: "Модификатор сложности")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DyingEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DyingEffects_Effects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "Effects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты при смерти")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "FireEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
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
                comment: "Эффекты горения")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "FreezeEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
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
                comment: "Эффекты заморозки")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PoisonEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PoisonEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PoisonEffects_Effects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "Effects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты отравления")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "StaggeredEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaggeredEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StaggeredEffects_Effects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "Effects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты ошеломления")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "StunEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
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
                comment: "Эффекты дезориентации")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SufflocationEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SufflocationEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SufflocationEffects_Effects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "Effects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты удушья")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ComplexArmCritEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    PenaltyApplied = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "Пенальти применено")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComplexArmCritEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComplexArmCritEffects_CritEffects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "CritEffects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты перелома руки")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ComplexHead1CritEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComplexHead1CritEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComplexHead1CritEffects_CritEffects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "CritEffects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты выбитых зубов")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ComplexHead2CritEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComplexHead2CritEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComplexHead2CritEffects_CritEffects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "CritEffects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты небольшой травмы головы")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ComplexLegCritEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    PenaltyApplied = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "Пенальти применено")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComplexLegCritEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComplexLegCritEffects_CritEffects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "CritEffects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты перелома ноги")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ComplexTailCritEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    PenaltyApplied = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "Пенальти применено")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComplexTailCritEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComplexTailCritEffects_CritEffects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "CritEffects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты перелома хвоста")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ComplexTorso1CritEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComplexTorso1CritEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComplexTorso1CritEffects_CritEffects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "CritEffects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты сломанных ребер")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ComplexTorso2CritEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RoundCounter = table.Column<int>(type: "int", nullable: false, comment: "Счетчик раундов")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComplexTorso2CritEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComplexTorso2CritEffects_CritEffects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "CritEffects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты разрыва селезенки")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ComplexWingCritEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    PenaltyApplied = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "Пенальти применено")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComplexWingCritEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComplexWingCritEffects_CritEffects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "CritEffects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты перелома крыла")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DeadlyArmCritEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    PenaltyApplied = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "Пенальти применено")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeadlyArmCritEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeadlyArmCritEffects_CritEffects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "CritEffects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты потери руки")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DeadlyHead1CritEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeadlyHead1CritEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeadlyHead1CritEffects_CritEffects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "CritEffects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты повреждения глаза")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DeadlyHead2CritEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeadlyHead2CritEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeadlyHead2CritEffects_CritEffects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "CritEffects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты отсечения головы")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DeadlyLegCritEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SpeedModifier = table.Column<int>(type: "int", nullable: false, comment: "Модификатор скорости"),
                    DodgeModifier = table.Column<int>(type: "int", nullable: false, comment: "Модификатор уклонения"),
                    AthleticsModifier = table.Column<int>(type: "int", nullable: false, comment: "Модификатор атлетики"),
                    PenaltyApplied = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "Пенальти применено")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeadlyLegCritEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeadlyLegCritEffects_CritEffects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "CritEffects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты потери ноги")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DeadlyTailCritEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DodgeModifier = table.Column<int>(type: "int", nullable: false, comment: "Модификатор уклонения"),
                    AthleticsModifier = table.Column<int>(type: "int", nullable: false, comment: "Модификатор атлетики"),
                    PenaltyApplied = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "Пенальти применено")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeadlyTailCritEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeadlyTailCritEffects_CritEffects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "CritEffects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты потери хвоста")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DeadlyTorso1CritEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    StaModifier = table.Column<int>(type: "int", nullable: false, comment: "Модификатор стамины"),
                    AfterTreatStaModifier = table.Column<int>(type: "int", nullable: false, comment: "Модификатор стамины после стабилизации")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeadlyTorso1CritEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeadlyTorso1CritEffects_CritEffects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "CritEffects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты септического шока")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DeadlyTorso2CritEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SpeedModifier = table.Column<int>(type: "int", nullable: false, comment: "Модификатор скорости"),
                    AfterTreatSpeedModifier = table.Column<int>(type: "int", nullable: false, comment: "Модификатор скорости после стабилизации"),
                    BodyModifier = table.Column<int>(type: "int", nullable: false, comment: "Модификатор телосложения"),
                    AfterTreatBodyModifier = table.Column<int>(type: "int", nullable: false, comment: "Модификатор телосложения после стабилизации"),
                    StaModifier = table.Column<int>(type: "int", nullable: false, comment: "Модификатор стамины"),
                    AfterTreatStaModifier = table.Column<int>(type: "int", nullable: false, comment: "Модификатор стамины после стабилизации")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeadlyTorso2CritEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeadlyTorso2CritEffects_CritEffects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "CritEffects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты травмы сердца")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DeadlyWingCritEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SpeedModifier = table.Column<int>(type: "int", nullable: false, comment: "Модификатор скорости"),
                    DodgeModifier = table.Column<int>(type: "int", nullable: false, comment: "Модификатор уклонения"),
                    AthleticsModifier = table.Column<int>(type: "int", nullable: false, comment: "Модификатор атлетики"),
                    PenaltyApplied = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "Пенальти применено")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeadlyWingCritEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeadlyWingCritEffects_CritEffects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "CritEffects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты потери крыла")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DifficultArmCritEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    PenaltyApplied = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "Пенальти применено")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DifficultArmCritEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DifficultArmCritEffects_CritEffects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "CritEffects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты открытого перелома руки")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DifficultHead1CritEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RoundCounter = table.Column<int>(type: "int", nullable: false, comment: "Счетчик раундов"),
                    NextCheck = table.Column<int>(type: "int", nullable: false, comment: "Раунд следующей проверки")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DifficultHead1CritEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DifficultHead1CritEffects_CritEffects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "CritEffects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты контузии")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DifficultHead2CritEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DifficultHead2CritEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DifficultHead2CritEffects_CritEffects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "CritEffects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты проломленного черепа")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DifficultLegCritEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SpeedModifier = table.Column<int>(type: "int", nullable: false, comment: "Модификатор скорости"),
                    AfterTreatSpeedModifier = table.Column<int>(type: "int", nullable: false, comment: "Модификатор скорости после стабилизации"),
                    DodgeModifier = table.Column<int>(type: "int", nullable: false, comment: "Модификатор уклонения"),
                    AfterTreatDodgeModifier = table.Column<int>(type: "int", nullable: false, comment: "Модификатор уклонения после стабилизации"),
                    AthleticsModifier = table.Column<int>(type: "int", nullable: false, comment: "Модификатор атлетики"),
                    AfterTreatAthleticsModifier = table.Column<int>(type: "int", nullable: false, comment: "Модификатор атлетики после стабилизации"),
                    PenaltyApplied = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "Пенальти применено")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DifficultLegCritEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DifficultLegCritEffects_CritEffects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "CritEffects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты открытого перелома ноги")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DifficultTailCritEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DodgeModifier = table.Column<int>(type: "int", nullable: false, comment: "Модификатор уклонения"),
                    AfterTreatDodgeModifier = table.Column<int>(type: "int", nullable: false, comment: "Модификатор уклонения после стабилизации"),
                    AthleticsModifier = table.Column<int>(type: "int", nullable: false, comment: "Модификатор атлетики"),
                    AfterTreatAthleticsModifier = table.Column<int>(type: "int", nullable: false, comment: "Модификатор атлетики после стабилизации"),
                    PenaltyApplied = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "Пенальти применено")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DifficultTailCritEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DifficultTailCritEffects_CritEffects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "CritEffects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты открытого перелома хвоста")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DifficultTorso1CritEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DifficultTorso1CritEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DifficultTorso1CritEffects_CritEffects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "CritEffects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты сосущей раны грудной клетки")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DifficultTorso2CritEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DifficultTorso2CritEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DifficultTorso2CritEffects_CritEffects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "CritEffects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты раны в живот")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DifficultWingCritEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SpeedModifier = table.Column<int>(type: "int", nullable: false, comment: "Модификатор скорости"),
                    AfterTreatSpeedModifier = table.Column<int>(type: "int", nullable: false, comment: "Модификатор скорости после стабилизации"),
                    DodgeModifier = table.Column<int>(type: "int", nullable: false, comment: "Модификатор уклонения"),
                    AfterTreatDodgeModifier = table.Column<int>(type: "int", nullable: false, comment: "Модификатор уклонения после стабилизации"),
                    AthleticsModifier = table.Column<int>(type: "int", nullable: false, comment: "Модификатор атлетики"),
                    AfterTreatAthleticsModifier = table.Column<int>(type: "int", nullable: false, comment: "Модификатор атлетики после стабилизации"),
                    PenaltyApplied = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "Пенальти применено")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DifficultWingCritEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DifficultWingCritEffects_CritEffects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "CritEffects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты открытого перелома крыла")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SimpleArmCritEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    PenaltyApplied = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "Пенальти применено")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SimpleArmCritEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SimpleArmCritEffects_CritEffects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "CritEffects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты вывиха руки")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SimpleHead1CritEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SimpleHead1CritEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SimpleHead1CritEffects_CritEffects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "CritEffects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты уродующего шрама")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SimpleHead2CritEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SimpleHead2CritEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SimpleHead2CritEffects_CritEffects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "CritEffects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты треснувшей челюсти")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SimpleLegCritEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    PenaltyApplied = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "Пенальти применено")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SimpleLegCritEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SimpleLegCritEffects_CritEffects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "CritEffects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты вывиха ноги")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SimpleTailCritEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    PenaltyApplied = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "Пенальти применено")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SimpleTailCritEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SimpleTailCritEffects_CritEffects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "CritEffects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты вывиха крыла")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SimpleTorso1CritEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SimpleTorso1CritEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SimpleTorso1CritEffects_CritEffects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "CritEffects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты инородного объекта")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SimpleTorso2CritEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SimpleTorso2CritEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SimpleTorso2CritEffects_CritEffects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "CritEffects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты треснувших ребер")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SimpleWingCritEffects",
                schema: "Effects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    PenaltyApplied = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "Пенальти применено")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SimpleWingCritEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SimpleWingCritEffects_CritEffects_Id",
                        column: x => x.Id,
                        principalSchema: "Battles",
                        principalTable: "CritEffects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Эффекты вывиха крыла")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Characters",
                schema: "Characters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UserGameActivatedId = table.Column<Guid>(type: "char(36)", nullable: true, comment: "Айди активировашего персонажа пользователя", collation: "ascii_general_ci"),
                    ActivationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true, comment: "Время активации персонажа")
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
                comment: "Персонажи")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserGameCharacters",
                schema: "Characters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CharacterId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "Айди персонажа", collation: "ascii_general_ci"),
                    UserGameId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "Айди пользователя игры", collation: "ascii_general_ci"),
                    InterfaceId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "Айди интерфейса", collation: "ascii_general_ci"),
                    CreatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RoleCreatedUser = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ModifiedByUserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RoleModifiedUser = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
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
                comment: "Персонажи пользователя игры")
                .Annotation("MySql:CharSet", "utf8mb4");

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
                    { new Guid("8094e0d0-3147-4791-9053-9667cbe107d7"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "AdminRole", "Default", "Default" },
                    { new Guid("8094e0d0-3148-4791-9053-9667cbe107d8"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "UserRole", "Default", "Default" }
                });

            migrationBuilder.InsertData(
                schema: "System",
                table: "Users",
                columns: new[] { "Id", "AvatarId", "CreatedByUserId", "CreatedOn", "Email", "InterfaceId", "ModifiedByUserId", "ModifiedOn", "Name", "Phone", "RoleCreatedUser", "RoleModifiedUser" },
                values: new object[] { new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), null, new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "andmin@email.ru", new Guid("8094e0d0-3137-4791-9053-9667cbe107d7"), new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Системный пользователь", "Нет", "Default", "Default" });

            migrationBuilder.CreateIndex(
                name: "IX_Abilities_GameId",
                schema: "GameRules",
                table: "Abilities",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_AppliedConditions_AbilityId",
                schema: "GameRules",
                table: "AppliedConditions",
                column: "AbilityId");

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
                name: "IX_CreatureDamageTypeModifier_CreatureId",
                schema: "Battles",
                table: "CreatureDamageTypeModifier",
                column: "CreatureId");

            migrationBuilder.CreateIndex(
                name: "IX_CreatureParts_CreatureId",
                schema: "Battles",
                table: "CreatureParts",
                column: "CreatureId");

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
                name: "IX_Creatures_ImgFileId",
                schema: "Battles",
                table: "Creatures",
                column: "ImgFileId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CreatureSkills_CreatureId",
                schema: "Battles",
                table: "CreatureSkills",
                column: "CreatureId");

            migrationBuilder.CreateIndex(
                name: "IX_CreatureTemplateAbilities_CreatureTemplatesId",
                schema: "GameRules",
                table: "CreatureTemplateAbilities",
                column: "CreatureTemplatesId");

            migrationBuilder.CreateIndex(
                name: "IX_CreatureTemplateDamageTypeModifier_CreatureTemplateId",
                schema: "GameRules",
                table: "CreatureTemplateDamageTypeModifier",
                column: "CreatureTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_CreatureTemplateParts_CreatureTemplateId",
                schema: "GameRules",
                table: "CreatureTemplateParts",
                column: "CreatureTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_CreatureTemplates_BodyTemplateId",
                schema: "GameRules",
                table: "CreatureTemplates",
                column: "BodyTemplateId");

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
                name: "IX_CreatureTemplateSkills_CreatureId",
                schema: "GameRules",
                table: "CreatureTemplateSkills",
                column: "CreatureId");

            migrationBuilder.CreateIndex(
                name: "IX_CritEffects_CreaturePartId",
                schema: "Battles",
                table: "CritEffects",
                column: "CreaturePartId");

            migrationBuilder.CreateIndex(
                name: "IX_DefensiveSkill_AbilityId",
                schema: "GameRules",
                table: "DefensiveSkill",
                column: "AbilityId");

            migrationBuilder.CreateIndex(
                name: "IX_Effects_CreatureId",
                schema: "Battles",
                table: "Effects",
                column: "CreatureId");

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

        /// <inheritdoc />
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
                name: "AppliedConditions",
                schema: "GameRules");

            migrationBuilder.DropTable(
                name: "BleedEffects",
                schema: "Effects");

            migrationBuilder.DropTable(
                name: "BleedingWoundEffects",
                schema: "Effects");

            migrationBuilder.DropTable(
                name: "BodyTemplateParts",
                schema: "GameRules");

            migrationBuilder.DropTable(
                name: "ComplexArmCritEffects",
                schema: "Effects");

            migrationBuilder.DropTable(
                name: "ComplexHead1CritEffects",
                schema: "Effects");

            migrationBuilder.DropTable(
                name: "ComplexHead2CritEffects",
                schema: "Effects");

            migrationBuilder.DropTable(
                name: "ComplexLegCritEffects",
                schema: "Effects");

            migrationBuilder.DropTable(
                name: "ComplexTailCritEffects",
                schema: "Effects");

            migrationBuilder.DropTable(
                name: "ComplexTorso1CritEffects",
                schema: "Effects");

            migrationBuilder.DropTable(
                name: "ComplexTorso2CritEffects",
                schema: "Effects");

            migrationBuilder.DropTable(
                name: "ComplexWingCritEffects",
                schema: "Effects");

            migrationBuilder.DropTable(
                name: "CreatureAbilities",
                schema: "Battles");

            migrationBuilder.DropTable(
                name: "CreatureDamageTypeModifier",
                schema: "Battles");

            migrationBuilder.DropTable(
                name: "CreatureSkills",
                schema: "Battles");

            migrationBuilder.DropTable(
                name: "CreatureTemplateAbilities",
                schema: "GameRules");

            migrationBuilder.DropTable(
                name: "CreatureTemplateDamageTypeModifier",
                schema: "GameRules");

            migrationBuilder.DropTable(
                name: "CreatureTemplateParts",
                schema: "GameRules");

            migrationBuilder.DropTable(
                name: "CreatureTemplateSkills",
                schema: "GameRules");

            migrationBuilder.DropTable(
                name: "DeadEffects",
                schema: "Effects");

            migrationBuilder.DropTable(
                name: "DeadlyArmCritEffects",
                schema: "Effects");

            migrationBuilder.DropTable(
                name: "DeadlyHead1CritEffects",
                schema: "Effects");

            migrationBuilder.DropTable(
                name: "DeadlyHead2CritEffects",
                schema: "Effects");

            migrationBuilder.DropTable(
                name: "DeadlyLegCritEffects",
                schema: "Effects");

            migrationBuilder.DropTable(
                name: "DeadlyTailCritEffects",
                schema: "Effects");

            migrationBuilder.DropTable(
                name: "DeadlyTorso1CritEffects",
                schema: "Effects");

            migrationBuilder.DropTable(
                name: "DeadlyTorso2CritEffects",
                schema: "Effects");

            migrationBuilder.DropTable(
                name: "DeadlyWingCritEffects",
                schema: "Effects");

            migrationBuilder.DropTable(
                name: "DefensiveSkill",
                schema: "GameRules");

            migrationBuilder.DropTable(
                name: "DifficultArmCritEffects",
                schema: "Effects");

            migrationBuilder.DropTable(
                name: "DifficultHead1CritEffects",
                schema: "Effects");

            migrationBuilder.DropTable(
                name: "DifficultHead2CritEffects",
                schema: "Effects");

            migrationBuilder.DropTable(
                name: "DifficultLegCritEffects",
                schema: "Effects");

            migrationBuilder.DropTable(
                name: "DifficultTailCritEffects",
                schema: "Effects");

            migrationBuilder.DropTable(
                name: "DifficultTorso1CritEffects",
                schema: "Effects");

            migrationBuilder.DropTable(
                name: "DifficultTorso2CritEffects",
                schema: "Effects");

            migrationBuilder.DropTable(
                name: "DifficultWingCritEffects",
                schema: "Effects");

            migrationBuilder.DropTable(
                name: "DyingEffects",
                schema: "Effects");

            migrationBuilder.DropTable(
                name: "FireEffects",
                schema: "Effects");

            migrationBuilder.DropTable(
                name: "FreezeEffects",
                schema: "Effects");

            migrationBuilder.DropTable(
                name: "GameImgFile");

            migrationBuilder.DropTable(
                name: "GameTextFile");

            migrationBuilder.DropTable(
                name: "ImgFileUser",
                schema: "System");

            migrationBuilder.DropTable(
                name: "PoisonEffects",
                schema: "Effects");

            migrationBuilder.DropTable(
                name: "SimpleArmCritEffects",
                schema: "Effects");

            migrationBuilder.DropTable(
                name: "SimpleHead1CritEffects",
                schema: "Effects");

            migrationBuilder.DropTable(
                name: "SimpleHead2CritEffects",
                schema: "Effects");

            migrationBuilder.DropTable(
                name: "SimpleLegCritEffects",
                schema: "Effects");

            migrationBuilder.DropTable(
                name: "SimpleTailCritEffects",
                schema: "Effects");

            migrationBuilder.DropTable(
                name: "SimpleTorso1CritEffects",
                schema: "Effects");

            migrationBuilder.DropTable(
                name: "SimpleTorso2CritEffects",
                schema: "Effects");

            migrationBuilder.DropTable(
                name: "SimpleWingCritEffects",
                schema: "Effects");

            migrationBuilder.DropTable(
                name: "StaggeredEffects",
                schema: "Effects");

            migrationBuilder.DropTable(
                name: "StunEffects",
                schema: "Effects");

            migrationBuilder.DropTable(
                name: "SufflocationEffects",
                schema: "Effects");

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
                name: "Abilities",
                schema: "GameRules");

            migrationBuilder.DropTable(
                name: "CritEffects",
                schema: "Battles");

            migrationBuilder.DropTable(
                name: "TextFiles",
                schema: "System");

            migrationBuilder.DropTable(
                name: "SystemRoles",
                schema: "System");

            migrationBuilder.DropTable(
                name: "CreatureParts",
                schema: "Battles");

            migrationBuilder.DropTable(
                name: "Effects",
                schema: "Battles");

            migrationBuilder.DropTable(
                name: "BodyParts",
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
