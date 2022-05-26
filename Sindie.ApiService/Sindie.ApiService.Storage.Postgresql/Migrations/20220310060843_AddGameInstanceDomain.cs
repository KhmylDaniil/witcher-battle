using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sindie.ApiService.Storage.Postgresql.Migrations
{
    public partial class AddGameInstanceDomain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bags_Games_GameId",
                table: "Bags");

            migrationBuilder.DropForeignKey(
                name: "FK_Body_Characters_CharacterId",
                table: "Body");

            migrationBuilder.DropForeignKey(
                name: "FK_Body_Slots_SlotId",
                table: "Body");

            migrationBuilder.DropForeignKey(
                name: "FK_BodyItem_Characters_CharacterId",
                table: "BodyItem");

            migrationBuilder.DropForeignKey(
                name: "FK_BodyItem_Items_ItemId",
                table: "BodyItem");

            migrationBuilder.DropForeignKey(
                name: "FK_Characters_Bags_BagId",
                table: "Characters");

            migrationBuilder.DropForeignKey(
                name: "FK_Characters_Games_GameId",
                table: "Characters");

            migrationBuilder.DropForeignKey(
                name: "FK_Characters_UserCharacters_UserActivateId",
                table: "Characters");

            migrationBuilder.DropTable(
                name: "CharacterCharacteristic");

            migrationBuilder.DropTable(
                name: "CharacteristicModifier");

            migrationBuilder.DropTable(
                name: "ModifierParametrs");

            migrationBuilder.DropTable(
                name: "ParameterItems");

            migrationBuilder.DropTable(
                name: "UserCharacters");

            migrationBuilder.DropIndex(
                name: "IX_Scripts_Name",
                schema: "GameRules",
                table: "Scripts");

            migrationBuilder.DropIndex(
                name: "IX_Parties_Name",
                schema: "InteractionRules",
                table: "Parties");

            migrationBuilder.DropIndex(
                name: "IX_ItemTemplates_Name",
                schema: "GameRules",
                table: "ItemTemplates");

            migrationBuilder.DropIndex(
                name: "IX_CharacterTemplates_Name",
                schema: "GameRules",
                table: "CharacterTemplates");

            migrationBuilder.DropIndex(
                name: "IX_BodyItem_CharacterId",
                table: "BodyItem");

            migrationBuilder.DropIndex(
                name: "IX_BodyItem_ItemId",
                table: "BodyItem");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationAreas_Name",
                schema: "InteractionRules",
                table: "ApplicationAreas");

            migrationBuilder.DropIndex(
                name: "IX_Activities_Name",
                schema: "InteractionRules",
                table: "Activities");

            migrationBuilder.DropIndex(
                name: "IX_Actions_Name",
                schema: "InteractionRules",
                table: "Actions");

            migrationBuilder.DropIndex(
                name: "IX_Characters_BagId",
                table: "Characters");

            migrationBuilder.DropIndex(
                name: "IX_Characters_GameId",
                table: "Characters");

            migrationBuilder.DropIndex(
                name: "IX_Characters_UserActivateId",
                table: "Characters");

            migrationBuilder.DropIndex(
                name: "IX_Bags_GameId",
                table: "Bags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Body",
                table: "Body");

            migrationBuilder.DropColumn(
                name: "CharacterId",
                table: "BodyItem");

            migrationBuilder.DropColumn(
                name: "CounterWearing_QuantityItem",
                table: "BodyItem");

            migrationBuilder.DropColumn(
                name: "CounterWearing_TimeWear",
                table: "BodyItem");

            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "BodyItem");

            migrationBuilder.DropColumn(
                name: "CharacterInInteraction",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "GameId",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "TimeActivate",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "TypeCharacter",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "UserActivateId",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "GameId",
                table: "Bags");

            migrationBuilder.DropColumn(
                name: "MaxOccupiedBagSize",
                table: "Bags");

            migrationBuilder.DropColumn(
                name: "BodySlotActivity_CounterWearing",
                table: "Body");

            migrationBuilder.DropColumn(
                name: "BodySlotActivity_InActivationTime",
                table: "Body");

            migrationBuilder.DropColumn(
                name: "BodySlotActivity_MaxQuantityWearing",
                table: "Body");

            migrationBuilder.DropColumn(
                name: "BodySlotActivity_PeriodOfInactivity",
                table: "Body");

            migrationBuilder.DropColumn(
                name: "MaxQuantityInSlot",
                table: "Body");

            migrationBuilder.EnsureSchema(
                name: "GameInstance");

            migrationBuilder.RenameTable(
                name: "Characters",
                newName: "Characters",
                newSchema: "GameInstance");

            migrationBuilder.RenameTable(
                name: "CharacterParameters",
                newName: "CharacterParameters",
                newSchema: "GameInstance");

            migrationBuilder.RenameTable(
                name: "Bags",
                newName: "Bags",
                newSchema: "GameInstance");

            migrationBuilder.RenameTable(
                name: "BagItems",
                newName: "BagItems",
                newSchema: "GameInstance");

            migrationBuilder.RenameTable(
                name: "Body",
                newName: "Bodies",
                newSchema: "GameInstance");

            migrationBuilder.RenameColumn(
                name: "ParameterValue",
                schema: "GameInstance",
                table: "CharacterParameters",
                newName: "ParametrValue");

            migrationBuilder.RenameIndex(
                name: "IX_Body_SlotId",
                schema: "GameInstance",
                table: "Bodies",
                newName: "IX_Bodies_SlotId");

            migrationBuilder.RenameIndex(
                name: "IX_Body_CharacterId",
                schema: "GameInstance",
                table: "Bodies",
                newName: "IX_Bodies_CharacterId");

            migrationBuilder.AlterTable(
                name: "Characters",
                schema: "GameInstance",
                comment: "Персонажи");

            migrationBuilder.AlterTable(
                name: "CharacterParameters",
                schema: "GameInstance",
                comment: "Параметры персонажа");

            migrationBuilder.AlterTable(
                name: "Bags",
                schema: "GameInstance",
                comment: "Сумки");

            migrationBuilder.AlterTable(
                name: "BagItems",
                schema: "GameInstance",
                comment: "Предметы в сумке");

            migrationBuilder.AlterTable(
                name: "Bodies",
                schema: "GameInstance",
                comment: "Тела");

            migrationBuilder.AlterColumn<Guid>(
                name: "ImgFileId",
                schema: "GameRules",
                table: "CharacterTemplates",
                type: "uuid",
                nullable: true,
                comment: "Айди графического файла",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "Айди графического файла(аватарки)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "GameInstance",
                table: "Characters",
                type: "text",
                nullable: false,
                comment: "Имя персонажа",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                schema: "GameInstance",
                table: "Characters",
                type: "text",
                nullable: true,
                comment: "Описание персонажа",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "BagId",
                schema: "GameInstance",
                table: "Characters",
                type: "uuid",
                nullable: true,
                comment: "Айди сумки",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ActivationTime",
                schema: "GameInstance",
                table: "Characters",
                type: "timestamp with time zone",
                nullable: true,
                comment: "Время активации персонажа");

            migrationBuilder.AddColumn<Guid>(
                name: "CharacterTemplateId",
                schema: "GameInstance",
                table: "Characters",
                type: "uuid",
                nullable: true,
                comment: "Айди шаблона персонажа");

            migrationBuilder.AddColumn<Guid>(
                name: "ImgFileId",
                schema: "GameInstance",
                table: "Characters",
                type: "uuid",
                nullable: true,
                comment: "Айди графического файла");

            migrationBuilder.AddColumn<Guid>(
                name: "InstanceId",
                schema: "GameInstance",
                table: "Characters",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "Айди экземпляра игры");

            migrationBuilder.AddColumn<Guid>(
                name: "TextFileId",
                schema: "GameInstance",
                table: "Characters",
                type: "uuid",
                nullable: true,
                comment: "Айди текстового файла");

            migrationBuilder.AddColumn<Guid>(
                name: "UserGameActivatedId",
                schema: "GameInstance",
                table: "Characters",
                type: "uuid",
                nullable: true,
                comment: "Айди активировашего персонажа пользователя");

            migrationBuilder.AlterColumn<Guid>(
                name: "ParameterId",
                schema: "GameInstance",
                table: "CharacterParameters",
                type: "uuid",
                nullable: false,
                comment: "Айди параметра",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "CharacterId",
                schema: "GameInstance",
                table: "CharacterParameters",
                type: "uuid",
                nullable: false,
                comment: "Айди персонажа",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<double>(
                name: "ParametrValue",
                schema: "GameInstance",
                table: "CharacterParameters",
                type: "double precision",
                nullable: false,
                comment: "Значение параметра",
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "GameInstance",
                table: "Bags",
                type: "text",
                nullable: false,
                comment: "Название сумки",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "MaxBagSize",
                schema: "GameInstance",
                table: "Bags",
                type: "integer",
                nullable: true,
                comment: "Максимальный размер сумки",
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                schema: "GameInstance",
                table: "Bags",
                type: "text",
                nullable: true,
                comment: "Описание сумки",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CharacterId",
                schema: "GameInstance",
                table: "Bags",
                type: "uuid",
                nullable: true,
                comment: "Айди персонажа");

            migrationBuilder.AddColumn<Guid>(
                name: "ImgFileId",
                schema: "GameInstance",
                table: "Bags",
                type: "uuid",
                nullable: true,
                comment: "Айди графического файла");

            migrationBuilder.AddColumn<Guid>(
                name: "InstanceId",
                schema: "GameInstance",
                table: "Bags",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "Айди экземпляра игры");

            migrationBuilder.AlterColumn<int>(
                name: "Stack",
                schema: "GameInstance",
                table: "BagItems",
                type: "integer",
                nullable: false,
                comment: "Стек",
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "QuantityItem",
                schema: "GameInstance",
                table: "BagItems",
                type: "integer",
                nullable: false,
                comment: "Количество предметов в стеке",
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<Guid>(
                name: "ItemId",
                schema: "GameInstance",
                table: "BagItems",
                type: "uuid",
                nullable: false,
                comment: "Айди предмета",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "BagId",
                schema: "GameInstance",
                table: "BagItems",
                type: "uuid",
                nullable: false,
                comment: "Айди сумки",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "SlotId",
                schema: "GameInstance",
                table: "Bodies",
                type: "uuid",
                nullable: false,
                comment: "Айди слота",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "CharacterId",
                schema: "GameInstance",
                table: "Bodies",
                type: "uuid",
                nullable: false,
                comment: "Айди персонажа",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "ItemId",
                schema: "GameInstance",
                table: "Bodies",
                type: "uuid",
                nullable: true,
                comment: "Айди предмета");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Bodies",
                schema: "GameInstance",
                table: "Bodies",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "CharacterModifiers",
                schema: "GameInstance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    CharacterId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди персонажа"),
                    ModifierId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди модификатора"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterModifiers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CharacterModifiers_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalSchema: "GameInstance",
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterModifiers_Modifiers_ModifierId",
                        column: x => x.ModifierId,
                        principalSchema: "GameRules",
                        principalTable: "Modifiers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Модификаторы персонажа");

            migrationBuilder.CreateTable(
                name: "Instances",
                schema: "GameInstance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    GameId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди игры"),
                    ImgFileId = table.Column<Guid>(type: "uuid", nullable: true, comment: "Айди графического файла"),
                    UserGameActivatedId = table.Column<Guid>(type: "uuid", nullable: true, comment: "Айди активировавшего игру пользователя"),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Название экземпляра"),
                    Description = table.Column<string>(type: "text", nullable: true, comment: "Описание экземпляра"),
                    ActivationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Время активации экземпляра"),
                    DateOfGame = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Дата проведения игры"),
                    StoryAboutRules = table.Column<string>(type: "text", nullable: true, comment: "Описание правил игры"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Instances_Games_GameId",
                        column: x => x.GameId,
                        principalSchema: "BaseGame",
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Instances_ImgFiles_ImgFileId",
                        column: x => x.ImgFileId,
                        principalSchema: "System",
                        principalTable: "ImgFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Instances_UserGames_UserGameActivatedId",
                        column: x => x.UserGameActivatedId,
                        principalSchema: "BaseGame",
                        principalTable: "UserGames",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                },
                comment: "Экземпляры");

            migrationBuilder.CreateTable(
                name: "UserGameCharacters",
                schema: "GameInstance",
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
                        principalSchema: "GameInstance",
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

            migrationBuilder.CreateIndex(
                name: "IX_Characters_CharacterTemplateId",
                schema: "GameInstance",
                table: "Characters",
                column: "CharacterTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_ImgFileId",
                schema: "GameInstance",
                table: "Characters",
                column: "ImgFileId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Characters_InstanceId",
                schema: "GameInstance",
                table: "Characters",
                column: "InstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_TextFileId",
                schema: "GameInstance",
                table: "Characters",
                column: "TextFileId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Characters_UserGameActivatedId",
                schema: "GameInstance",
                table: "Characters",
                column: "UserGameActivatedId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bags_CharacterId",
                schema: "GameInstance",
                table: "Bags",
                column: "CharacterId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bags_ImgFileId",
                schema: "GameInstance",
                table: "Bags",
                column: "ImgFileId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bags_InstanceId",
                schema: "GameInstance",
                table: "Bags",
                column: "InstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_Bodies_ItemId",
                schema: "GameInstance",
                table: "Bodies",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterModifiers_CharacterId",
                schema: "GameInstance",
                table: "CharacterModifiers",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterModifiers_ModifierId",
                schema: "GameInstance",
                table: "CharacterModifiers",
                column: "ModifierId");

            migrationBuilder.CreateIndex(
                name: "IX_Instances_GameId",
                schema: "GameInstance",
                table: "Instances",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Instances_ImgFileId",
                schema: "GameInstance",
                table: "Instances",
                column: "ImgFileId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Instances_UserGameActivatedId",
                schema: "GameInstance",
                table: "Instances",
                column: "UserGameActivatedId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGameCharacters_CharacterId",
                schema: "GameInstance",
                table: "UserGameCharacters",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGameCharacters_InterfaceId",
                schema: "GameInstance",
                table: "UserGameCharacters",
                column: "InterfaceId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGameCharacters_UserGameId",
                schema: "GameInstance",
                table: "UserGameCharacters",
                column: "UserGameId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bags_Characters_CharacterId",
                schema: "GameInstance",
                table: "Bags",
                column: "CharacterId",
                principalSchema: "GameInstance",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Bags_ImgFiles_ImgFileId",
                schema: "GameInstance",
                table: "Bags",
                column: "ImgFileId",
                principalSchema: "System",
                principalTable: "ImgFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Bags_Instances_InstanceId",
                schema: "GameInstance",
                table: "Bags",
                column: "InstanceId",
                principalSchema: "GameInstance",
                principalTable: "Instances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bodies_Characters_CharacterId",
                schema: "GameInstance",
                table: "Bodies",
                column: "CharacterId",
                principalSchema: "GameInstance",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bodies_Items_ItemId",
                schema: "GameInstance",
                table: "Bodies",
                column: "ItemId",
                principalSchema: "GameRules",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Bodies_Slots_SlotId",
                schema: "GameInstance",
                table: "Bodies",
                column: "SlotId",
                principalSchema: "GameRules",
                principalTable: "Slots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_CharacterTemplates_CharacterTemplateId",
                schema: "GameInstance",
                table: "Characters",
                column: "CharacterTemplateId",
                principalSchema: "GameRules",
                principalTable: "CharacterTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_ImgFiles_ImgFileId",
                schema: "GameInstance",
                table: "Characters",
                column: "ImgFileId",
                principalSchema: "System",
                principalTable: "ImgFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_Instances_InstanceId",
                schema: "GameInstance",
                table: "Characters",
                column: "InstanceId",
                principalSchema: "GameInstance",
                principalTable: "Instances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_TextFiles_TextFileId",
                schema: "GameInstance",
                table: "Characters",
                column: "TextFileId",
                principalSchema: "System",
                principalTable: "TextFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_UserGameCharacters_UserGameActivatedId",
                schema: "GameInstance",
                table: "Characters",
                column: "UserGameActivatedId",
                principalSchema: "GameInstance",
                principalTable: "UserGameCharacters",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bags_Characters_CharacterId",
                schema: "GameInstance",
                table: "Bags");

            migrationBuilder.DropForeignKey(
                name: "FK_Bags_ImgFiles_ImgFileId",
                schema: "GameInstance",
                table: "Bags");

            migrationBuilder.DropForeignKey(
                name: "FK_Bags_Instances_InstanceId",
                schema: "GameInstance",
                table: "Bags");

            migrationBuilder.DropForeignKey(
                name: "FK_Bodies_Characters_CharacterId",
                schema: "GameInstance",
                table: "Bodies");

            migrationBuilder.DropForeignKey(
                name: "FK_Bodies_Items_ItemId",
                schema: "GameInstance",
                table: "Bodies");

            migrationBuilder.DropForeignKey(
                name: "FK_Bodies_Slots_SlotId",
                schema: "GameInstance",
                table: "Bodies");

            migrationBuilder.DropForeignKey(
                name: "FK_Characters_CharacterTemplates_CharacterTemplateId",
                schema: "GameInstance",
                table: "Characters");

            migrationBuilder.DropForeignKey(
                name: "FK_Characters_ImgFiles_ImgFileId",
                schema: "GameInstance",
                table: "Characters");

            migrationBuilder.DropForeignKey(
                name: "FK_Characters_Instances_InstanceId",
                schema: "GameInstance",
                table: "Characters");

            migrationBuilder.DropForeignKey(
                name: "FK_Characters_TextFiles_TextFileId",
                schema: "GameInstance",
                table: "Characters");

            migrationBuilder.DropForeignKey(
                name: "FK_Characters_UserGameCharacters_UserGameActivatedId",
                schema: "GameInstance",
                table: "Characters");

            migrationBuilder.DropTable(
                name: "CharacterModifiers",
                schema: "GameInstance");

            migrationBuilder.DropTable(
                name: "Instances",
                schema: "GameInstance");

            migrationBuilder.DropTable(
                name: "UserGameCharacters",
                schema: "GameInstance");

            migrationBuilder.DropIndex(
                name: "IX_Characters_CharacterTemplateId",
                schema: "GameInstance",
                table: "Characters");

            migrationBuilder.DropIndex(
                name: "IX_Characters_ImgFileId",
                schema: "GameInstance",
                table: "Characters");

            migrationBuilder.DropIndex(
                name: "IX_Characters_InstanceId",
                schema: "GameInstance",
                table: "Characters");

            migrationBuilder.DropIndex(
                name: "IX_Characters_TextFileId",
                schema: "GameInstance",
                table: "Characters");

            migrationBuilder.DropIndex(
                name: "IX_Characters_UserGameActivatedId",
                schema: "GameInstance",
                table: "Characters");

            migrationBuilder.DropIndex(
                name: "IX_Bags_CharacterId",
                schema: "GameInstance",
                table: "Bags");

            migrationBuilder.DropIndex(
                name: "IX_Bags_ImgFileId",
                schema: "GameInstance",
                table: "Bags");

            migrationBuilder.DropIndex(
                name: "IX_Bags_InstanceId",
                schema: "GameInstance",
                table: "Bags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Bodies",
                schema: "GameInstance",
                table: "Bodies");

            migrationBuilder.DropIndex(
                name: "IX_Bodies_ItemId",
                schema: "GameInstance",
                table: "Bodies");

            migrationBuilder.DropColumn(
                name: "ActivationTime",
                schema: "GameInstance",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "CharacterTemplateId",
                schema: "GameInstance",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "ImgFileId",
                schema: "GameInstance",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "InstanceId",
                schema: "GameInstance",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "TextFileId",
                schema: "GameInstance",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "UserGameActivatedId",
                schema: "GameInstance",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "CharacterId",
                schema: "GameInstance",
                table: "Bags");

            migrationBuilder.DropColumn(
                name: "ImgFileId",
                schema: "GameInstance",
                table: "Bags");

            migrationBuilder.DropColumn(
                name: "InstanceId",
                schema: "GameInstance",
                table: "Bags");

            migrationBuilder.DropColumn(
                name: "ItemId",
                schema: "GameInstance",
                table: "Bodies");

            migrationBuilder.RenameTable(
                name: "Characters",
                schema: "GameInstance",
                newName: "Characters");

            migrationBuilder.RenameTable(
                name: "CharacterParameters",
                schema: "GameInstance",
                newName: "CharacterParameters");

            migrationBuilder.RenameTable(
                name: "Bags",
                schema: "GameInstance",
                newName: "Bags");

            migrationBuilder.RenameTable(
                name: "BagItems",
                schema: "GameInstance",
                newName: "BagItems");

            migrationBuilder.RenameTable(
                name: "Bodies",
                schema: "GameInstance",
                newName: "Body");

            migrationBuilder.RenameColumn(
                name: "ParametrValue",
                table: "CharacterParameters",
                newName: "ParameterValue");

            migrationBuilder.RenameIndex(
                name: "IX_Bodies_SlotId",
                table: "Body",
                newName: "IX_Body_SlotId");

            migrationBuilder.RenameIndex(
                name: "IX_Bodies_CharacterId",
                table: "Body",
                newName: "IX_Body_CharacterId");

            migrationBuilder.AlterTable(
                name: "Characters",
                oldComment: "Персонажи");

            migrationBuilder.AlterTable(
                name: "CharacterParameters",
                oldComment: "Параметры персонажа");

            migrationBuilder.AlterTable(
                name: "Bags",
                oldComment: "Сумки");

            migrationBuilder.AlterTable(
                name: "BagItems",
                oldComment: "Предметы в сумке");

            migrationBuilder.AlterTable(
                name: "Body",
                oldComment: "Тела");

            migrationBuilder.AlterColumn<Guid>(
                name: "ImgFileId",
                schema: "GameRules",
                table: "CharacterTemplates",
                type: "uuid",
                nullable: true,
                comment: "Айди графического файла(аватарки)",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "Айди графического файла");

            migrationBuilder.AddColumn<Guid>(
                name: "CharacterId",
                table: "BodyItem",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "CounterWearing_QuantityItem",
                table: "BodyItem",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CounterWearing_TimeWear",
                table: "BodyItem",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ItemId",
                table: "BodyItem",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Characters",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldComment: "Имя персонажа");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Characters",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "Описание персонажа");

            migrationBuilder.AlterColumn<Guid>(
                name: "BagId",
                table: "Characters",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "Айди сумки");

            migrationBuilder.AddColumn<bool>(
                name: "CharacterInInteraction",
                table: "Characters",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "GameId",
                table: "Characters",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "TimeActivate",
                table: "Characters",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TypeCharacter",
                table: "Characters",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "UserActivateId",
                table: "Characters",
                type: "uuid",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ParameterId",
                table: "CharacterParameters",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "Айди параметра");

            migrationBuilder.AlterColumn<Guid>(
                name: "CharacterId",
                table: "CharacterParameters",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "Айди персонажа");

            migrationBuilder.AlterColumn<double>(
                name: "ParameterValue",
                table: "CharacterParameters",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldComment: "Значение параметра");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Bags",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldComment: "Название сумки");

            migrationBuilder.AlterColumn<int>(
                name: "MaxBagSize",
                table: "Bags",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true,
                oldComment: "Максимальный размер сумки");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Bags",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "Описание сумки");

            migrationBuilder.AddColumn<Guid>(
                name: "GameId",
                table: "Bags",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "MaxOccupiedBagSize",
                table: "Bags",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Stack",
                table: "BagItems",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldComment: "Стек");

            migrationBuilder.AlterColumn<int>(
                name: "QuantityItem",
                table: "BagItems",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldComment: "Количество предметов в стеке");

            migrationBuilder.AlterColumn<Guid>(
                name: "ItemId",
                table: "BagItems",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "Айди предмета");

            migrationBuilder.AlterColumn<Guid>(
                name: "BagId",
                table: "BagItems",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "Айди сумки");

            migrationBuilder.AlterColumn<Guid>(
                name: "SlotId",
                table: "Body",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "Айди слота");

            migrationBuilder.AlterColumn<Guid>(
                name: "CharacterId",
                table: "Body",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "Айди персонажа");

            migrationBuilder.AddColumn<int>(
                name: "BodySlotActivity_CounterWearing",
                table: "Body",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "BodySlotActivity_InActivationTime",
                table: "Body",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BodySlotActivity_MaxQuantityWearing",
                table: "Body",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BodySlotActivity_PeriodOfInactivity",
                table: "Body",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaxQuantityInSlot",
                table: "Body",
                type: "integer",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Body",
                table: "Body",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "CharacterCharacteristic",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    CharacterId = table.Column<Guid>(type: "uuid", nullable: false),
                    CharacteristicId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterCharacteristic", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CharacterCharacteristic_Characteristics_CharacteristicId",
                        column: x => x.CharacteristicId,
                        principalSchema: "InteractionRules",
                        principalTable: "Characteristics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterCharacteristic_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CharacteristicModifier",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    CharacteristicId = table.Column<Guid>(type: "uuid", nullable: false),
                    ModifierId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacteristicModifier", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CharacteristicModifier_Characteristics_CharacteristicId",
                        column: x => x.CharacteristicId,
                        principalSchema: "InteractionRules",
                        principalTable: "Characteristics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacteristicModifier_Modifiers_ModifierId",
                        column: x => x.ModifierId,
                        principalSchema: "GameRules",
                        principalTable: "Modifiers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ModifierParametrs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    ModifierId = table.Column<Guid>(type: "uuid", nullable: false),
                    ParameterId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifierValue = table.Column<double>(type: "double precision", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModifierParametrs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModifierParametrs_Modifiers_ModifierId",
                        column: x => x.ModifierId,
                        principalSchema: "GameRules",
                        principalTable: "Modifiers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ModifierParametrs_Parameters_ParameterId",
                        column: x => x.ParameterId,
                        principalSchema: "GameRules",
                        principalTable: "Parameters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ParameterItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    ItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    ParameterId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ItemValue = table.Column<double>(type: "double precision", nullable: false),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParameterItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParameterItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalSchema: "GameRules",
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParameterItems_Parameters_ParameterId",
                        column: x => x.ParameterId,
                        principalSchema: "GameRules",
                        principalTable: "Parameters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserCharacters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    CharacterId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCharacters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserCharacters_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserCharacters_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "System",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Scripts_Name",
                schema: "GameRules",
                table: "Scripts",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Parties_Name",
                schema: "InteractionRules",
                table: "Parties",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItemTemplates_Name",
                schema: "GameRules",
                table: "ItemTemplates",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CharacterTemplates_Name",
                schema: "GameRules",
                table: "CharacterTemplates",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BodyItem_CharacterId",
                table: "BodyItem",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_BodyItem_ItemId",
                table: "BodyItem",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationAreas_Name",
                schema: "InteractionRules",
                table: "ApplicationAreas",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Activities_Name",
                schema: "InteractionRules",
                table: "Activities",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Actions_Name",
                schema: "InteractionRules",
                table: "Actions",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Characters_BagId",
                table: "Characters",
                column: "BagId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Characters_GameId",
                table: "Characters",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_UserActivateId",
                table: "Characters",
                column: "UserActivateId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bags_GameId",
                table: "Bags",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterCharacteristic_CharacterId",
                table: "CharacterCharacteristic",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterCharacteristic_CharacteristicId",
                table: "CharacterCharacteristic",
                column: "CharacteristicId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacteristicModifier_CharacteristicId",
                table: "CharacteristicModifier",
                column: "CharacteristicId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacteristicModifier_ModifierId",
                table: "CharacteristicModifier",
                column: "ModifierId");

            migrationBuilder.CreateIndex(
                name: "IX_ModifierParametrs_ModifierId",
                table: "ModifierParametrs",
                column: "ModifierId");

            migrationBuilder.CreateIndex(
                name: "IX_ModifierParametrs_ParameterId",
                table: "ModifierParametrs",
                column: "ParameterId");

            migrationBuilder.CreateIndex(
                name: "IX_ParameterItems_ItemId",
                table: "ParameterItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ParameterItems_ParameterId",
                table: "ParameterItems",
                column: "ParameterId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCharacters_CharacterId",
                table: "UserCharacters",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCharacters_UserId",
                table: "UserCharacters",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bags_Games_GameId",
                table: "Bags",
                column: "GameId",
                principalSchema: "BaseGame",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Body_Characters_CharacterId",
                table: "Body",
                column: "CharacterId",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Body_Slots_SlotId",
                table: "Body",
                column: "SlotId",
                principalSchema: "GameRules",
                principalTable: "Slots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BodyItem_Characters_CharacterId",
                table: "BodyItem",
                column: "CharacterId",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BodyItem_Items_ItemId",
                table: "BodyItem",
                column: "ItemId",
                principalSchema: "GameRules",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_Bags_BagId",
                table: "Characters",
                column: "BagId",
                principalTable: "Bags",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_Games_GameId",
                table: "Characters",
                column: "GameId",
                principalSchema: "BaseGame",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_UserCharacters_UserActivateId",
                table: "Characters",
                column: "UserActivateId",
                principalTable: "UserCharacters",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
