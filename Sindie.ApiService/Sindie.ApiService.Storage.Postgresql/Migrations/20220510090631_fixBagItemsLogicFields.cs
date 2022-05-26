using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sindie.ApiService.Storage.Postgresql.Migrations
{
    public partial class fixBagItemsLogicFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NotificationTradeRequests_Bags_ReceiveBagId",
                schema: "Notifications",
                table: "NotificationTradeRequests");

            migrationBuilder.DropIndex(
                name: "IX_NotificationTradeRequests_ReceiveBagId",
                schema: "Notifications",
                table: "NotificationTradeRequests");

            migrationBuilder.AddColumn<Guid>(
                name: "BagId",
                schema: "Notifications",
                table: "NotificationTradeRequests",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_NotificationTradeRequests_BagId",
                schema: "Notifications",
                table: "NotificationTradeRequests",
                column: "BagId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationTradeRequests_ReceiveCharacterId",
                schema: "Notifications",
                table: "NotificationTradeRequests",
                column: "ReceiveCharacterId");

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationTradeRequests_Bags_BagId",
                schema: "Notifications",
                table: "NotificationTradeRequests",
                column: "BagId",
                principalSchema: "GameInstance",
                principalTable: "Bags",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationTradeRequests_Characters_ReceiveCharacterId",
                schema: "Notifications",
                table: "NotificationTradeRequests",
                column: "ReceiveCharacterId",
                principalSchema: "GameInstance",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NotificationTradeRequests_Bags_BagId",
                schema: "Notifications",
                table: "NotificationTradeRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_NotificationTradeRequests_Characters_ReceiveCharacterId",
                schema: "Notifications",
                table: "NotificationTradeRequests");

            migrationBuilder.DropIndex(
                name: "IX_NotificationTradeRequests_BagId",
                schema: "Notifications",
                table: "NotificationTradeRequests");

            migrationBuilder.DropIndex(
                name: "IX_NotificationTradeRequests_ReceiveCharacterId",
                schema: "Notifications",
                table: "NotificationTradeRequests");

            migrationBuilder.DropColumn(
                name: "BagId",
                schema: "Notifications",
                table: "NotificationTradeRequests");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationTradeRequests_ReceiveBagId",
                schema: "Notifications",
                table: "NotificationTradeRequests",
                column: "ReceiveBagId");

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationTradeRequests_Bags_ReceiveBagId",
                schema: "Notifications",
                table: "NotificationTradeRequests",
                column: "ReceiveBagId",
                principalSchema: "GameInstance",
                principalTable: "Bags",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
