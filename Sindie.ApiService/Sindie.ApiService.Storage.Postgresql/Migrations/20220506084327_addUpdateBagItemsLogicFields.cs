using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Sindie.ApiService.Storage.Postgresql.Migrations
{
    public partial class addUpdateBagItemsLogicFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Notifications");

            migrationBuilder.AddColumn<int>(
                name: "MaxQuantity",
                schema: "GameRules",
                table: "ItemTemplates",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "Максимальное количество в стаке");

            migrationBuilder.AddColumn<double>(
                name: "Weight",
                schema: "GameRules",
                table: "ItemTemplates",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                comment: "Вес");

            migrationBuilder.AddColumn<int>(
                name: "MaxQuantity",
                schema: "GameRules",
                table: "Items",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "Максимальное количество в стаке");

            migrationBuilder.AddColumn<double>(
                name: "Weight",
                schema: "GameRules",
                table: "Items",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                comment: "Вес");

            migrationBuilder.AddColumn<double>(
                name: "MaxWeight",
                schema: "GameInstance",
                table: "Bags",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Blocked",
                schema: "GameInstance",
                table: "BagItems",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "Заблокировано");

            migrationBuilder.AddColumn<int>(
                name: "MaxQuantityItem",
                schema: "GameInstance",
                table: "BagItems",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "Максимальное количество в стаке");

            migrationBuilder.AddColumn<double>(
                name: "Weight",
                schema: "GameInstance",
                table: "BagItems",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                comment: "Вес");

            migrationBuilder.CreateTable(
                name: "Notifications",
                schema: "Notifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Название"),
                    Message = table.Column<string>(type: "text", nullable: false, comment: "Сообщение"),
                    Duration = table.Column<int>(type: "integer", nullable: false, comment: "Длительность существования в минутах"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleCreatedUser = table.Column<string>(type: "text", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleModifiedUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                },
                comment: "Уведомления");

            migrationBuilder.CreateTable(
                name: "NotificationsDeletedItems",
                schema: "Notifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    BagId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди сумки")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationsDeletedItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationsDeletedItems_Bags_BagId",
                        column: x => x.BagId,
                        principalSchema: "GameInstance",
                        principalTable: "Bags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NotificationsDeletedItems_Notifications_Id",
                        column: x => x.Id,
                        principalSchema: "Notifications",
                        principalTable: "Notifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Модификаторы");

            migrationBuilder.CreateTable(
                name: "NotificationTradeRequests",
                schema: "Notifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)"),
                    ReceiveBagId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди сумки-получателя"),
                    ReceiveCharacterId = table.Column<Guid>(type: "uuid", nullable: true),
                    SourceBagId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди сумки-отправителя")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationTradeRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationTradeRequests_Bags_ReceiveBagId",
                        column: x => x.ReceiveBagId,
                        principalSchema: "GameInstance",
                        principalTable: "Bags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_NotificationTradeRequests_Bags_SourceBagId",
                        column: x => x.SourceBagId,
                        principalSchema: "GameInstance",
                        principalTable: "Bags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NotificationTradeRequests_Notifications_Id",
                        column: x => x.Id,
                        principalSchema: "Notifications",
                        principalTable: "Notifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Уведомления о намерении передать предметы");

            migrationBuilder.CreateTable(
                name: "UserNotications",
                schema: "Notifications",
                columns: table => new
                {
                    NotificationsId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReceiversId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserNotications", x => new { x.NotificationsId, x.ReceiversId });
                    table.ForeignKey(
                        name: "FK_UserNotications_Notifications_NotificationsId",
                        column: x => x.NotificationsId,
                        principalSchema: "Notifications",
                        principalTable: "Notifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserNotications_Users_ReceiversId",
                        column: x => x.ReceiversId,
                        principalSchema: "System",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NotificationTradeRequestItem",
                schema: "Notifications",
                columns: table => new
                {
                    NotificationTradeRequestId = table.Column<Guid>(type: "uuid", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ItemId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Айди предмета"),
                    ItemName = table.Column<string>(type: "text", nullable: true, comment: "Название предмета"),
                    QuantityItem = table.Column<int>(type: "integer", nullable: false, comment: "Количество предметов"),
                    MaxQuantityItem = table.Column<int>(type: "integer", nullable: false, comment: "Максимальное количество предметов"),
                    TotalWeight = table.Column<double>(type: "double precision", nullable: false, comment: "Общий вес стака"),
                    Stack = table.Column<int>(type: "integer", nullable: false, comment: "Стак")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationTradeRequestItem", x => new { x.NotificationTradeRequestId, x.Id });
                    table.ForeignKey(
                        name: "FK_NotificationTradeRequestItem_NotificationTradeRequests_Noti~",
                        column: x => x.NotificationTradeRequestId,
                        principalSchema: "Notifications",
                        principalTable: "NotificationTradeRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NotificationsDeletedItems_BagId",
                schema: "Notifications",
                table: "NotificationsDeletedItems",
                column: "BagId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationTradeRequests_ReceiveBagId",
                schema: "Notifications",
                table: "NotificationTradeRequests",
                column: "ReceiveBagId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationTradeRequests_SourceBagId",
                schema: "Notifications",
                table: "NotificationTradeRequests",
                column: "SourceBagId");

            migrationBuilder.CreateIndex(
                name: "IX_UserNotications_ReceiversId",
                schema: "Notifications",
                table: "UserNotications",
                column: "ReceiversId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NotificationsDeletedItems",
                schema: "Notifications");

            migrationBuilder.DropTable(
                name: "NotificationTradeRequestItem",
                schema: "Notifications");

            migrationBuilder.DropTable(
                name: "UserNotications",
                schema: "Notifications");

            migrationBuilder.DropTable(
                name: "NotificationTradeRequests",
                schema: "Notifications");

            migrationBuilder.DropTable(
                name: "Notifications",
                schema: "Notifications");

            migrationBuilder.DropColumn(
                name: "MaxQuantity",
                schema: "GameRules",
                table: "ItemTemplates");

            migrationBuilder.DropColumn(
                name: "Weight",
                schema: "GameRules",
                table: "ItemTemplates");

            migrationBuilder.DropColumn(
                name: "MaxQuantity",
                schema: "GameRules",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Weight",
                schema: "GameRules",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "MaxWeight",
                schema: "GameInstance",
                table: "Bags");

            migrationBuilder.DropColumn(
                name: "Blocked",
                schema: "GameInstance",
                table: "BagItems");

            migrationBuilder.DropColumn(
                name: "MaxQuantityItem",
                schema: "GameInstance",
                table: "BagItems");

            migrationBuilder.DropColumn(
                name: "Weight",
                schema: "GameInstance",
                table: "BagItems");
        }
    }
}
